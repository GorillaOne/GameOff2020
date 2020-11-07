using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float m_speed; 
	private Rigidbody2D m_rigidbody2D;

	private void Awake()
	{
		m_rigidbody2D = GetComponent<Rigidbody2D>(); 
	}

	public void Move(Vector2 move)
	{
		m_rigidbody2D.velocity = move * m_speed; 
	}
}
