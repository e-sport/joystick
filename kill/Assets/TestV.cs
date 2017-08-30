using UnityEngine;
using System.Collections;


public class TestV : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{

		var h = InputManager.GetAxis ("Horizontal");
		var v = InputManager.GetAxis ("Vertical");

		if (h > 0 || v > 0) {
			Debug.Log ("Horizontal: " + h.ToString ());
			Debug.Log ("Vertical: " + v.ToString ());
		}
	}
}
