using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PortalEffect : MonoBehaviour
{
    public Transform playerCamera, empty, thisCamera;
	public Transform portal;
	public Transform otherPortal;
	public Vector3 cameraOffset;
	public bool reverseInZ;
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
		if(!reverseInZ) empty.position = portal.position + new Vector3(playerOffsetFromPortal.x, playerOffsetFromPortal.y, playerOffsetFromPortal.z);
		else empty.position = portal.position + new Vector3(-playerOffsetFromPortal.z, -playerOffsetFromPortal.y, playerOffsetFromPortal.x);

		float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

		Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
		Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
		// if(cameraOffset.y == 180)
		// {
		// 	float newXFloat = newCameraDirection.x * -1;
		// 	float newYFloat = newCameraDirection.y * -1;
		// 	newCameraDirection.x = newXFloat;
		// 	newCameraDirection.y = newYFloat;
		// }
		empty.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);

		thisCamera.position = empty.position;
		thisCamera.rotation = empty.rotation;

		Quaternion currentRotation = thisCamera.rotation;
		Quaternion yRotation = Quaternion.Euler(0, -180, 0);
		//Quaternion yRotation = currentRotation;


		thisCamera.rotation = yRotation * currentRotation;
	}

	
}
