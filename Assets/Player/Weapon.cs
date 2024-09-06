using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private GenericWeapon currentWeapon;
    [SerializeField] private List<GenericWeapon> WeaponList = new List<GenericWeapon>();

    void Start()
    {
        // Gets the player's starting weapons and adds them to the array.
        GenericWeapon[] startingWeapons = gameObject.GetComponentsInChildren<GenericWeapon>();
        foreach (GenericWeapon weapon in startingWeapons)
        {
           WeaponList.Add(weapon);
        }

        // Gives the player a weapon if they don't have any to start off with.
        if (WeaponList.Count < 1)
        {
            LegacyWeapon startingWeapon = gameObject.AddComponent<LegacyWeapon>();
            WeaponList.Add(startingWeapon);
        }
        currentWeapon = WeaponList[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            currentWeapon.Shoot();
        }
    }
}
