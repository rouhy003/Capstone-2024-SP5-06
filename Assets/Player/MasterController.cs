using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour
{
    public PickUpWeapon puw;
    public bool weaponHeld = false;

    void Update()
    {
        if ( puw != null)
        {
            if (OVRInput.Get(OVRInput.Button.Two))
            {
                if (puw.pickedUp == true && weaponHeld == true)
                {
                    puw.pickedUp = false;
                    weaponHeld = false;
                }
            }
            else if (OVRInput.Get(OVRInput.Button.One))
            {
                if (puw.canBePickedUp == true && weaponHeld == false)
                {
                    puw.PickUp();
                    weaponHeld = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<PickUpWeapon>() != null && weaponHeld == false)
        {
            puw = collider.GetComponent<PickUpWeapon>();
            puw.canBePickedUp = true;
            puw.controller = this.gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (puw != null)
        {
            puw.canBePickedUp = false;
        }
    }
}
