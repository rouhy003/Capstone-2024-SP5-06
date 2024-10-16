using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Meta.XR.MRUtilityKit;

public class MasterController : MonoBehaviour
{
    public PickUpWeapon puw;
    public GenericWeapon weaponHeld;

    public bool isWeaponHeld = false;
    public bool leftHand = false;

    public Transform handAnchor;
    public GameObject ovrRig;
    
    public float CoolDownTime = 2;

    public bool isSpaceSyncing;

    bool isJoined = false;
    
    StartMenu sm;
    GameObject mr;

    private void Start()
    {
        sm = FindObjectOfType<StartMenu>();
    }

    void Update()
    {
        if (mr == null)
        {
            try
            {
                mr = FindObjectOfType<MRUKRoom>().gameObject;
            }
            catch{}
        }

        SyncSpace();

        //Updates the controller position to match where it actually is
        gameObject.transform.position = handAnchor.transform.position;
        gameObject.transform.rotation = handAnchor.transform.rotation;

        //Handles input for joining a game.
        if (!isJoined && !isSpaceSyncing)
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
            HandleInput();
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

    //Repositions the VR rig and Room Scan depending on the thumbstick inputs
    private void SyncSpace()
    {
        if (isSpaceSyncing)
        {
            //Right thumbstick changes the X and Z position of the VR rig
            ovrRig.transform.position -= new Vector3(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x / 100, ovrRig.transform.position.y, OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y / 100);

            //Left thumbstick changes the Y rotation of the VR rig
            Quaternion r = ovrRig.transform.rotation;
            ovrRig.transform.rotation = new Quaternion(r.x, r.y + OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y / 50, r.z, r.w);

            if (mr != null)
            {
                //Right thumbstick changes the X and Z position of the room scan as well
                mr.transform.position -= new Vector3(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x / 100, mr.transform.position.y, OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y / 100);

                //Left thumbstick changes the Y rotation of the room scan as well
                Quaternion rm = mr.transform.rotation;
                mr.transform.rotation = new Quaternion(rm.x, rm.y + OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y / 50, rm.z, rm.w);
            }

            if (OVRInput.Get(OVRInput.Button.Three) || OVRInput.Get(OVRInput.Button.One))
            {
                isSpaceSyncing = false;
            }
        }
    }

    private void HandleInput()
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
