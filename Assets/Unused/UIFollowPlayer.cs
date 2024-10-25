using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    private RectTransform uiTransform;
    public Transform playerHead;

    public float spawnDistance;

    private Vector3 playerHeadForward;

    private void Start()
    {
        uiTransform = GetComponent<RectTransform>();
        playerHeadForward = playerHead.forward;
    }

    void Update()
    {
        uiTransform.position = new Vector3(playerHeadForward.x, 0, playerHeadForward.z).normalized * spawnDistance;
        uiTransform.LookAt(new Vector3(playerHead.position.x, uiTransform.position.y, playerHead.position.z));
        uiTransform.forward *= -1;
    }

}
