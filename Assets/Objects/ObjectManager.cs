using Fusion;
using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class ObjectManager : NetworkBehaviour
{
    [SerializeField] private GameObject objectPrefab;

    [SerializeField] private List<NetworkObject> objectPool = new List<NetworkObject>();
    [Networked] private int CurrentObjects { get; set; }
    [SerializeField] private const int ObjectLimit = 3;

    public override void FixedUpdateNetwork()
    {
        // TODO: Insert spawning functionality here.
    }
}
