using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameState _GameState;

    public UnityEngine.UI.Image WeaponRenderer;
    public UnityEngine.UI.Text WeaponText;

    public UnityEngine.UI.Image ArmorRenderer;
    public UnityEngine.UI.Text ArmorText;

    public UnityEngine.UI.Text GoldText;

    public void Update()
    {
        this.UpdateScreen();
    }

    public void UpdateScreen()
    {
        Weapon w = _GameState._Adventurer.weapon;
        WeaponRenderer.sprite = w.sprite;
        WeaponText.text = $"{w.Name}\nDamage: {w.MinDamage} - {w.MaxDamage}";

        Armor a = _GameState._Adventurer.armor;
        ArmorRenderer.sprite = a.sprite;
        ArmorText.text = $"{a.Name}\nArmor: {a.Defense}";

        GoldText.text = $"Gold: {_GameState._Adventurer.Gold}";
    }
}
