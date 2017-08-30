using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class shoot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	#region IPointerUpHandler implementation

	public void OnPointerUp (PointerEventData eventData)
	{
		InputManager.Shoot.Enqueue (1);
	}

	#endregion

	#region IPointerDownHandler implementation

	public void OnPointerDown (PointerEventData eventData)
	{
		InputManager.Shoot.Enqueue (0);
	}

	#endregion

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
