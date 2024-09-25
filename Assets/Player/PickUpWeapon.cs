using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    GameObject startTransform;
    public Transform controller;
    public bool canBePickedUp = false;
    public bool pickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        startTransform = new GameObject();
        startTransform.transform.position = transform.position;
        startTransform.transform.rotation = transform.rotation;
    }

    // Update is called once per frame
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

    public void PickUp()
    {
        pickedUp = true;
    }
}
