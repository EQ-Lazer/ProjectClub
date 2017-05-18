using System.Collections.Generic;
using UnityEngine;

public class RayTracerOpti3 : MonoBehaviour
{

	public float resolution = 1.0f;
	public float maxRayCastDist = 50.0f;
	public int maxRayReflections = 1;
	public Color backgroundColor = Color.cyan;
	public Camera mainCam;

	//private GameObject planeTrans;
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

		//planeTrans = GameObject.Find ("Floor");

	}

	private void Update()
	{

	}

	private void LateUpdate() 
	{
		RasterizeRenderTexture();

	}

	// With skip every line optiization
    private void RasterizeRenderTexture()
    {
        for (int y = 0; y < textureHeight; y+=2)
        {
            for (int x = 0; x < textureWidth; x+=2)
            {
                Vector3 rayStartPos = new Vector3(x / resolution, y / resolution, 0);
                Ray ray = mainCam.ScreenPointToRay(rayStartPos);
                renderTexture.SetPixel(x, y, RayTrace(ray, backgroundColor, 0));
            }
        }
		for (int y = 1; y < textureHeight; y+=2)
		{
			for (int x = 1; x < textureWidth; x+=2)
			{
				Vector3 rayStartPos = new Vector3(x / resolution, y / resolution, 0);
				Ray ray = mainCam.ScreenPointToRay(rayStartPos);
				renderTexture.SetPixel(x, y, RayTrace(ray, backgroundColor, 0));
			}
		}
		for (int y = 0; y < textureHeight; y+=2)
		{
			for (int x = 1; x < textureWidth; x+=2)
			{
				Color top = renderTexture.GetPixel (x, y - 1);
				Color bottom = renderTexture.GetPixel (x, y + 1);
				Color left = renderTexture.GetPixel (x-1, y);
				Color right = renderTexture.GetPixel (x+1, y);

				renderTexture.SetPixel(x, y, (top + bottom + left + right) / 4);
			}
		}
		for (int y = 1; y < textureHeight; y+=2)
		{
			for (int x = 0; x < textureWidth; x+=2)
			{
				Color top = renderTexture.GetPixel (x, y - 1);
				Color bottom = renderTexture.GetPixel (x, y + 1);
				Color left = renderTexture.GetPixel (x-1, y);
				Color right = renderTexture.GetPixel (x+1, y);
				renderTexture.SetPixel(x, y, (top + bottom + left + right) / 4);
			}
		}


        renderTexture.Apply();

    }
	/*
	private void RasterizeRenderTexture()
	    {
	        for (int y = 0; y < textureHeight; y+=2)
	        {
	            for (int x = 0; x < textureWidth; x++)
	            {
	                Vector3 rayStartPos = new Vector3(x / resolution, y / resolution, 0);
	                Ray ray = mainCam.ScreenPointToRay(rayStartPos);
	                renderTexture.SetPixel(x, y, RayTrace(ray, backgroundColor, 0));
	            }
	        }
			for (int y = 1; y < textureHeight; y+=2)
			{
				for (int x = 0; x < textureWidth; x++)
				{
					Color top = renderTexture.GetPixel (x, y - 1);
					Color bottom = renderTexture.GetPixel (x, y + 1);
					renderTexture.SetPixel(x, y, (top + bottom) / 2);
				}
			}
	
	
	        renderTexture.Apply();
	
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
	*/


	// Determines the color of the pixel at the intersection of the ray
	private Color RayTrace(Ray ray, Color positionColor, int currentRefIteration)
	{

		if (currentRefIteration < maxRayReflections) {
			RaycastHit hit;

			// Check if there is a collision
			if (Physics.Raycast (ray, out hit, maxRayCastDist) && hit.collider.tag != "PointLight") {
				// If there is a hit, then add a 1 to opimize matrix
				Vector3 viewVector = ray.direction * -1;
				Vector3 hitPoint = hit.point;
				Vector3 hitNormal = hit.normal;

				//Vector3 planeNorm = new Vector3(0, 1, 0);

				if (hitNormal.magnitude != 1)
					hitNormal.Normalize ();
				
				// Get the basic material color of the object
				GameObject hitObj = hit.collider.gameObject;
				RayTracerObject rto = hit.collider.gameObject.GetComponent<RayTracerObject> ();

				// Ambient calc
				Color La = rto.ambientColor * rto.objectColor * rto.Ka;

				Color Ld = Color.black;
				Color Ls = Color.black;

				float LdotN;
				float RdotV;

				// Apply some phong shading per light in scene if there are lights in the scene
				if (lights.Length > 0) {
					foreach (Light light in lights) {
						
						// Getting Light color
						Color Lc = light.color;
						Vector3 lightDir = light.transform.position - hitPoint;
						lightDir.Normalize ();

						if (lightTrace (hitPoint + hitNormal * 0.0001f, lightDir)) {
							// Diffuse calc
							LdotN = Vector3.Dot (lightDir, hitNormal);
							if (LdotN > 0f)
								Ld += Lc * LdotN * rto.objectColor;


							// Specular calculations
							Vector3 R = Vector3.Reflect (-1 * lightDir, hitNormal);

							RdotV = Vector3.Dot (R.normalized, viewVector.normalized);
							if (RdotV > 0f)
								Ls += Lc * Mathf.Pow (RdotV, rto.Ke);
						}

					}
				}
				positionColor = La + (rto.Kd * Ld) + (rto.Ks * Ls);

			}
		} 

		return positionColor;
	}

	private bool lightTrace(Vector3 origin, Vector3 lightDir){
		RaycastHit lightHit;
		Ray lightRay = new Ray (origin, lightDir);
		if (Physics.Raycast (lightRay, out lightHit, maxRayCastDist)) {
			if (lightHit.collider.tag == "PointLight") {
				return true;
			}
			return false;
		}
		return true;
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTexture);
	}
}
