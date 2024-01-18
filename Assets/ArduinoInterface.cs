using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO.Ports;
using UnityEngine;

public class ArduinoInterface : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool enableReadThread = false;
    
    private SerialPort data_stream = new SerialPort();
    [SerializeField] private string receivedString;
    
    
    private Thread readThread;
    private bool isSendData = false;
    private string message = "Hello";
    private char[] messageChar = new char[1];
    private int baud = 115200;
    public string com = "COM6";
    private void Start()
    {
        TryOpenPort(com);
    }

    public void TryOpenPort(string COM)
    {
        Debug.Log(com);
        if(data_stream.IsOpen) return;
        try
        {
            data_stream.PortName = COM;
            data_stream.BaudRate = baud;
            data_stream.Parity = Parity.None;
            data_stream.StopBits = StopBits.One;
            data_stream.DataBits = 8;
            data_stream.Handshake = Handshake.None;
            data_stream.Open();
            Debug.Log("Serial port opened on COM7");
            isSendData = true;
            //// Setup the thread to poll the port on
            //Thread readThread = new Thread(new ThreadStart(sendData));
            
            //readThread.Start();
 
        }
        catch (Exception e)
        {
            isSendData = false;
            Debug.Log("Couldn't open serial port: " + e.Message);
        }
    }
    void OnDisable()
    {
        data_stream.Close();
        isSendData = false;
    }

    public void EnableLED()
    {
        if(!isSendData) return;
        messageChar[0] = 'S';
        data_stream.Write(messageChar, 0, 1);
        data_stream.BaseStream.Flush();
    }
    
    public void DisableLED()
    {
        if(!isSendData) return;
        messageChar[0] = 'E';
        data_stream.Write(messageChar, 0, 1);
        data_stream.BaseStream.Flush();
    }
    
    public void ColorToBlue()
    {
        if(!isSendData) return;
        messageChar[0] = 'B';
        data_stream.Write(messageChar, 0, 1);
        data_stream.BaseStream.Flush();
    }
    
    public void ColorToPurple()
    {
        if(!isSendData) return;
        messageChar[0] = 'P';
        data_stream.Write(messageChar, 0, 1);
        data_stream.BaseStream.Flush();
    }
}
