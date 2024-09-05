using Fusion;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Meta.WitAi;

public class Weapon : NetworkBehaviour
{
    public UnityEvent OnWeaponShoot;
    private UnityAction shoot;
    public List<GenericWeapon> currentWeapons = new List<GenericWeapon>();

    private void Start()
    {
        if (currentWeapons.Count < 1)
        {
            AddWeapon(new LegacyWeapon());
        }
        OnWeaponShoot.AddListener()
    }

    // Adds the specified weapon to the player
    public bool AddWeapon(GenericWeapon weapon)
    {
        if (!currentWeapons.Contains(weapon))
        {
            currentWeapons.Add(weapon);
            return true;
        }
        return false;
    }

    // Removes the specified weapon from the player
    public bool RemoveWeapon(GenericWeapon weapon) {
        if (!currentWeapons.Contains(weapon))
        {
            currentWeapons.Remove(weapon);
            return true;
        }
        return false;
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
            OnWeaponShoot?.Invoke();
        }
    }
}
