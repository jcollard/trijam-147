using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopItemController : MonoBehaviour
{
    public UnityEngine.UI.Image Image;
    public UnityEngine.UI.Text Description;
    public Action BuyAction;

    public void Buy()
    {
        this.BuyAction.Invoke();
    }

}
