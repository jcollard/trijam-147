using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopController : MonoBehaviour
{
    public static bool IsOpen()
    {
        if (Instance == null) {
            return false;
        }
        return Instance.gameObject.activeInHierarchy;
    }
    private static WeaponShopController Instance;
    public ShopItemController TemplateItem;
    public Transform ItemContainer;
    public GameState _GameState;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        int y = 0;
        Collard.UnityUtils.DestroyImmediateChildren(ItemContainer);
        foreach (Weapon w in _GameState._WeaponShop.items)
        {
            ShopItemController item = UnityEngine.Object.Instantiate<ShopItemController>(TemplateItem);
            item.Image.sprite = w.sprite;
            item.Description.text = $"{w.Name}\nDamage: {w.MinDamage} - {w.MaxDamage}\nPrice: {w.Value} gold.";
            item.gameObject.name = w.Name;
            item.transform.SetParent(ItemContainer, false);
            item.transform.localPosition = new Vector3(0, y, 0);
            item.transform.localScale = new Vector3(1,1,1);
            item.gameObject.SetActive(true);
            item.BuyAction = () => _GameState.BuyWeapon(w);
            y += -256;
        }
    }
}
