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
    private bool isScanning;
    private bool isConnecting;

    public GameObject SelectDeviceMenu;
    GameObject select_device_menu;
    public GameObject container;

    public GameObject temp_app;

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
        this.isScanning = false;
        Debug.LogError("ScanEnded");
        select_device_menu.GetComponent<FillDeviceList>().Fill(devices, this);
    }

    void OnConnected(BluetoothHelper helper)
    {
        isConnecting = false;
        helper.StartListening();

        container.transform.DetachChildren();
        select_device_menu = Instantiate(temp_app);
        select_device_menu.transform.parent = container.transform;
        select_device_menu.GetComponent<TempApp>().Init(this);
    }

    public void SendMessage(MessageType message_type, int index, string payload = "")
    {
        string sent_text;
        sent_text = ((int)message_type).ToString() + ";" + index.ToString() + ";" + payload;
        helper.SendData(sent_text);
    }

    void OnConnectionFailed(BluetoothHelper helper)
    {
        isConnecting = false;
        Debug.LogError("Connection lost");
    }

    public void ScanForDevices()
    {
        Debug.LogError("Start Scan");
        container.transform.DetachChildren();
        select_device_menu = Instantiate(SelectDeviceMenu);
        select_device_menu.transform.parent = container.transform;
        isScanning = helper.ScanNearbyDevices();
    }

    public void TryConnectToDevice(BluetoothDevice device)
    {
        helper.setDeviceName(device.DeviceName);
        try
        {
            helper.Connect();
            isConnecting = true;
        }
        catch (Exception)
        {
            isConnecting = false;
        }
    }


    void OnDestroy()
    {
        if (helper != null)
            helper.Disconnect();
    }
}
