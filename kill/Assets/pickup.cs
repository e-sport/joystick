using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class pickup : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	InputManager.VirtualButton m_VirtualButton;

	public void OnPointerDown (PointerEventData eventData)
	{
		m_VirtualButton.Pressed ();
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		m_VirtualButton.Released ();
	}

	// Use this for initialization
	void Start ()
	{
		m_VirtualButton = new InputManager.VirtualButton ("Pickup");
		InputManager.RegisterVirtualButton (m_VirtualButton);
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
