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
    
    private float _sensitivityX;
    private float _sensitivityY;
    
    private float correctionX = 1;
    private float correctionY = 1;
    private SceneController _sceneController;
    private void Awake()
    {
        _sceneController = FindObjectOfType<SceneController>();
        _camera = GetComponent<CinemachineFreeLook>();
        _sensitivityX = _camera.m_XAxis.m_MaxSpeed;
        _sensitivityY = _camera.m_YAxis.m_MaxSpeed;
        if (_sceneController != null)
        {
            _camera.m_YAxis.m_MaxSpeed = _defaultSensitivityY;
            _camera.m_XAxis.m_MaxSpeed = _defaultSensitivityX;
            UpdateCameraSettings(FindObjectOfType<SceneController>().Settings);
        }
    }

    public void UpdateCameraSettings(SettingsData settings)
    {
        int sent;
        switch (_sceneController.Settings.sensitivity)
        {
            case 0:
                sent = 1;
                break;
            case 1:
                sent = 5;
                break;
            case 2:
                sent = 7;
                break;
            case 3:
                sent = 10;
                break;
            default:
                sent = 5;
                break;
        }
        _camera.m_YAxis.m_MaxSpeed = _defaultSensitivityY * (sent * correctionY);
        _camera.m_XAxis.m_MaxSpeed = _defaultSensitivityX * (sent * correctionX);
        _camera.m_XAxis.m_InvertInput = _sceneController.Settings.invertedX;
        _camera.m_YAxis.m_InvertInput = _sceneController.Settings.invertedY;
    }
    
    void ControlChanged(PlayerInput input)
    {
        if(_camera == null) return;
        var device = input.currentControlScheme;
        if (_sceneController == null)
        {
            if (device.Equals("Controller"))
            {
                correctionX = ControllerCorrectionX;
                correctionY = ControllerCorrectionY;
                _camera.m_YAxis.m_MaxSpeed = _sensitivityY * correctionY;
                _camera.m_XAxis.m_MaxSpeed = _sensitivityX * correctionX;
            }
            else if (device.Equals("KeyboardMouse"))
            {
                correctionX = 1;
                correctionY = 1;
                _camera.m_YAxis.m_MaxSpeed = _sensitivityY * correctionY;
                _camera.m_XAxis.m_MaxSpeed = _sensitivityX * correctionX;
            }
        }
        else
        {
            int sent;
            switch (_sceneController.Settings.sensitivity)
            {
                case 0:
                    sent = 1;
                    break;
                case 1:
                    sent = 5;
                    break;
                case 2:
                    sent = 7;
                    break;
                case 3:
                    sent = 10;
                    break;
                default:
                    sent = 5;
                    break;
            }
            if (device.Equals("Controller"))
            {

                correctionX = ControllerCorrectionX;
                correctionY = ControllerCorrectionY;
                _camera.m_YAxis.m_MaxSpeed = _defaultSensitivityY * (sent * correctionY);
                _camera.m_XAxis.m_MaxSpeed = _defaultSensitivityX * (sent * correctionX);
                _camera.m_XAxis.m_InvertInput = _sceneController.Settings.invertedX;
                _camera.m_YAxis.m_InvertInput = _sceneController.Settings.invertedY;
            }
            else if (device.Equals("KeyboardMouse"))
            {
                correctionX = 1;
                correctionY = 1;
                _camera.m_YAxis.m_MaxSpeed = _defaultSensitivityY * (sent * correctionY);
                _camera.m_XAxis.m_MaxSpeed = _defaultSensitivityX * (sent * correctionX);
                _camera.m_XAxis.m_InvertInput = _sceneController.Settings.invertedX;
                _camera.m_YAxis.m_InvertInput = _sceneController.Settings.invertedY;
            }
        }
    }
    
    public void SetCameraActive(bool active)
    {
        _camera.enabled = active;
    }
}
