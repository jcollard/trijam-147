using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorShopController : MonoBehaviour
{
    public ShopItemController TemplateItem;
    public Transform ItemContainer;
    public GameState _GameState;
    // Start is called before the first frame update
    void Start()
    {
        int y = 0;
        Collard.UnityUtils.DestroyImmediateChildren(ItemContainer);
        foreach (Armor a in _GameState._ArmorShop.items)
        {
            ShopItemController item = UnityEngine.Object.Instantiate<ShopItemController>(TemplateItem);
            item.Image.sprite = a.sprite;
            item.Description.text = $"{a.Name}\nArmor: {a.Defense}\nPrice: {a.Value} gold.";
            item.gameObject.name = a.Name;
            item.transform.parent = ItemContainer;
            item.transform.localPosition = new Vector3(0, y, 0);
            item.transform.localScale = new Vector3(1,1,1);
            item.gameObject.SetActive(true);
            // TODO: Add Buy Item
            y += -256;
        }
    }

}
