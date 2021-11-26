using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopController : MonoBehaviour
{
    public ShopItemController TemplateItem;
    public Transform ItemContainer;
    public GameState _GameState;
    // Start is called before the first frame update
    void Start()
    {
        int y = 0;
        Collard.UnityUtils.DestroyImmediateChildren(ItemContainer);
        foreach (Weapon w in _GameState._WeaponShop.items)
        {
            ShopItemController item = UnityEngine.Object.Instantiate<ShopItemController>(TemplateItem);
            item.Image.sprite = w.sprite;
            item.Description.text = $"{w.Name}\nDamage: {w.MinDamage} - {w.MaxDamage}\nPrice: {w.Value} gold.";
            item.gameObject.name = w.Name;
            item.transform.parent = ItemContainer;
            item.transform.localPosition = new Vector3(0, y, 0);
            item.transform.localScale = new Vector3(1,1,1);
            item.gameObject.SetActive(true);
            // TODO: Add Buy Item
            y += -256;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
