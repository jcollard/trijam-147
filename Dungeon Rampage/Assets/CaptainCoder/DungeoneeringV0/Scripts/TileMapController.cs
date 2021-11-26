using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaptainCoder.TileBuilder;

public class TileMapController : MonoBehaviour
{


    public TileController TileTemplate;
    public Transform Container;

    [SerializeField]
    private Material _DoorTexture;
    public Material DoorTexture
    {
        get => _DoorTexture;
        set => this.UpdateIfDifferent(value, ref _DoorTexture);
    }

    [SerializeField]
    private Material _WallTexture;
    public Material WallTexture
    {
        get => _WallTexture;
        set => this.UpdateIfDifferent(value, ref _WallTexture);
    }

    [SerializeField]
    private Material _TopTexture;
    public Material TopTexture
    {
        get => _TopTexture;
        set => this.UpdateIfDifferent(value, ref _TopTexture);
    }

    [SerializeField]
    private Material _BottomTexture;
    public Material BottomTexture
    {
        get => _BottomTexture;
        set => this.UpdateIfDifferent(value, ref _BottomTexture);
    }

    private void UpdateIfDifferent<T>(T value, ref T any)
    {
        if (value == null || any == null || value.Equals(any))
        {
            return;
        }
        any = value;
        this.BuildTiles();
    }

    [SerializeField]
    private TileMapJSON _JSON;
    public TileMapJSON JSON
    {
        get => _JSON;
        private set
        {
            this._JSON = value;
        }
    }

    private TileMap _Map;
    public TileMap Map
    {
        get
        {
            if (this._Map == null || this._Map.IsEmpty)
            {
                this._Map = this.JSON.ToTileMap(this.LookupObject);
            }
            return this._Map;
        }
    }

    private ITileObject LookupObject(((int, int) pos, char c) info)
    {
        if (ObjectLookup.Lookup.TryGetValue(info.c, out GameObject obj))
        {
            ITileObject tileObject = obj.GetComponent<ITileObject>();
            tileObject.Position = info.pos;
            return tileObject;
        }
        return null;
    }

    [SerializeField]
    public ObjectLookupMap ObjectLookup = new ObjectLookupMap();

    private TileController CreateTile(ITile tile)
    {
        TileController newTile = UnityEngine.Object.Instantiate<TileController>(TileTemplate);
        newTile.DelegateTile = tile;
        newTile.Position = tile.Position;
        foreach (TileSide side in TileUtils.ALL)
        {
            newTile.SetSide(side, tile.HasSide(side) ? tile.GetSide(side) : WallType.None);
        }
        return newTile;
    }

    public void ToggleObject((int x, int y) pos)
    {
        if (!Map.HasTile(pos))
        {
            return;
        }

        ITile tile = Map.GetTile(pos);

        tile.Object = tile.TextChar switch
        {
            '.' => this.LookupObject((pos, '8')),
            '8' => this.LookupObject((pos, 'm')),
            _ => null,
        };

        this.BuildTiles();
    }

    public void BuildTiles()
    {
        Collard.UnityUtils.DestroyImmediateChildren(this.Container);
        TileTemplate.gameObject.SetActive(false);
        foreach ((int x, int y) pos in this.Map.GetAllPos())
        {
            TileController tile = this.CreateTile(this.Map.GetTile(pos));
            tile.WallTexture = this.WallTexture;
            tile.TopTexture = this.TopTexture;
            tile.BottomTexture = this.BottomTexture;
            tile.DoorTexture = this.DoorTexture;
            tile.gameObject.SetActive(true);
            tile.name = $"(x: {pos.x}, y: {pos.y})";
            tile.transform.parent = this.Container;
            tile.transform.localPosition = new Vector3((float)(pos.x * 10), 0, (float)(pos.y * 10));
            tile.SpawnObject();
        }
        this.JSON = new TileMapJSON(this.Map);
    }

    public void Start()
    {
        BuildTiles();
    }
}

[Serializable]
public class ObjectLookupMap
{
    public List<ObjectLookup> Objects = new List<ObjectLookup>();
    public Dictionary<char, GameObject> Lookup
    {
        get
        {
            Dictionary<char, GameObject> dict = new Dictionary<char, GameObject>();
            foreach (ObjectLookup obj in Objects)
            {
                dict[obj.Symbol] = obj.Template;
            }

            // TODO: Objects should register on ObjectLookupMap

            if (!dict.ContainsKey('8'))
            {
                dict['8'] = null;
            }

            if (!dict.ContainsKey('m'))
            {
                dict['m'] = null;
            }

            return dict;
        }
    }

    public ObjectLookupMap() { }
    public ObjectLookupMap(Dictionary<char, GameObject> map)
    {
        foreach (char key in map.Keys)
        {
            Objects.Add(new ObjectLookup(key, map[key]));
        }
    }

}

[Serializable]
public class ObjectLookup
{
    public char Symbol;
    public GameObject Template;

    public ObjectLookup(char Symbol, GameObject Template)
    {
        this.Symbol = Symbol;
        this.Template = Template;
    }

}