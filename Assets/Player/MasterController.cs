using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MasterController : MonoBehaviour
{
    public PickUpWeapon puw;
    public GenericWeapon weaponHeld;

    public bool isWeaponHeld = false;
    public bool leftHand = false;
    
    public float CoolDownTime = 2;
    
    bool isJoined = false;
    
    StartMenu sm;    

    private void Start()
    {
        sm = FindObjectOfType<StartMenu>();
    }

    void Update()
    {
        //Handles input for joining a game.
        if (!isJoined)
        {
            if (OVRInput.Get(OVRInput.Button.Three) || OVRInput.Get(OVRInput.Button.One))
            {
                isJoined = true;
                sm.StartSharedVR();
            }
        }

        //Handles controller input if a weapon is overlapped.
        if ( puw != null)
        {
            //Left controller
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
            //Right controller
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

    //Sets the necessary values in the PickUpWeapon script, so that the weapon follows the controller.
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

    //Resets the values in the PickUpWeapon script, so it stops following controller.
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

    //Calls the Shoot method in the weapon script and starts the colldown coroutine.
    private void FireWeapon()
    {
        if (weaponHeld != null && weaponHeld.GetCanShoot())
        {
            weaponHeld.Shoot();
            StartCoroutine(weaponHeld.WeaponCoolDown());
        }
    }

    //Sets the PickUpWeapon script if collider overlaps an object with one.
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<PickUpWeapon>() != null && isWeaponHeld == false && !collider.GetComponent<PickUpWeapon>().pickedUp)
        {
            puw = collider.GetComponent<PickUpWeapon>();
            puw.canBePickedUp = true;
        }
    }

    //Sets the PickUpWeapon script to null if collider leaves an object.
    private void OnTriggerExit(Collider collider)
    {
        if (puw != null)
        {
            puw.canBePickedUp = false;
        }
    }

}
