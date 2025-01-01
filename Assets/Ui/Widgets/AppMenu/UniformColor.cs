using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UniformColor : AppBase
{
    // Start is called before the first frame update
    void Start()
    {
        update_mode();
        update_color();
        update_int();
    }

    public GameObject mode_slider;
    public GameObject int_slider;
    public GameObject color_picker;

    public void update_mode()
    {
        int value = (int)mode_slider.GetComponent<Slider>().value;
        GetParent().SendMessage(MessageType.SetMode, value);
    }
    public void update_int()
    {
        int value = (int)int_slider.GetComponent<Slider>().value;
        GetParent().SendMessage(MessageType.SetInt, 0, value.ToString());
    }
    public void update_color()
    {
        Color value = color_picker.GetComponent<FlexibleColorPicker>().GetColor();
        GetParent().SendMessage(MessageType.SetColor, 0, ((int)(value.r * 255)).ToString() + "," + ((int)(value.g * 255)).ToString() + "," + ((int)(value.b * 255)).ToString());
    }
}
