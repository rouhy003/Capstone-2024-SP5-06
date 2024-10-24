using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSyncSaving : MonoBehaviour
{
    [SerializeField] protected GameObject VR_rig;

    void Start()
    {
        LoadData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("XOffset", VR_rig.transform.position.x);
        PlayerPrefs.SetFloat("YOffset", VR_rig.transform.position.y);
        PlayerPrefs.SetFloat("ZOffset", VR_rig.transform.position.z);
        PlayerPrefs.SetFloat("XRotation", VR_rig.transform.rotation.x);
        PlayerPrefs.SetFloat("YRotation", VR_rig.transform.rotation.y);
        PlayerPrefs.SetFloat("ZRotation", VR_rig.transform.rotation.z);
        PlayerPrefs.SetFloat("WRotation", VR_rig.transform.rotation.w);
    }

    public void LoadData()
    {
        Vector3 position = new Vector3(PlayerPrefs.GetFloat("XOffset"), PlayerPrefs.GetFloat("YOffset"), PlayerPrefs.GetFloat("ZOffset"));
        Quaternion rotation = new Quaternion(PlayerPrefs.GetFloat("XRotation"), PlayerPrefs.GetFloat("YRotation"), PlayerPrefs.GetFloat("ZRotation"), PlayerPrefs.GetFloat("WRotation"));
        
        VR_rig.transform.position = position;
        VR_rig.transform.rotation = rotation;
    }
}
