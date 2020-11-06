using Bolt;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// A partial class which will contain all methods which respond to input. 
/// </summary>
public partial class MainSceneController
{
	[SerializeField] private PlayerInput m_inputManager;

	public void SubscribeToInput()
	{
		m_inGameMenu.ResumeButton.onClick.AddListener(InGameMenuController_ResumeButton_Clicked);
		m_inGameMenu.QuitToDesktopButton.onClick.AddListener(InGameMenuController_QuitToDesktopButton_Clicked);
	}

	public void ChangeActionMap(ActionMap map)
	{
		m_inputManager.GetComponent<PlayerInput>().SwitchCurrentActionMap(map.ToString()); 
	}

	#region Player Action Map
	public void InputManager_Player_Menu(CallbackContext context)
	{
		if (context.performed)
		{
			CustomEvent.Trigger(gameObject, context.action.name); 
		}
	}
	#endregion

	#region InGameMenu Action Map
	public void InputManager_InGameMenu_Close(CallbackContext context)
	{
		if (context.performed)
		{
			CustomEvent.Trigger(gameObject, context.action.name);
		}		
	}
	#endregion

	public void InGameMenuController_ResumeButton_Clicked()
	{
		CustomEvent.Trigger(gameObject, "ResumeButtonClicked"); 
	}

	public void InGameMenuController_QuitToDesktopButton_Clicked()
	{
#if UNITY_EDITOR
		EditorApplication.ExitPlaymode(); 
# else
		Application.Quit();
#endif

	}
}

public enum ActionMap
{
	Player,
	InGameMenu,
	UI
}
