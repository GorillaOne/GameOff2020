using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTactics.Factories
{
	public static class GameObjectFactory<T> where T : MonoBehaviour, IResettable, IDestroyable
	{
		public static event Action<T> ObjectAdded;
		public static event Action<T> ObjectRemoved;

		private static Queue<T> cachedObjects = new Queue<T>();
		private static HashSet<List<T>> managers = new HashSet<List<T>>();
		private static GameObject prefab;
		private static Transform deadObjectContainer;

		public static void Initialize(List<T> manager, GameObject prefab, Transform deadObjectContainer = null)
		{
			GameObjectFactory<T>.prefab = prefab;
			if (!managers.Add(manager))
			{
				Debug.LogError("Trying to add a manger list to the factory twice.");
			}
			GameObjectFactory<T>.deadObjectContainer = deadObjectContainer;
		}

		public static void AddManager(List<T> manager)
		{
			if (!managers.Add(manager))
			{
				Debug.LogError("Trying to add a manger list to the factory twice.");
			}
		}

		public static void RemoveManager(List<T> manager)
		{
			if (!managers.Remove(manager))
			{
				Debug.LogError("Trying to remove a manger list from the factory which has not been added.");
			}
		}
		public static T CreateNew()
		{
			//Edge case where we have not initialized the factory with a prefab.
			if (prefab == null)
			{
				Debug.LogError("Factory must be instantiated before use. No prefab set.");
			}

			if (managers.Count == 0)
			{
				Debug.LogError($"Factory for {typeof(T).ToString()} must be instantiated before use. No managers added.");
			}

			T script;
			if (cachedObjects.Count > 0)
			{
				script = cachedObjects.Dequeue();
				script.gameObject.SetActive(true);
				script.Reset();
			}
			else
			{
				var newObj = GameObject.Instantiate(prefab);
				script = FindScript(newObj);
				script.Destroyed += Script_Destroyed;
			}

			foreach (var manager in managers)
			{
				manager.Add(script);
			}

			ObjectAdded?.Invoke(script);
			return script;
		}

		public static void Destroy(T obj)
		{
			Script_Destroyed(obj);
		}
		private static void Script_Destroyed(object obj)
		{
			if (obj is T script)
			{
				if (cachedObjects.Contains(script))
				{
					Debug.Log("Trying to destroy an object that has already been destroyed."); 
				}
				else
				{
					foreach (var manager in managers)
					{
						manager.Remove(script);
					}
					script.transform.SetParent(deadObjectContainer);
					script.gameObject.SetActive(false);
					cachedObjects.Enqueue(script);
					ObjectRemoved?.Invoke(script);
				}
			}
			else
			{
				Debug.LogError($"Somehow this factory subscribed to the Destroy event on an object of type {obj.GetType()} when it was supposed to be of type {typeof(T)}");
			}
		}

		private static T FindScript(GameObject obj)
		{
			T script = obj.GetComponent<T>();
			if (script != null) return script;
			else
			{
				return obj.GetComponentInChildren<T>();
			}
		}
	}
}