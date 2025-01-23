using UnityEngine;

public class Management : MonoBehaviour
{
    public Vector3 lastWorldPosition;
    
    private void Awake() 
    {
        DontDestroyOnLoad(this);
    }
}
