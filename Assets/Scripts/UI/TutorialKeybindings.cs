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
    private string keybind;
    void ControlChanged(PlayerInput input)
    {
        var deviceLayoutName = default(string);
        var controlPath = default(string);

        var device = input.currentControlScheme;
        if(device.Equals("Controller"))
        {
            var bindingIndex = m_Action.action.bindings.IndexOf(x => x.id.ToString() == ControllerBindingId);
            if (bindingIndex != -1)
                keybind = m_Action.action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath);
        }
        else if (device.Equals("KeyboardMouse")){
            var bindingIndex = m_Action.action.bindings.IndexOf(x => x.id.ToString() == KeyboardBindingId);
            if (bindingIndex != -1)
                keybind = m_Action.action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath);
        }
        GetComponent<TMP_Text>().text = string.Format(tutorialText, keybind);
    }
}
