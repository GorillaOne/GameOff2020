using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class SimpleShowHideAnimator : MonoBehaviour
{
	[Header("Objects")]
	[SerializeField] private RectTransform m_rectToMove;

	[Header("Positions")]
	[SerializeField] private RectTransform m_onScreenRect;
	[SerializeField] private RectTransform m_offScreenRect;
	[SerializeField] private float m_animationTime;
	[SerializeField] private Ease m_animationEase;

	Sequence seq;

	private Vector3 OnScreenPosition => m_onScreenRect.position;
	private Vector3 OffScreenPosition => m_offScreenRect.position;

	public virtual Sequence Hide()
	{
		seq?.Complete(true); 

		//Moves the menu component to the offscreen position. 
		seq = DOTween.Sequence();
		seq.Append(m_rectToMove.Move(m_offScreenRect, m_onScreenRect, m_animationTime).SetEase(m_animationEase));
		return seq;
	}

	public virtual Sequence Show()
	{
		seq?.Complete(true);

		//Moves the menu component to the onscreen position. 
		seq = DOTween.Sequence();
		seq.Append(m_rectToMove.Move(m_onScreenRect, m_offScreenRect, m_animationTime).SetEase(m_animationEase));		
		return seq;
	}
}
