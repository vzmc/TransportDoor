using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    [SerializeField]
    private Transform door1;
    [SerializeField]
    private Transform door2;
    [SerializeField]
    private Transform selfCamera;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform playerCamera;
    [SerializeField]
    private Material doorMaterial;
    
    private bool isChanged;

    private void Start()
    {
        InitCamera();
    }

    private void InitCamera()
    {
        var sCamera = selfCamera.GetComponent<Camera>();
        var pCamera = playerCamera.GetComponent<Camera>();
        sCamera.farClipPlane = pCamera.farClipPlane;
        sCamera.nearClipPlane = pCamera.nearClipPlane;
        sCamera.fieldOfView = pCamera.fieldOfView;
        CreateRenderTexture(sCamera, doorMaterial);
    }

    /// <summary>
    /// Screenの解像度に合わせるため、RenderTextureを動的に生成する
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="material"></param>
    private void CreateRenderTexture(Camera camera, Material material)
    {
        if (camera.targetTexture != null)
        {
            camera.targetTexture.Release();
        }
        var renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        material.SetTexture("_MainTex", renderTexture);
        camera.targetTexture = renderTexture;
    }

    private void Update()
    {
        var seeDoor = isChanged ? door1 : door2;
        var otherDoor = isChanged ? door2 : door1;
        UpdatePosition(seeDoor, otherDoor);
        UpdateRotation(seeDoor, otherDoor);
    }

    /// <summary>
    /// カメラの位置更新
    /// </summary>
    /// <param name="seeDoor"></param>
    /// <param name="otherDoor"></param>
    private void UpdatePosition(Transform seeDoor, Transform otherDoor)
    {
        transform.position = seeDoor.TransformPoint(otherDoor.InverseTransformPoint(player.position));
        selfCamera.localPosition = playerCamera.localPosition;
    }

    /// <summary>
    /// カメラの回転行進
    /// </summary>
    /// <param name="seeDoor"></param>
    /// <param name="otherDoor"></param>
    private void UpdateRotation(Transform seeDoor, Transform otherDoor)
    {
        var deltaAngles = player.eulerAngles - otherDoor.eulerAngles;
        transform.rotation = seeDoor.rotation;
        transform.Rotate(deltaAngles);

        selfCamera.localRotation = playerCamera.localRotation;
    }

    /// <summary>
    /// 見るドアを変更フラグ
    /// </summary>
    public void ChangeDoor()
    {
        isChanged = !isChanged;
    }
}
