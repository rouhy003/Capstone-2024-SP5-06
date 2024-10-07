using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    GameObject startTransform;
    public Transform controller;
    public bool canBePickedUp = false;
    public bool pickedUp = false;

    void Start()
    {
        startTransform = new GameObject();
        startTransform.transform.position = transform.position;
        startTransform.transform.rotation = transform.rotation;
    }

    //Sets the position and rotation of the weapon to either its starting transform or the transform of the controller that picked it up.
    void Update()
    {
        if (pickedUp == true)
        {
            transform.position = controller.transform.position;
            transform.rotation = controller.transform.rotation;
        }
        else
        {
            transform.position = startTransform.transform.position;
            transform.rotation = startTransform.transform.rotation;
        }
    }

    //Sets pickedUp to true and makes the boxCollider a trigger, to avoid collisions while the weapon is held.
    public void PickUp()
    {
        pickedUp = true;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    //Sets pickedUp to false and resets the boxCollider.
    public void PutDown()
    {
        pickedUp = false;
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
    }
}
