﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTracer : MonoBehaviour
{

    public float resolution = 1.0f;
    public float maxRayCastDist = 50.0f;
    public int maxRayReflections = 1;
    public Color backgroundColor = Color.cyan;
	public Camera mainCam;

	private Light[] lights;
    private Texture2D renderTexture;
    private int textureWidth;
    private int textureHeight;

    private void Awake()
    {
        textureWidth = (int)(Screen.width * resolution);
        textureHeight = (int)(Screen.height * resolution);
        renderTexture = new Texture2D(textureWidth, textureHeight);
    }


    // Use this for initialization
    void Start()
    {
		if (mainCam == null) {
			mainCam = GetComponent<Camera> ();
		}

		lights = Light.FindObjectsOfType(typeof(Light)) as Light[];



    }

    private void Update()
    {
        
    }

	private void LateUpdate() 
	{
		RasterizeRenderTexture();

	}

    private void RasterizeRenderTexture()
    {
        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                Vector3 rayStartPos = new Vector3(x / resolution, y / resolution, 0);
                Ray ray = mainCam.ScreenPointToRay(rayStartPos);
                renderTexture.SetPixel(x, y, RayTrace(ray, backgroundColor, 0));
            }
        }

        renderTexture.Apply();

    }
		
    // Determines the color of the pixel at the intersection of the ray
	private Color RayTrace(Ray ray, Color positionColor, int currentRefIteration)
    {

		if (currentRefIteration < maxRayReflections) {
			RaycastHit hit;

			// Check if there is a collision
			if (Physics.Raycast (ray, out hit, maxRayCastDist)) {
				// If there is a hit, then add a 1 to opimize matrix
				Vector3 viewVector = ray.direction * -1;
				Vector3 hitPoint = hit.point;
				Vector3 hitNormal = hit.normal;
				hitNormal.Normalize ();
				// Get the basic material color of the object
				GameObject hitObj = hit.collider.gameObject;
				RayTracerObject rto = hit.collider.gameObject.GetComponent<RayTracerObject> ();

				// Ambient calc
				Color La = rto.ambientColor * rto.objectColor * rto.Ka;
			
				Color Ld = Color.black;
				Color Ls = Color.black;

				// Apply some phong shading per light in scene if there are lights in the scene
				if (lights.Length > 0) {
					foreach (Light light in lights) {
						// Getting Light color
						Color Lc = light.color;
						Vector3 lightDir = light.transform.position - hitPoint;
						lightDir.Normalize ();

						// Diffuse calc
						Ld += Lc * Mathf.Max (0f, Vector3.Dot (lightDir, hitNormal)) * rto.objectColor;


						// Specular calculations
						Vector3 R = Vector3.Reflect (-1 * lightDir, hitNormal);
						R.Normalize ();
						Vector3 Refl = lightDir - (hitNormal * Vector3.Dot (lightDir, hitNormal) * 2);
						Refl.Normalize ();

						Ls += Lc * Mathf.Max (0f, Mathf.Pow (Vector3.Dot (R, viewVector.normalized), rto.Ke));

					}
				}
				positionColor = La + (rto.Kd * Ld) + (rto.Ks * Ls);

			}
		} 

        return positionColor;
    }

    private void OnGUI()
    {
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTexture);
    }
}
