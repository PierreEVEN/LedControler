using ArduinoBluetoothAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillDeviceList : MonoBehaviour
{
    public GameObject device_list;
    public GameObject SelectDeviceButton;

    public void Fill(LinkedList<BluetoothDevice> devices, LedController controller)
    {
        device_list.transform.DetachChildren();
        foreach (var device in devices)
        {
            GameObject select_device_menu = Instantiate(SelectDeviceButton);
            select_device_menu.transform.parent = device_list.transform;
            select_device_menu.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = device.DeviceName;
            select_device_menu.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => {
                controller.TryConnectToDevice(device);
            });
        }
    }
}
