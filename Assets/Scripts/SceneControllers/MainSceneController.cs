using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This is the controller class where we will wire everything together that isn't handled directly by Bolt,
/// or which would be easier or more readable to do here than in bolt. 
/// </summary>
public partial class MainSceneController : MonoBehaviour
{
	[SerializeField] InGameMenuController m_inGameMenu;

	private void Awake()
	{
		SubscribeToInput(); 
	}
}
