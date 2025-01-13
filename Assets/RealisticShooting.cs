using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RealisticShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float fireRate = .1f; // Time between shots in seconds
    public int magazineSize = 30; // Bullets in one magazine
    public float reloadTime = 2f; // Time to reload in seconds
    public float recoilForce = .1f; // Recoil kickback distance
    public float spreadAngle = 1f; // Bullet spread in degrees
    public float resetDuration = .5f;

    [Header("References")]
    public Transform shootPoint; // Point from which bullets are fired
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform gunModelTransform; // Transform of the gun model attached to the controller
    public Transform controllerTransform; // Transform of the controller

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public AudioSource gunshotSound;
    public AudioSource reloadSound;

    private int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    private Vector3 initialGunPosition; // To store the initial position of the gun
    private Quaternion initialGunRotation; // To store the initial rotation of the gun
    private Vector3 recoilOffset; // To store the recoil offset

    public XRGrabInteractable xRGrabInteractable;

    bool isShooting = false;

    void Start()
    {
        currentAmmo = magazineSize; // Start with a full magazine
        xRGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        if (gunModelTransform != null && controllerTransform != null)
        {
            if(xRGrabInteractable.isSelected) 
            {
                Debug.Log("Grab is active");

                // gunModelTransform.SetParent(controllerTransform);
                // // Update the gun's position to match the controller's position in real-time
                gunModelTransform.position = controllerTransform.position;

                // // Update the gun's rotation to match the controller's rotation in real-time
                gunModelTransform.rotation = controllerTransform.rotation;

            }
        }
    }

    public void SetChild() 
    {
        gunModelTransform.SetParent(controllerTransform);
    }

    public void Undo() 
    {
        gunModelTransform.SetParent(null);
    }

    public void Shoot()
    {
        if (isReloading || Time.time < nextFireTime || currentAmmo <= 0)
            return;

        isShooting = true;
        
        nextFireTime = Time.time + fireRate;
        currentAmmo--;

        // Play shooting effects
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (gunshotSound != null)
            gunshotSound.Play();

        // Instantiate and shoot the bullet
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        // Apply spread
        float spreadX = Random.Range(-spreadAngle, spreadAngle);
        float spreadY = Random.Range(-spreadAngle, spreadAngle);
        Vector3 spread = new(spreadX, spreadY, 0);
        bullet.transform.Rotate(spread);

        // Recoil kickback
        ApplyRecoil();
    }

    public void Reload()
    {
        if (isReloading || currentAmmo == magazineSize)
            return;

        StartCoroutine(ReloadCoroutine());
    }

    private void ApplyRecoil()
    {
        if (gunModelTransform != null)
        {

            if(gunModelTransform != null)
            {
                initialGunPosition = gunModelTransform.position; 
                initialGunRotation = gunModelTransform.rotation;
            }

            // Apply recoil by moving the gun backward (in world space)
            recoilOffset = gunModelTransform.forward * recoilForce;

            gunModelTransform.position -= recoilOffset; // Move the gun back
            gunModelTransform.rotation *= Quaternion.Euler(-recoilForce, 0, 0); // Slight rotation for recoil effect

            StartCoroutine(ResetGunPosition());
        }
    }

    private IEnumerator ResetGunPosition()
    {
        float elapsedTime = 0f;

        // Smoothly return to the original position and rotation
        while (elapsedTime < resetDuration)
        {
            gunModelTransform.position = Vector3.Lerp(
                gunModelTransform.position,
                initialGunPosition,
                elapsedTime / resetDuration
            );

            gunModelTransform.rotation = Quaternion.Slerp(
                gunModelTransform.rotation,
                initialGunRotation,
                elapsedTime / resetDuration
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the gun's position and rotation are exactly reset
        gunModelTransform.position = initialGunPosition;
        gunModelTransform.rotation = initialGunRotation;
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        // Play reload sound
        if (reloadSound != null)
            reloadSound.Play();

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;
        isReloading = false;
    }

    // Draw ammo status on the screen
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"Ammo: {currentAmmo} / {magazineSize}");
    }
}


