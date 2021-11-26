using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUIController : MonoBehaviour
{
    public GameObject UIElement;

    public void ToggleUIElement ()
    {
        UIElement.SetActive(!UIElement.activeInHierarchy);
    }

    public void OnMouseDown()
    {
        ToggleUIElement();
    }
}
