using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{ 
    [SerializeField] IThrowable throwableInHand;
    [SerializeField] Transform throwableInHandTransform;
    [SerializeField] List<IThrowable> throwablesToPickup = new List<IThrowable>();

    [SerializeField] float throwForce;
    // Update is called once per frame
    void Update()
    {

        //Shoot();

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(throwableInHand != null)
            {
                Unequip();
            }
            else
            {
                if (throwablesToPickup.Count > 0)
                {
                    throwableInHandTransform = GetInterfaceTransform();
                    throwableInHandTransform.gameObject.layer = 8;
                    throwableInHandTransform.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    throwableInHandTransform.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    Vector3 originalScale = throwableInHandTransform.localScale;
                    throwableInHandTransform.SetParent(gameObject.transform);
                    throwableInHandTransform.localScale = originalScale;
                    throwableInHandTransform.position = gameObject.transform.position;  
                    throwableInHandTransform.transform.position += gameObject.transform.forward * 0.5f;
                }
            }
        }
    }

    public void Equip()
    {
        if(throwableInHand != null)
        {
            Unequip();
        }
        else
        {
            if (throwablesToPickup.Count > 0)
            {
                throwableInHandTransform = GetInterfaceTransform();
                throwableInHandTransform.gameObject.layer = 8;
                throwableInHandTransform.gameObject.GetComponent<Rigidbody>().useGravity = false;
                throwableInHandTransform.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                Vector3 originalScale = throwableInHandTransform.localScale;
                throwableInHandTransform.SetParent(gameObject.transform);
                throwableInHandTransform.localScale = originalScale;
                throwableInHandTransform.position = gameObject.transform.position;  
                throwableInHandTransform.transform.position += gameObject.transform.forward * 0.5f;
            }
        }
    }

    public void Shoot()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
            if (throwableInHandTransform != null)
            {
                Unequip();
                throwableInHandTransform.gameObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * throwForce);
                throwableInHandTransform.gameObject.layer = 7;
            }
        //}
    }

    void Unequip()
    {
        throwableInHand = null;
        if (throwableInHandTransform != null) 
        {
            throwableInHandTransform.SetParent(null);
            throwableInHandTransform.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("collision with trigger");
        if (collision.TryGetComponent(out IThrowable _throwable))
        {   
            throwablesToPickup.Add(_throwable);
            Debug.Log("to pickup: " + throwablesToPickup[0]);
        }   
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out IThrowable _throwable) && throwablesToPickup.Contains(_throwable))
        {   
            throwablesToPickup.Remove(_throwable);
        }   
    }

    Transform GetInterfaceTransform()
    {
        throwableInHand = throwablesToPickup[0];
        MonoBehaviour componentAsMonoBehaviour = throwableInHand as MonoBehaviour;
        if (componentAsMonoBehaviour != null)
        {
            return componentAsMonoBehaviour.transform;
        }
        else return null;
    }
}
