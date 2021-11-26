using UnityEngine;
using CaptainCoder.TileBuilder;
using System.Collections.Generic;

public class MapBuilderController : MonoBehaviour
{

    public Camera MainCamera;
    public TileMapController TileMapController;
    
    [SerializeField]
    private int _X;
    
    [SerializeField]
    private int _Y;
    public (int x, int y) Position
    {
        get => (this._X, this._Y);
        set
        {
            (int x, int y) = value;
            if (_X == x && _Y == y)
            {
                return;
            }
            _X = x;
            _Y = y;
            this.UpdatePosition();
        }
    }

    [SerializeField]
    private TileSide _Facing = TileSide.North;
    public TileSide Facing
    {
        get => this._Facing;
        set
        {
            if (value == TileSide.Top || value == TileSide.Bottom || value == _Facing)
            {
                return;
            }
            this._Facing = value;
            this.UpdatePosition();
        }
    }

    public static readonly Dictionary<TileSide, (int, int)> MoveLookup = new Dictionary<TileSide, (int, int)>();
    public static readonly Dictionary<TileSide, (int, int)> StrafeLookup = new Dictionary<TileSide, (int, int)>();
    public static readonly Dictionary<TileSide, (float, float)> PositionLookup = new Dictionary<TileSide, (float, float)>();
    public static readonly Dictionary<TileSide, Quaternion> RotationLookup = new Dictionary<TileSide, Quaternion>();

    static MapBuilderController()
    {
        MoveLookup[TileSide.North] = (0, 1);
        MoveLookup[TileSide.East] = (1, 0);
        MoveLookup[TileSide.South] = (0, -1);
        MoveLookup[TileSide.West] = (-1, 0);

        StrafeLookup[TileSide.North] = (1, 0);
        StrafeLookup[TileSide.East] = (0, -1);
        StrafeLookup[TileSide.South] = (-1, 0);
        StrafeLookup[TileSide.West] = (0, 1);

        PositionLookup[TileSide.North] = (0, -5);
        PositionLookup[TileSide.South] = (0, 5);
        PositionLookup[TileSide.East] = (-5, 0);
        PositionLookup[TileSide.West] = (5, 0);

        RotationLookup[TileSide.North] = Quaternion.Euler(0, 0, 0);
        RotationLookup[TileSide.East] = Quaternion.Euler(0, 90, 0);
        RotationLookup[TileSide.South] = Quaternion.Euler(0, 180, 0);
        RotationLookup[TileSide.West] = Quaternion.Euler(0, 270, 0);
    }

    public void MoveForward() => this.Move(MoveLookup[this.Facing]);
    public void MoveLeft() => this.FlipMove(StrafeLookup[this.Facing]);
    public void MoveRight() => this.Move(StrafeLookup[this.Facing]);
    public void MoveBackward() => FlipMove(MoveLookup[this.Facing]);
    private void FlipMove((int x, int y) offset) => this.Move((-offset.x, -offset.y));
    public void Move((int x, int y) offset)
    {
        // TODO: Check current tile and make sure you can't move through a wall
        this.Position = (this.Position.x + offset.x, this.Position.y + offset.y);
    }

    public void RotateLeft()
    {
        this.Facing = this.Facing switch
        {
            TileSide.North => TileSide.West,
            TileSide.West => TileSide.South,
            TileSide.South => TileSide.East,
            TileSide.East => TileSide.North,
            _ => this.Facing
        };
    }

    public void RotateRight()
    {
        this.Facing = this.Facing switch
        {
            TileSide.North => TileSide.East,
            TileSide.East => TileSide.South,
            TileSide.South => TileSide.West,
            TileSide.West => TileSide.North,
            _ => this.Facing
        };
    }

    public void UpdatePosition()
    {
        (float offX, float offZ) = PositionLookup[this.Facing];
        this.MainCamera.transform.position = new Vector3(this.Position.x * 10 + offX, 5, this.Position.y * 10 + offZ);
        this.MainCamera.transform.localRotation = RotationLookup[this.Facing];
    }

    public string GetMapString()
    {
        return this.TileMapController.Map.ToStringMap(this.Position.x, this.Position.y, this.Facing);
    }
}