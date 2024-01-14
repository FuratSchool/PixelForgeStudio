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
    private SettingsData _settingsData;
    private SceneController _sceneController;
    private void Awake()
    {
        _sceneController = FindObjectOfType<SceneController>();
        if (_sceneController != null)
        {
            _camera = GetComponent<CinemachineFreeLook>();
            _camera.m_YAxis.m_MaxSpeed = _defaultSensitivityY;
            _camera.m_XAxis.m_MaxSpeed = _defaultSensitivityX;
            UpdateCameraSettings(FindObjectOfType<SceneController>().Settings);
        }
    }

    public void UpdateCameraSettings(SettingsData settings)
    {
        _settingsData = settings;
        _camera.m_YAxis.m_MaxSpeed = _defaultSensitivityY * (_settingsData.sensitivityY * correctionY);
        _camera.m_XAxis.m_MaxSpeed = _defaultSensitivityX * (_settingsData.sensitivityX * correctionX);
        _camera.m_XAxis.m_InvertInput = _settingsData.invertedX;
        _camera.m_YAxis.m_InvertInput = _settingsData.invertedY;
    }
    
    void ControlChanged(PlayerInput input)
    {
        if(_camera == null) return;
        var device = input.currentControlScheme;
        if(device.Equals("Controller"))
        {
            
            correctionX = ControllerCorrectionX;
            correctionY = ControllerCorrectionY;
            _camera.m_YAxis.m_MaxSpeed = _defaultSensitivityY * (_settingsData.sensitivityY * correctionY);
            _camera.m_XAxis.m_MaxSpeed = _defaultSensitivityX * (_settingsData.sensitivityX * correctionX);
            _camera.m_XAxis.m_InvertInput = _settingsData.invertedX;
            _camera.m_YAxis.m_InvertInput = _settingsData.invertedY;
        }
        else if (device.Equals("KeyboardMouse"))
        {
            correctionX = 1;
            correctionY = 1;
            _camera.m_YAxis.m_MaxSpeed = _defaultSensitivityY * (_settingsData.sensitivityY * correctionY);
            _camera.m_XAxis.m_MaxSpeed = _defaultSensitivityX * (_settingsData.sensitivityX * correctionX);
            _camera.m_XAxis.m_InvertInput = _settingsData.invertedX;
            _camera.m_YAxis.m_InvertInput = _settingsData.invertedY;
        }
    }
    
    public void SetCameraActive(bool active)
    {
        _camera.enabled = active;
    }
}
