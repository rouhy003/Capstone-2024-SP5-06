using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class RoomSpawner : AnchorPrefabSpawner
{ 
    public void InitialiseRoom()
    {
        SpawnPrefabs(MRUK.Instance.GetCurrentRoom());
    }
}
