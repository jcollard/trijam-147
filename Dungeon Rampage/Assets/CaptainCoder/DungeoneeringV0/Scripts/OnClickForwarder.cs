using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnClickForwarder : MonoBehaviour
{
    public UnityEvent OnClick;

    void OnMouseUpAsButton()
    {
        if (OnClick != null)
        {
            OnClick.Invoke();
        }
    }
}