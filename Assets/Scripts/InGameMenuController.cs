using Bolt;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence; 

public class InGameMenuController : SimpleShowHideAnimator
{
	public Button ResumeButton;
	public Button QuitToDesktopButton;

	[SerializeField] private GameObject m_canvasObj; 

	private void Start()
	{
		//Disabled by default. 
		m_canvasObj.SetActive(false); 
	}

	public override Sequence Show()
	{
		//Turns on the canvas to allow raycast hits. 
		var showAnim = base.Show();
		showAnim.InsertCallback(0, () => { m_canvasObj.SetActive(true); });
		return showAnim;  
	}

	public override Sequence Hide()
	{
		//Also hides the canvas to prevent improper raycast hits or unecessary updates. 
		var hideAnim = base.Hide();
		hideAnim.AppendCallback(() => { m_canvasObj.SetActive(false); });
		return hideAnim; 
	}
}
