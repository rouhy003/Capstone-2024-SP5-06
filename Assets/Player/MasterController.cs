using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour
{
    public PickUpWeapon puw;
    public bool weaponHeld = false;

    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            if (puw.pickedUp == true && puw != null && weaponHeld == true)
            {
                puw.pickedUp = false;
                weaponHeld = false;
            }
        }
        else if (OVRInput.Get(OVRInput.Button.One))
        {
            if (puw.canBePickedUp == true && puw != null && weaponHeld == false)
            {
                puw.PickUp();
                weaponHeld = true;
            }
        }
    }
}
