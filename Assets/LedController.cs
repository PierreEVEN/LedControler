using ArduinoBluetoothAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public enum MessageType
{
    SetMode = 1,
    SetColor = 2,
    SetInt = 3,
}
public class LedController : MonoBehaviour
{
    private BluetoothHelper helper;

    public GameObject SelectDeviceMenu;
    GameObject select_device_menu;
    public GameObject container;

    public GameObject appMenu;

    void Start()
    {
        try
        {
            BluetoothHelper.BLE = false;
           
            helper = BluetoothHelper.GetInstance();
            helper.OnConnected += OnConnected;
            helper.OnConnectionFailed += OnConnectionFailed;
            helper.OnScanEnded += OnScanEnded;
            helper.OnDataReceived += OnDataReceived;

            //helper.setCustomStreamManager(new MyStreamManager()); //implement your own way of delimiting the messages
            helper.setTerminatorBasedStream("\n"); //every messages ends with new line character

        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    Dictionary<string, float> unreceived_messages = new Dictionary<string, float>();

    private void Update()
    {
        /*
        var copy = unreceived_messages.ToDictionary(entry => entry.Key, entry => entry.Value);

        foreach (var entry in copy)
        {
            var fst = unreceived_messages.First();
            if (Time.realtimeSinceStartup - fst.Value > 1.0f)
                unreceived_messages.Remove(fst.Key);
            else
                helper.SendData(fst.Key);
        }*/
    }

    void OnDataReceived(BluetoothHelper helper)
    {
        string received = helper.Read();
        unreceived_messages.Remove(received);

        string sent_text;
        sent_text = (4).ToString() + ";" + 5.ToString() + ";" + 6.ToString();
        helper.SendData(sent_text);
    }

    void OnScanEnded(BluetoothHelper helper, LinkedList<BluetoothDevice> devices)
    {
        select_device_menu.GetComponent<FillDeviceList>().Fill(devices, this);
    }

    void OnConnected(BluetoothHelper helper)
    {
        helper.StartListening();

        container.transform.DetachChildren();
        select_device_menu = Instantiate(appMenu);
        select_device_menu.transform.parent = container.transform;
        select_device_menu.GetComponent<AppMenu>().Init(this);
    }

    public void SendMessage(MessageType message_type, int index, string payload = "")
    {
        string sent_text;
        sent_text = ((int)message_type).ToString() + ";" + index.ToString() + ";" + payload;
        //unreceived_messages.Add(sent_text, Time.realtimeSinceStartup);
        helper.SendData(sent_text);
    }

    void OnConnectionFailed(BluetoothHelper helper)
    {
        Debug.LogError("Connection lost");
    }

    public void ScanForDevices()
    {
        Debug.LogError("Start Scan");
        container.transform.DetachChildren();
        select_device_menu = Instantiate(SelectDeviceMenu);
        select_device_menu.transform.parent = container.transform;
        helper.ScanNearbyDevices();
    }

    public void TryConnectToDevice(BluetoothDevice device)
    {
        helper.setDeviceName(device.DeviceName);
        try
        {
            helper.Connect();
        }
        catch (Exception)
        {
            ScanForDevices();
        }
    }


    void OnDestroy()
    {
        if (helper != null)
            helper.Disconnect();
    }
}
