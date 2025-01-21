// using System.Collections;
// using Unity.XR.CoreUtils;
// using UnityEngine;
// using UnityEngine.InputSystem.XR;

// public class PokeInteractor : MonoBehaviour
// {
//     public GameObject clapBoard;
//     public Vector3 clapBoardOriginalPosition;

//     public GameObject clapboardTopSide; // The object to rotate (e.g., the clapboard itself)
//     public Transform targetPivot; // The pivot point the clapboard will align with
//     public float rotationSpeed = 20.0f; // Speed of the rotation

//     private bool shouldClose = false;

//     // Painting stuff
//     public GameObject layer_1, layer_2;
//     // public GameObject painting, xrRig; 
//     // private TrackedPoseDriver trackedPoseDriver;
//     // [SerializeField] private Camera cam;


//     private void Awake() 
//     {
//         // trackedPoseDriver = cam.GetComponent<TrackedPoseDriver>();
//     }

//     private void Start() 
//     {
//         clapBoardOriginalPosition = clapBoard.transform.position;
//     }

//     public void Poke(bool _bool)
//     {
//         // Trigger rotation if the boolean is true
//         shouldClose = _bool;
//     }


//     /// <summary>
//     /// This is for when the clapboard is grabbed
//     /// </summary>
//     // public void FixCameraTowardsPainting() 
//     // {    
//     //     // Disable the tracking component on the cam
//     //     // Transform cam = xrRig.transform.GetChild(0).transform.GetChild(0);
//     //     // trackedPoseDriver.enabled = false;

//     //     float rotationSpeed = 4f;
        
//     //     // Fetch the position of the painting
//     //     Vector3 directionToPainting = painting.transform.position - cam.transform.position;  

//     //     // Get the desired rotation to face the painting
//     //     Quaternion targetRotation = Quaternion.LookRotation(directionToPainting);
        
//     //     // If the camera isn't facing the painting
//     //     float angleDifference = Quaternion.Angle(cam.transform.rotation, targetRotation);

//     //     if(angleDifference > .1f) 
//     //     {
//     //         // Smoothly rotate the camera to face the painting
//     //         cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
//     //     }
//     // }

//     private void Update()
//     {
//         if (shouldClose)
//         {
//             Debug.Log("Select interaction event executed");
//             // Smoothly rotate the clapboard to align with the target pivot
//             clapboardTopSide.transform.rotation = Quaternion.RotateTowards(
//                 clapboardTopSide.transform.rotation,
//                 targetPivot.rotation,
//                 rotationSpeed * Time.deltaTime
//             );

//             Debug.Log("Clapboard has clapped");

//             // Check if the rotation is complete
//             if (Quaternion.Angle(clapboardTopSide.transform.rotation, targetPivot.rotation) < 0.1f)
//             {
//                 clapboardTopSide.transform.rotation = targetPivot.rotation; // Snap to the final rotation
//                 shouldClose = false; // Stop rotating

//                 Debug.Log("Rotation is complete!");
//                 // StartCoroutine(StopLoopPainting(layer_1, layer_2));

//                 // trackedPoseDriver.enabled = true;
//             }
//         }
//     }

//     private IEnumerator StopLoopPainting(GameObject layer_1, GameObject layer_2) 
//     {
//         // Wait before making changes to the painting
//         yield return new WaitForSeconds(2f);

//         // Disable the current layer
//         layer_1.SetActive(false);

//         // Enable the next layer
//         layer_2.SetActive(true);
        
//         // After changes move the clapboard back
//         StartCoroutine(MoveClapBoardBackToOriginalPosition());
//     }

//     private IEnumerator MoveClapBoardBackToOriginalPosition() 
//     {
//         // Wait before moving
//         yield return new WaitForSeconds(2f);

//         clapBoard.transform.position = Vector3.Lerp(clapBoard.transform.position, clapBoardOriginalPosition, 3f * Time.deltaTime);
//     }
// }

using UnityEngine;

public class ClapBoard : MonoBehaviour
{
    public GameObject clapBoard;
    public Transform hingePoint; // Hinge point (pivot) for rotation
    public Transform targetPivot; // Target rotation for the hinge
    public float rotationSpeed = 20.0f; // Speed of the rotation

    private bool shouldClose = false;
    private Quaternion initialRotation; // Original rotation of the hinge

    private void Start()
    {
        // Store the initial rotation of the hinge point
        if (hingePoint != null)
        {
            initialRotation = hingePoint.rotation;
        }
    }

    public void Poke(bool _bool)
    {
        // Trigger rotation if the boolean is true
        shouldClose = _bool;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space)) 
        {
            Debug.Log("Input detected");
            if (hingePoint != null)
            {
                // Smoothly rotate the hinge point to align with the target pivot
                hingePoint.rotation = Quaternion.RotateTowards(
                    hingePoint.rotation,
                    targetPivot.rotation,
                    rotationSpeed * Time.deltaTime
                );

                // Check if the rotation is complete
                if (Quaternion.Angle(hingePoint.rotation, targetPivot.rotation) < 0.1f)
                {
                    hingePoint.rotation = targetPivot.rotation; // Snap to final rotation
                    shouldClose = false; // Stop rotating
                }
            }
        }
        // if (shouldClose && hingePoint != null)
        // {
        //     // Smoothly rotate the hinge point to align with the target pivot
        //     hingePoint.rotation = Quaternion.RotateTowards(
        //         hingePoint.rotation,
        //         targetPivot.rotation,
        //         rotationSpeed * Time.deltaTime
        //     );

        //     // Check if the rotation is complete
        //     if (Quaternion.Angle(hingePoint.rotation, targetPivot.rotation) < 0.1f)
        //     {
        //         hingePoint.rotation = targetPivot.rotation; // Snap to final rotation
        //         shouldClose = false; // Stop rotating
        //     }
        // }
    }

    public void ResetClapboard()
    {
        // Reset the hinge back to its original rotation
        if (hingePoint != null)
        {
            hingePoint.rotation = initialRotation;
        }
    }
}
