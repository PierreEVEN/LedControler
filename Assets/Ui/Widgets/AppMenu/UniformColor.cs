using UnityEngine;

public class UniformColor : AppBase
{
    // Start is called before the first frame update
    void Start()
    {
        update_color();
    }

    public GameObject color_picker;

    public void update_color()
    {
        update_color(0, color_picker.GetComponent<FlexibleColorPicker>().GetColor());
    }
}
