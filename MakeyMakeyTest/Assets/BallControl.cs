using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed;

	[SerializeField] private float rotSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.UpArrow))
		{
			gameObject.transform.position += transform.forward * moveSpeed * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			gameObject.transform.Rotate(0f, rotSpeed * Time.deltaTime, 0f);
		}
		
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			gameObject.transform.Rotate(0f, -rotSpeed * Time.deltaTime, 0f);
		}
		
	}
}
