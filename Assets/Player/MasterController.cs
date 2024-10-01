using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MasterController : MonoBehaviour
{
    public PickUpWeapon puw;
    public bool isWeaponHeld = false;
    public GenericWeapon weaponHeld;
    public float CoolDownTime = 2;
    public bool leftHand = false;

    void Update()
    {
        if ( puw != null)
        {
            if (leftHand)
            {
                if (OVRInput.Get(OVRInput.Button.Four))
                {
                    PutDownWeapon();
                }
                else if (OVRInput.Get(OVRInput.Button.Three))
                {
                    PickUpWeapon();
                }
                else if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0 && leftHand)
                {
                    FireWeapon();
                }
            }
            else
            {
                if (OVRInput.Get(OVRInput.Button.Two))
                {
                    PutDownWeapon();
                }
                else if (OVRInput.Get(OVRInput.Button.One))
                {
                    PickUpWeapon();
                }
                else if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0 && !leftHand)
                {
                    FireWeapon();
                }
            }
        }
    }

    private void PickUpWeapon()
    {
        if (puw.canBePickedUp == true && isWeaponHeld == false)
        {
            puw.controller = gameObject.transform;
            puw.PickUp();
            isWeaponHeld = true;
            weaponHeld = puw.gameObject.GetComponent<GenericWeapon>();
        }
    }

    private void PutDownWeapon()
    {
        if (puw.pickedUp == true && isWeaponHeld == true)
        {
            puw.PutDown();
            isWeaponHeld = false;
            weaponHeld = null;
            puw = null;
        }
    }

    private void FireWeapon()
    {
        if (weaponHeld != null && weaponHeld.GetCanShoot())
        {
            weaponHeld.Shoot();
            StartCoroutine(weaponHeld.WeaponCoolDown());
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<PickUpWeapon>() != null && isWeaponHeld == false && !collider.GetComponent<PickUpWeapon>().pickedUp)
        {
            puw = collider.GetComponent<PickUpWeapon>();
            puw.canBePickedUp = true;
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
