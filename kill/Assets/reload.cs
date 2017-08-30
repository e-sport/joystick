using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class reload : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

	public void OnPointerDown (PointerEventData eventData)
	{
		InputManager.Shoot.Enqueue (3);
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		InputManager.Shoot.Enqueue (4);
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
