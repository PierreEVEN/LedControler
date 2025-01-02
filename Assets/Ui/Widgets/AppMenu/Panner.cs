using UnityEngine;
using UnityEngine.UI;

public class Panner : AppBase
{
    // Start is called before the first frame update
    void Start()
    {
        update_spacing();
        update_colorA();
        update_colorB();
        update_speed();
    }

    public GameObject spacing_slider;
    public GameObject speed_slider;
    public GameObject color_pickerA;
    public GameObject color_pickerB;

    public void update_spacing()
    {
        update_int(1, (int)spacing_slider.GetComponent<Slider>().value);
    }
    public void update_speed()
    {
        update_int(0, (int)speed_slider.GetComponent<Slider>().value);
    }
    public void update_colorA()
    {
        update_color(0, color_pickerA.GetComponent<FlexibleColorPicker>().GetColor());
    }
    public void update_colorB()
    {
        update_color(1, color_pickerB.GetComponent<FlexibleColorPicker>().GetColor());
    }
}
