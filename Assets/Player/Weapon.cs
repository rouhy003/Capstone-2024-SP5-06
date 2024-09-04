using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : NetworkBehaviour
{
    public UnityEvent OnWeaponShoot;

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
