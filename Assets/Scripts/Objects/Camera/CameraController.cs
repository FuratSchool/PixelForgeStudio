using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineFreeLook _camera;
    [SerializeField] private float _defaultSensitivityX;
    [SerializeField] private float _defaultSensitivityY;
    private void Awake()
    {
        _camera = GetComponent<CinemachineFreeLook>();
        _defaultSensitivityY = _camera.m_YAxis.m_MaxSpeed;
        _defaultSensitivityX = _camera.m_XAxis.m_MaxSpeed;
         UpdateCameraSettings(FindObjectOfType<SceneController>().Settings);
    }

    public void UpdateCameraSettings(SettingsData settings)
    {
        _camera.m_YAxis.m_MaxSpeed = _defaultSensitivityY * (settings.sensitivityY);
        _camera.m_XAxis.m_MaxSpeed = _defaultSensitivityX * (settings.sensitivityX);
        _camera.m_XAxis.m_InvertInput = settings.invertedX;
        _camera.m_YAxis.m_InvertInput = settings.invertedY;
    }
}
