using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialKeybindings : MonoBehaviour
{
    [SerializeField] private InputActionReference m_Action;
    [SerializeField] private string KeyboardBindingId;
    [SerializeField] private string ControllerBindingId;
    [SerializeField] private string tutorialText;
    
    [SerializeField] private bool SecondAction = false;
    [SerializeField] private InputActionReference m_Action2;
    [SerializeField] private string KeyboardBindingId2;
    [SerializeField] private string ControllerBindingId2;
    [SerializeField] private string tutorialText2;
    
    
    
    private string keybind;
    private string keybind2;
    public string TutorialText;

    void OnEnable()
    {
        ChangeText(FindObjectOfType<PlayerInput>());
    }
    void ControlChanged(PlayerInput input)
    {
        ChangeText(input);
    }

    void ChangeText(PlayerInput input)
    {
        
        var deviceLayoutName = default(string);
        var controlPath = default(string);

        var device = input.currentControlScheme;
        if(device.Equals("Controller"))
        {
            if (m_Action.name == "Movement/Move")
            {
                keybind = "Left Stick";
            }
            else
            {
                var bindingIndex = m_Action.action.bindings.IndexOf(x => x.id.ToString() == ControllerBindingId);
                if (bindingIndex != -1)
                    keybind = m_Action.action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath);
            }
            
        }
        else if (device.Equals("KeyboardMouse")){
            if (m_Action.name == "Movement/Move")
            {
                keybind = "WASD";
            }
            else
            {
                var bindingIndex = m_Action.action.bindings.IndexOf(x => x.id.ToString() == KeyboardBindingId);
                if (bindingIndex != -1)
                    keybind = m_Action.action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath);
            }
        }
        var output = string.Format(tutorialText, keybind);

        if (SecondAction)
        {
            if(device.Equals("Controller"))
            {
                Debug.Log(m_Action2.name);
                if (m_Action2.name == "Movement/Move")
                {
                    keybind2 = "Left Stick";
                }
                else
                {
                    var bindingIndex = m_Action2.action.bindings.IndexOf(x => x.id.ToString() == ControllerBindingId2);
                    if (bindingIndex != -1)
                        keybind2 = m_Action2.action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath);
                }
            
            }
            else if (device.Equals("KeyboardMouse")){
                if (m_Action2.name == "Movement/Move")
                {
                    keybind2 = "WASD";
                }
                else
                {
                    var bindingIndex = m_Action2.action.bindings.IndexOf(x => x.id.ToString() == KeyboardBindingId2);
                    if (bindingIndex != -1)
                        keybind2 = m_Action2.action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath);
                }
            }
            
        }
        var output2 = string.Format(tutorialText2, keybind2);
        TutorialText = output + " " + output2;
    }
}
