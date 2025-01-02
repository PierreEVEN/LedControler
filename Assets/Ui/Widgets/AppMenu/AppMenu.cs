using UnityEngine;

public class AppMenu : MonoBehaviour
{
    LedController parent;

    public GameObject dropDown;
    public GameObject container;

    public GameObject widgetUniformColor;
    public GameObject widgetPanner;

    public void Init(LedController in_parent)
    {
        parent = in_parent;
        UpdateMode();
    }

    public void UpdateMode()
    {
        container.transform.DetachChildren();
        GameObject mode = null;
        int new_mode = dropDown.GetComponent<TMPro.TMP_Dropdown>().value;
        switch (new_mode)
        {
            case 0:
                mode = Instantiate(widgetUniformColor);
                break;
            case 1:
                mode = Instantiate(widgetPanner);
                break;
        }
        mode.transform.parent = container.transform;
        foreach (var comp in mode.GetComponents<AppBase>())
            comp.Init(parent);
        parent.SendMessage(MessageType.SetMode, new_mode);
    }
}

public class AppBase : MonoBehaviour
{
    LedController parent;
    public void Init(LedController in_parent)
    {
        parent = in_parent;
    }

    public LedController GetParent() { return parent; }

    public void update_int(int index, int value)
    {
        GetParent().SendMessage(MessageType.SetInt, index, index.ToString());
    }
    public void update_color(int index, Color value)
    {
        GetParent().SendMessage(MessageType.SetColor, index, ((int)(value.r * 255)).ToString() + "," + ((int)(value.g * 255)).ToString() + "," + ((int)(value.b * 255)).ToString());
    }
}