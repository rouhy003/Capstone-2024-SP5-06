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

    // Selects the next weapon
    public void NextWeapon()
    {
        // If the player has more than one weapon
        if (WeaponList.Count > 0)
        {
            int weaponIndex = WeaponList.IndexOf(currentWeapon);

            // If the player is currently weilding the last weapon in the list
            if (weaponIndex >= WeaponList.Count - 1)
            {
                currentWeapon = WeaponList[0];
            }
            else
            {
                currentWeapon = WeaponList[weaponIndex + 1];
            }
        }
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
        else if (Input.GetKeyDown(KeyCode.R))
        {
            NextWeapon();
        }
    }
}
