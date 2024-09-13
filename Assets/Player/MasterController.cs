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

    void Update()
    {
        if ( puw != null)
        {
            if (OVRInput.Get(OVRInput.Button.Two))
            {
                if (puw.pickedUp == true && isWeaponHeld == true)
                {
                    puw.pickedUp = false;
                    isWeaponHeld = false;
                    weaponHeld = null;
                    puw = null;
                }
            }
            else if (OVRInput.Get(OVRInput.Button.One))
            {
                if (puw.canBePickedUp == true && isWeaponHeld == false)
                {
                    puw.PickUp();
                    isWeaponHeld = true;
                    weaponHeld = puw.gameObject.GetComponent<GenericWeapon>();
                }
            }
            else if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0 && canShoot)
            {
                if (weaponHeld != null)
                {
                    weaponHeld.Shoot();
                    canShoot = false;
                    StartCoroutine(WeaponCoolDown());
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<PickUpWeapon>() != null && isWeaponHeld == false)
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
