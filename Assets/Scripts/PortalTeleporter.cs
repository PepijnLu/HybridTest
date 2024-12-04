using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform reciever;
    public PlayerController playerController;
    public string thisPortal;
    [HideInInspector] public string exitPortal;

    void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == "TPCheck")
		{
            if(!playerController.justTeleported)
            {
                // Teleport him!
                Vector3 portalToPlayer = playerController.gameObject.transform.position - transform.position;
				float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
				rotationDiff += 180;
				playerController.gameObject.transform.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;

				Vector3 newPosition = reciever.position + positionOffset;
                newPosition.y = playerController.gameObject.transform.position.y;

                Debug.Log($"Teleported: from {playerController.gameObject.transform.position} to {newPosition}");
                playerController.gameObject.transform.position = newPosition;

				playerController.justTeleported = true;
                
                PortalTeleporter recieverPortal = reciever.gameObject.GetComponent<PortalTeleporter>();

                switch(thisPortal)
                {
                    case "PortalA":
                        recieverPortal.exitPortal = "PortalB";
                        break;
                    case "PortalB":
                        recieverPortal.exitPortal = "PortalA";
                        break;
                    case "PortalC":
                        recieverPortal.exitPortal = "PortalD";
                        break;
                    case "PortalD":
                        recieverPortal.exitPortal = "PortalC";
                        break;
                    
                }
            }
		}
	}

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "TPCheck")
		{
            if(playerController.justTeleported && (exitPortal == thisPortal))
            {
                playerController.justTeleported = false;
                exitPortal = "";
            }
		}
    }
}
