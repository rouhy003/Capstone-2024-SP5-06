using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PropState : NetworkBehaviour
{
    [SerializeField] private GameObject originalMesh;
    [SerializeField] private GameObject damagedMesh;

    // Changes the mesh to use the damaged version.
    // If no damaged version exists, the object simply disappears.
    public void UseDamageMesh()
    {
        originalMesh.SetActive(false);
        if (damagedMesh != null) damagedMesh.SetActive(true);
    }
}
