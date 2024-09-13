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
    bool canShoot = true;
    bool coroutineRunning = false;
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
                else if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0 && canShoot && leftHand)
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
                else if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0 && canShoot && !leftHand)
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
            puw.controller = this.gameObject.transform;
            puw.PickUp();
            isWeaponHeld = true;
            weaponHeld = puw.gameObject.GetComponent<GenericWeapon>();
        }
    }

    private void PutDownWeapon()
    {
        if (puw.pickedUp == true && isWeaponHeld == true)
        {
            puw.pickedUp = false;
            isWeaponHeld = false;
            weaponHeld = null;
            puw = null;
        }
    }

    private void FireWeapon()
    {
        if (weaponHeld != null)
        {
            weaponHeld.Shoot();
            canShoot = false;
            StartCoroutine(WeaponCoolDown());
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

    public IEnumerator WeaponCoolDown()
    {
        if (!coroutineRunning)
        {
            coroutineRunning = true;
            yield return new WaitForSeconds(CoolDownTime);
            canShoot = true;
            coroutineRunning = false;
        }
    }
}
