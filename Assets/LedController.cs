using ArduinoBluetoothAPI;
using System;
using System.Collections.Generic;
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

    void OnDataReceived(BluetoothHelper helper)
    {
        helper.Read();
    }

    void OnScanEnded(BluetoothHelper helper, LinkedList<BluetoothDevice> devices)
    {
        Debug.LogError("ScanEnded");
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
