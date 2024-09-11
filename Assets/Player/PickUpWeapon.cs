using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    GameObject startTransform;
    Transform controller;
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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<MasterController>() != null && collider.GetComponent<MasterController>().weaponHeld == false)
        {
            canBePickedUp = true;
            controller = collider.gameObject.transform;
            collider.GetComponent<MasterController>().puw = this;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        canBePickedUp = false;
    }

    public void PickUp()
    {
        pickedUp = true;
    }
}
