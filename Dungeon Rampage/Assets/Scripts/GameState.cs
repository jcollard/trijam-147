using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameState : MonoBehaviour
{
    public bool CanMove => !WeaponShopController.IsOpen() && !ArmorShopController.IsOpen();
    public PlayerController _PlayerController;
    public TileMapController _TileMapController;
    public MessageController _MessageController;

    public Adventurer _Adventurer; 

    public WeaponShop _WeaponShop;
    public ArmorShop _ArmorShop;

    public void BuyWeapon(Weapon w)
    {
        if (w.Value > _Adventurer.Gold)
        {
            _MessageController.DisplayMessage("Not enough gold!");
            return;
        }
        _Adventurer.Gold -= w.Value;
        _Adventurer.weapon = w;
        _MessageController.DisplayMessage($"{w.Name} equipped!");
    }

    public void BuyArmor(Armor a)
    {
        if (a.Value > _Adventurer.Gold)
        {
            _MessageController.DisplayMessage("Not enough gold!");
            return;
        }
        _Adventurer.Gold -= a.Value;
        _Adventurer.armor = a;
        _MessageController.DisplayMessage($"{a.Name} equipped!");
    }
}