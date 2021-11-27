using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaptainCoder.TileBuilder;

public class RampageBarellController : MonoBehaviour, ITileObject
{
    public char TextChar => '8';
    public int Health = 5;
    public bool Exploded = false;

    public int X, Y;

    public (int x, int y) Position
    {
        get => (X, Y);
        set
        {
            X = value.x;
            Y = value.y;
        }
    }

    private MeshExploder MeshExploder
    {
        get => this.transform.Find("Mesh").GetComponent<MeshExploder>();
    }

    public void Explode()
    {
        if (PlayerController.INSTANCE.Position != this.Position || Exploded)
        {
            return;
        }
        Adventurer adventurer = GameState.Instance._Adventurer;

        if (adventurer.Health <= 0)
        {
            MessageController.Instance.DisplayMessage("You're too tired!");
            return;
        }

        adventurer.Health--;
        Weapon w = GameState.Instance._Adventurer.weapon;
        int damage = Random.Range(w.MinDamage, w.MaxDamage);

        this.Health -= damage;
        if (this.Health > 0)
        {
            AudioController.Instance.Punch.Play();
            return;
        }

        AudioController.Instance.Barrel.Play();


        GameState.Instance._Adventurer.BarrelsDestroyed++;
        TileMapController.Barrels--;

        if (GameState.Instance._Adventurer.BarrelsDestroyed % 5 == 0)
        {
            MessageController.Instance.DisplayMessage($"You found a key!");
            GameState.Instance._Adventurer.Keys++;
        }
        else
        {
            int gold = Random.Range(3, 15);
            MessageController.Instance.DisplayMessage($"You found {gold} gold!");
            GameState.Instance._Adventurer.Gold += gold;
        }
        GameObject rv = this.MeshExploder.Explode();
        MeshExploder.gameObject.SetActive(false);
        Exploded = true;
    }

    public void Interact()
    {
        this.Explode();
    }

    public ITileObject Spawn(ITile parent)
    {
        TileController tile = (TileController)parent;
        GameObject obj = UnityEngine.Object.Instantiate(this.gameObject);
        obj.SetActive(true);
        obj.transform.Find("Mesh").gameObject.SetActive(true);

        ITileObject tileObj = obj.GetComponent<ITileObject>();
        tileObj.Position = parent.Position;
        obj.transform.parent = tile.transform;
        obj.transform.localPosition = new Vector3();
        return tileObj;
    }
}
