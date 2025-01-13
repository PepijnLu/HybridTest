using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
//using UnityEngine.XR.Oculus;

public class PortalTextureSetup : MonoBehaviour 
{

	public Camera cameraA;
	public Camera cameraB;
	public Camera cameraC;
	public Camera cameraD;
	public Camera windowCamera;

	public Material cameraMatA;
	public Material cameraMatB;
	public Material cameraMatC;
	public Material cameraMatD;
	public Material windowCameraMat;

	// Use this for initialization
	void Start () {

		XRSettings.eyeTextureResolutionScale = 0.8f; // Adjust scale here
		//OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.Medium;

		//float[] refreshRates = OVRPlugin.systemDisplayFrequenciesAvailable;
        //OVRPlugin.systemDisplayFrequency = 72.0f; // Set the desired refresh rate (72Hz, 90Hz, etc.)



		if(SceneManager.GetActiveScene().name == "EuclidianHallway")
		{
			if (cameraA.targetTexture != null)
			{
				cameraA.targetTexture.Release();
			}
			cameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
			cameraMatA.mainTexture = cameraA.targetTexture;

			if (cameraB.targetTexture != null)
			{
				cameraB.targetTexture.Release();
			}
			cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
			cameraMatB.mainTexture = cameraB.targetTexture;

			if (cameraC.targetTexture != null)
			{
				cameraC.targetTexture.Release();
			}
			cameraC.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
			cameraMatC.mainTexture = cameraC.targetTexture;

			if (cameraD.targetTexture != null)
			{
				cameraD.targetTexture.Release();
			}
			cameraD.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
			cameraMatD.mainTexture = cameraD.targetTexture;
		}

		if(SceneManager.GetActiveScene().name == "Painting")
		{
			if (windowCamera.targetTexture != null)
			{
				windowCamera.targetTexture.Release();
			}
			windowCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
			windowCameraMat.mainTexture = windowCamera.targetTexture;
		}

		if(SceneManager.GetActiveScene().name == "Museum")
		{
			if (windowCamera.targetTexture != null)
			{
				windowCamera.targetTexture.Release();
			}
			windowCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
			windowCameraMat.mainTexture = windowCamera.targetTexture;
		}
	}
	
}
