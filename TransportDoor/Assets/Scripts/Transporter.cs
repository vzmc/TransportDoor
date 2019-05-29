using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Transporter : MonoBehaviour
{
    [SerializeField]
    private Transform shadow;
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private FirstPersonController firstPersonController;

    private bool requestChange;

    private void LateUpdate()
    {
        if(requestChange)
        {
            Transport();
        }
    }
    
    /// <summary>
    /// 転送
    /// </summary>
    private void Transport()
    {
        // characterControllerとfirstPersonControllerが転送を邪魔するので、
        // 転送前に一旦閉じる必要がある
        characterController.enabled = false;
        firstPersonController.enabled = false;

        var tempPos = shadow.position;
        var tempRot = shadow.rotation;

        shadow.position = transform.position;
        shadow.rotation = transform.rotation;

        transform.position = tempPos;
        transform.rotation = tempRot;

        shadow.GetComponent<ShadowController>().ChangeDoor();

        characterController.enabled = true;
        firstPersonController.enabled = true;

        requestChange = false;
    }

    public void RequestTransport()
    {
        requestChange = true;
    }
}
