using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoInterfaceGui : MonoBehaviour
{
    // Start is called before the first frame update
    private ArduinoInterface _arduinoInterface;
    private bool InterfaceEnabled = false;
    void Start()
    {
        _arduinoInterface = FindObjectOfType<ArduinoInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            InterfaceEnabled = !InterfaceEnabled;
            _arduinoInterface.enabled = InterfaceEnabled;
        }
    }
    
    public void input(string input)
    {
        _arduinoInterface.TryOpenPort(input);
    }
}
