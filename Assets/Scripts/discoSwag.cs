using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class discoSwag : MonoBehaviour {

	Vector3 pos;
	float randTime;
	private float moveFactor = -0.65f;
	public GameObject example;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		pos = transform.position;

		if (!IsInvoking ("MakeMove")) {
			randTime = Random.Range (0.0f, 0.5f);
			Invoke ("MakeMove", randTime);
		}
	}

	void MakeMove() {
		int move = (int)Random.Range (0, 4);

		switch (move) {
		case 0:
			Move0 ();
			break;
		case 1: 
			Move1 ();
			break;
		case 2:
			Move2 ();
			break;
		case 3:
			Move3 ();
			break;
		case 4:
			Move4 ();
			break;
		}
	}

	public void Move0() {
		transform.position = new Vector3 (pos.x, 0.25f, pos.z);
	}

	public void Move1() {
		transform.position = new Vector3 (pos.x, -0.4f, pos.z);
	}

	public void Move2() {
		transform.position = new Vector3 (pos.x, -1.05f, pos.z);
	}

	public void Move3() {
		transform.position = new Vector3 (pos.x, -1.7f, pos.z);
	}

	public void Move4() {
		transform.position = new Vector3 (pos.x, -2.35f, pos.z);
	}
}
