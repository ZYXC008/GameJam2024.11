using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("ÊÂ¼þ¼àÌý")]
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO cameraShakeEvent;

    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impluseSource;

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRiased += OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRiased += OnAfterSceneLoadedEvent;
    }
    private void OnDisable()
    {
        cameraShakeEvent.OnEventRiased -= OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRiased -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        GetNewCameraBounds();
    }

    private void OnCameraShakeEvent()
    {
        impluseSource.GenerateImpulse();
    }
    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
        {
            return;
        }
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
    }
}
