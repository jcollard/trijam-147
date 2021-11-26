using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CaptainCoder.TileBuilder;

public class TileController : MonoBehaviour, ITile
{

    public (int x, int y) Position {get; set;}
    private ITile _DelegateTile;
    public ITile DelegateTile
    {
        get => _DelegateTile == null ? InitDelegateTile() : _DelegateTile;
        set => _DelegateTile = value;
    }

    [SerializeField] // TODO: Should I clear this out?
    private Dictionary<TileSide, MeshRenderer> _tileRenderers;

    private Dictionary<TileSide, MeshRenderer> MeshRenderers
    {
        get
        {
            if (_tileRenderers == null)
            {
                _tileRenderers = InitRenderers();
            }
            return _tileRenderers;
        }
    }

    [SerializeField]
    private Dictionary<TileSide, WallType> _WallTypes;

    private Dictionary<TileSide, WallType> WallTypes
    {
        get
        {
            if (_WallTypes == null)
            {
                _WallTypes = InitWallTypes();
            }
            return _WallTypes;
        }
    }


    [SerializeField]
    private Material _WallTexture;
    public Material WallTexture
    {
        get => _WallTexture;
        set
        {
            _WallTexture = value;
            foreach (TileSide side in TileUtils.WALLS)
            {
                MeshRenderers[side].material = _WallTexture;
            }
        }
    }

    [SerializeField]
    private Material _DoorTexture;
    public Material DoorTexture
    {
        get => _DoorTexture;
        set
        {
            _DoorTexture = value;
            foreach (TileSide side in TileUtils.WALLS)
            {
                if (this.GetSide(side) == WallType.Door)
                {
                    MeshRenderers[side].material = _DoorTexture;
                }
            }
        }
    }

    [SerializeField]
    private Material _BottomTexture;
    public Material BottomTexture
    {
        get => _BottomTexture;
        set
        {
            _BottomTexture = value;
            MeshRenderers[TileSide.Bottom].material = _BottomTexture;
        }
    }

    [SerializeField]
    private Material _TopTexture;

    public Material TopTexture
    {
        get => _TopTexture;
        set
        {
            _TopTexture = value;
            MeshRenderers[TileSide.Top].material = _TopTexture;
        }
    }

    public bool HasObject => DelegateTile.HasObject;
    public ITileObject Object { get => DelegateTile.Object; set => DelegateTile.Object = value; }
    public char TextChar => DelegateTile.TextChar;
    public void RemoveObject() => DelegateTile.RemoveObject();

    public ITileObject Spawned {get; set;}
    

    private ITile InitDelegateTile() => new BasicTile(this.Position);
    private Dictionary<TileSide, MeshRenderer> InitRenderers()
    {
        Dictionary<TileSide, MeshRenderer> renderers = new Dictionary<TileSide, MeshRenderer>();
        // TODO: Potentially optimize this to not run if it is already initialized
        Transform n = this.transform.Find("Walls").Find("North");
        Transform e = this.transform.Find("Walls").Find("East");
        Transform s = this.transform.Find("Walls").Find("South");
        Transform w = this.transform.Find("Walls").Find("West");
        Transform bottom = this.transform.Find("Bottom");
        Transform top = this.transform.Find("Top");
        renderers[TileSide.North] = n.gameObject.GetComponent<MeshRenderer>();
        renderers[TileSide.East] = e.gameObject.GetComponent<MeshRenderer>();
        renderers[TileSide.South] = s.gameObject.GetComponent<MeshRenderer>();
        renderers[TileSide.West] = w.gameObject.GetComponent<MeshRenderer>();
        renderers[TileSide.Top] = top.GetComponent<MeshRenderer>();
        renderers[TileSide.Bottom] = bottom.GetComponent<MeshRenderer>();
        return renderers;
    }

    private Dictionary<TileSide, WallType> InitWallTypes()
    {
        Dictionary<TileSide, WallType> types = new Dictionary<TileSide, WallType>();
        foreach (TileSide side in TileUtils.ALL)
        {
            types[side] = WallType.Wall;
        }
        return types;
    }

    public bool HasSide(TileSide side) => this.MeshRenderers[side].gameObject.activeInHierarchy;


    public void SetSide(TileSide side, WallType wallType)
    {
        this.WallTypes[side] = wallType;
        if (wallType == WallType.None)
        {
            this.MeshRenderers[side].gameObject.SetActive(false);
            return;
        }
        // TODO: Else determine which object to render.
        this.MeshRenderers[side].gameObject.SetActive(true);
        if (side == TileSide.Top || side == TileSide.Bottom)
        {
            return;
        }

        this.MeshRenderers[side].material = this.GetMaterial(wallType);
    }

    public WallType GetSide(TileSide side) => this.WallTypes[side];


    private Material GetMaterial(WallType type)
    {
        return type switch
        {
            WallType.Wall => this.WallTexture,
            WallType.Door => this.DoorTexture,
            _ => throw new Exception($"Illegal WallType.")
        };
    }

    public void Interact()
    {
        if (this.Spawned != null)
        {
            this.Spawned.Interact();
        }
    }

    public void SpawnObject()
    {
        if (this.HasObject)
        {
            this.Spawned = this.Object.Spawn(this);
            DelegateTile.Spawned = this.Spawned;
        }    
    }
}