using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CinemachineFreeLook _camera;
    [SerializeField] private float _defaultSensitivityX;
    [SerializeField] private float _defaultSensitivityY;
    [SerializeField] private float ControllerCorrectionX =5;
    [SerializeField] private float ControllerCorrectionY =5;
    private float correctionX = 1;
    private float correctionY = 1;
    private void Awake()
    {
        _camera = GetComponent<CinemachineFreeLook>();
        _defaultSensitivityY = _camera.m_YAxis.m_MaxSpeed;
        _defaultSensitivityX = _camera.m_XAxis.m_MaxSpeed;
         UpdateCameraSettings(FindObjectOfType<SceneController>().Settings);
    }

    public void UpdateCameraSettings(SettingsData settings)
    {
        _camera.m_YAxis.m_MaxSpeed = _defaultSensitivityY * (settings.sensitivityY * correctionY);
        _camera.m_XAxis.m_MaxSpeed = _defaultSensitivityX * (settings.sensitivityX * correctionX);
        _camera.m_XAxis.m_InvertInput = settings.invertedX;
        _camera.m_YAxis.m_InvertInput = settings.invertedY;
    }
    
    void ControlChanged(PlayerInput input)
    {
        var device = input.currentControlScheme;
        if(device.Equals("Controller"))
        {
            correctionX = ControllerCorrectionX;
            correctionY = ControllerCorrectionY;
        }
        else if (device.Equals("KeyboardMouse"))
        {
            correctionX = 1;
            correctionY = 1;
        }
    }
}
