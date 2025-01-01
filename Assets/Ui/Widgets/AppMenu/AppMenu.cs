using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
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
        switch (dropDown.GetComponent<TMPro.TMP_Dropdown>().value)
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
}