// -------------------------------------------------
// public class RealisticShooting : MonoBehaviour
// {
//     [Header("Shooting Settings")]
//     public float fireRate = .1f; // Time between shots in seconds
//     public int magazineSize = 30; // Bullets in one magazine
//     public float reloadTime = 2f; // Time to reload in seconds
//     public float recoilForce = .1f; // Recoil kickback distance
//     public float spreadAngle = 1f; // Bullet spread in degrees
//     public float resetDuration = .5f;

//     [Header("References")]
//     public Transform shootPoint; // Point from which bullets are fired
//     public GameObject bulletPrefab; // Prefab for the bullet
//     public Transform gunModelTransform; // Transform of the gun model attached to the controller

//     [Header("Effects")]
//     public ParticleSystem muzzleFlash;
//     public AudioSource gunshotSound;
//     public AudioSource reloadSound;

//     private int currentAmmo;
//     private bool isReloading = false;
//     private float nextFireTime = 0f;

//     private Vector3 initialGunPosition; // To store the initial position of the gun
//     private Quaternion initialGunRotation; // To store the initial rotation of the gun

//     void Start()
//     {
//         currentAmmo = magazineSize; // Start with a full magazine
//     }

//     public void Shoot()
//     {
//         if (isReloading || Time.time < nextFireTime || currentAmmo <= 0)
//             return;
 
//         nextFireTime = Time.time + fireRate;
//         currentAmmo--;

//         // Play shooting effects
//         if (muzzleFlash != null)
//             muzzleFlash.Play();

//         if (gunshotSound != null)
//             gunshotSound.Play();

//         // Instantiate and shoot the bullet
//         GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

//         // Apply spread
//         float spreadX = Random.Range(-spreadAngle, spreadAngle);
//         float spreadY = Random.Range(-spreadAngle, spreadAngle);
//         Vector3 spread = new(spreadX, spreadY, 0);
//         bullet.transform.Rotate(spread);

//         // Recoil kickback
//         ApplyRecoil();
//     }

//     public void Reload()
//     {
//         if (isReloading || currentAmmo == magazineSize)
//             return;

//         StartCoroutine(ReloadCoroutine());
//     }

//     private void ApplyRecoil()
//     {
//         if (gunModelTransform != null)
//         {

//             if(gunModelTransform != null)
//             {
//                 initialGunPosition = gunModelTransform.position; 
//                 initialGunRotation = gunModelTransform.rotation;
//             }

//             // Log the initial position for debugging
//             Debug.Log($"Initial Gun Position: {initialGunPosition}");

//             // Apply recoil by moving the gun backward (in world space) and slightly rotating it
//             gunModelTransform.position -= gunModelTransform.forward * recoilForce; // Move the gun back
//             gunModelTransform.rotation *= Quaternion.Euler(-recoilForce, 0, 0); // Slight rotation for recoil effect

//             // Log the new position after applying recoil for debugging
//             Debug.Log($"New Gun Position: {gunModelTransform.position}");

//             // Smoothly reset position and rotation back to the initial state
//             StartCoroutine(ResetGunPosition());
//         }
//     }

//     private IEnumerator ResetGunPosition()
//     {
//         float elapsedTime = 0f;

//         // Smoothly return to the original position and rotation
//         while (elapsedTime < resetDuration)
//         {
//             gunModelTransform.position = Vector3.Lerp(
//                 gunModelTransform.position,
//                 initialGunPosition,
//                 elapsedTime / resetDuration
//             );

//             gunModelTransform.rotation = Quaternion.Slerp(
//                 gunModelTransform.rotation,
//                 initialGunRotation,
//                 elapsedTime / resetDuration
//             );

//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }

//         // Ensure the gun's position and rotation are exactly reset
//         gunModelTransform.position = initialGunPosition;
//         gunModelTransform.rotation = initialGunRotation;
//     }

//     private IEnumerator ReloadCoroutine()
//     {
//         isReloading = true;

//         // Play reload sound
//         if (reloadSound != null)
//             reloadSound.Play();

//         yield return new WaitForSeconds(reloadTime);

//         currentAmmo = magazineSize;
//         isReloading = false;
//     }