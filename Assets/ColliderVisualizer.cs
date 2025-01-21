using UnityEngine;

[ExecuteInEditMode] // Allows the script to run in the editor
public class ColliderVisualizer : MonoBehaviour
{
    [SerializeField]
    private Color colliderColor = new Color(0, 1, 0, 0.3f); // Default color with transparency

    private void OnDrawGizmos()
    {
        Collider[] colliders = GetComponents<Collider>();

        foreach (Collider collider in colliders)
        {
            Gizmos.color = colliderColor; // Use the customizable color

            if (collider is BoxCollider boxCollider)
            {
                DrawBoxCollider(boxCollider);
            }
            else if (collider is SphereCollider sphereCollider)
            {
                DrawSphereCollider(sphereCollider);
            }
            else if (collider is CapsuleCollider capsuleCollider)
            {
                DrawCapsuleCollider(capsuleCollider);
            }
            else if (collider is MeshCollider meshCollider)
            {
                DrawMeshCollider(meshCollider);
            }
        }
    }

    private void DrawBoxCollider(BoxCollider boxCollider)
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawCube(boxCollider.center, boxCollider.size);
        Gizmos.matrix = Matrix4x4.identity; // Reset the Gizmos matrix
    }

    private void DrawSphereCollider(SphereCollider sphereCollider)
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
        Gizmos.matrix = Matrix4x4.identity; // Reset the Gizmos matrix
    }

    private void DrawCapsuleCollider(CapsuleCollider capsuleCollider)
    {
        Vector3 start = capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);
        Vector3 end = capsuleCollider.center - Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawSphere(start, capsuleCollider.radius);
        Gizmos.DrawSphere(end, capsuleCollider.radius);
        Gizmos.DrawLine(start + Vector3.forward * capsuleCollider.radius, end + Vector3.forward * capsuleCollider.radius);
        Gizmos.DrawLine(start - Vector3.forward * capsuleCollider.radius, end - Vector3.forward * capsuleCollider.radius);
        Gizmos.DrawLine(start + Vector3.right * capsuleCollider.radius, end + Vector3.right * capsuleCollider.radius);
        Gizmos.DrawLine(start - Vector3.right * capsuleCollider.radius, end - Vector3.right * capsuleCollider.radius);
        Gizmos.matrix = Matrix4x4.identity; // Reset the Gizmos matrix
    }

    private void DrawMeshCollider(MeshCollider meshCollider)
    {
        if (meshCollider.sharedMesh != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawMesh(meshCollider.sharedMesh);
            Gizmos.matrix = Matrix4x4.identity; // Reset the Gizmos matrix
        }
    }
}