using UnityEngine;
using System.Collections;

public class RayTracerObject : MonoBehaviour {

	public Color ambientColor = Color.red;
	public Color objectColor;
	public Color specColor = Color.white;


	public float Ka = 0.2f;
	public float Kd = 0.5f;
	public float Ks = 0.4f;
	public float Ke = 10000.0f;

	private void Start() {
		objectColor = GetComponent<Renderer> ().material.color;
	}

}