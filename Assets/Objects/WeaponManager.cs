using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    public GameObject[] weaponPrefabs;

    // Spawning boundaries for each weapon
    private Vector3 spawnBoundaries = new Vector3(0.5f, 0.2f, 0.5f);

    private List<NetworkObject> currentWeapons = new List<NetworkObject>();

    public const int WeaponLimit = 3;
}
