using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机控制
/// </summary>
public class CameraManager : MonoBehaviour
{
	private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    //放大
    public void ZoomIn()
    {
        mainCamera.DOOrthoSize(14f, 0.5f);
    }

    //缩小
    public void ZoomOut()
    {
        mainCamera.DOOrthoSize(20f, 0.5f);
    }
}
