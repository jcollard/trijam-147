using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaptainCoder.TileBuilder;

public class RampageBarellController : MonoBehaviour, ITileObject
{
    public char TextChar => '8';
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
        int gold = Random.Range(3, 20);
        MessageController.Instance.DisplayMessage($"You found {gold} gold!");
        GameState.Instance._Adventurer.Gold += gold;
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
