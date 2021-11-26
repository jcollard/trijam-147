using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaptainCoder.TileBuilder;

public class PlayerController : MonoBehaviour
{
    public static PlayerController INSTANCE;

    public TileMapController TileMap;
    public Camera MainCamera;
    public float ActionSpeed;

    private bool _LookDown = false;
    public bool LookDown
    {
        get => _LookDown;
        set
        {
            if (value == _LookDown)
            {
                return;
            }


            (Vector3 startPos, Quaternion startRot) = this.GetPosition(this.Position, this.Facing, this._LookDown);
            (Vector3 endPos, Quaternion endRot) = this.GetPosition(this.Position, this.Facing, value);

            PlayerAction action = new PlayerAction(
                StartPosition: startPos,
                EndPosition: endPos,
                StartRotation: startRot,
                EndRotation: endRot
            );
            this.ActionQueue.Enqueue(action);

            _LookDown = value;

        }
    }

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

            (Vector3 startPos, Quaternion startRot) = this.GetPosition(this.Position, this.Facing, this.LookDown);
            (Vector3 endPos, Quaternion endRot) = this.GetPosition((x, y), this.Facing, this.LookDown);
            PlayerAction action = new PlayerAction(
                StartPosition: startPos,
                EndPosition: endPos,
                StartRotation: startRot,
                EndRotation: endRot
            );
            this.ActionQueue.Enqueue(action);

            _X = x;
            _Y = y;

            //this.SetPosition();
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

            (Vector3 startPos, Quaternion startRot) = this.GetPosition(this.Position, this._Facing, this.LookDown);
            (Vector3 endPos, Quaternion endRot) = this.GetPosition(this.Position, value, this.LookDown);
            PlayerAction action = new PlayerAction(
                StartPosition: startPos,
                EndPosition: endPos,
                StartRotation: startRot,
                EndRotation: endRot
            );
            this.ActionQueue.Enqueue(action);
            this._Facing = value;
            //this.SetPosition();
        }
    }

    private Queue<PlayerAction> ActionQueue = new Queue<PlayerAction>();
    private readonly Dictionary<string, Action> controls;
    public static readonly Dictionary<TileSide, (int, int)> MoveLookup = new Dictionary<TileSide, (int, int)>();
    public static readonly Dictionary<TileSide, (int, int)> StrafeLookup = new Dictionary<TileSide, (int, int)>();
    public static readonly Dictionary<TileSide, (float, float)> PositionLookup = new Dictionary<TileSide, (float, float)>();
    public static readonly Dictionary<TileSide, Quaternion> RotationLookup = new Dictionary<TileSide, Quaternion>();

    static PlayerController()
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

    public void Start()
    {
        PlayerController.INSTANCE = this;
        SetPosition();
    }

    public void MoveForward() => this.Move(MoveLookup[this.Facing]);
    public void MoveLeft() => this.FlipMove(StrafeLookup[this.Facing]);
    public void MoveRight() => this.Move(StrafeLookup[this.Facing]);
    public void MoveBackward() => FlipMove(MoveLookup[this.Facing]);
    private void FlipMove((int x, int y) offset) => this.Move((-offset.x, -offset.y));
    public void Move((int x, int y) offset)
    {
        TileSide side = this.FindSide(offset);
        ITile tile = this.TileMap.Map.GetTile(this.Position);
        if (tile.HasSide(side) && tile.GetSide(side) == WallType.Wall)
        {
            // TODO: Queue "hitting wall"
            Debug.Log("Bounce!");
            return;
        }
        this.Position = (this.Position.x + offset.x, this.Position.y + offset.y);
    }

    public void Interact()
    {
        ITile tile = this.TileMap.Map.GetTile(this.Position);
        if (tile.HasObject)
        {
            tile.Interact();
        }
    }

    private TileSide FindSide((int, int) toCheck)
    {
        foreach (TileSide side in MoveLookup.Keys)
        {
            (int, int) offset = MoveLookup[side];
            if (offset == toCheck)
            {
                return side;
            }
        }
        throw new Exception($"Bad move detected {toCheck}. Can only move orthoganally.");
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

    public void SetPosition()
    {
        (float offX, float offZ) = PositionLookup[this.Facing];
        MainCamera.transform.position = new Vector3(this.Position.x * 10 + offX, 5, this.Position.y * 10 + offZ);
        MainCamera.transform.localRotation = RotationLookup[this.Facing];
    }

    private (Vector3, Quaternion) GetPosition((int x, int y) pos, TileSide side, bool IsLookingDown)
    {
        (float offX, float offZ) = PositionLookup[side];
        Vector3 newPosition = new Vector3(pos.x * 10 + offX, 5, pos.y * 10 + offZ);
        Quaternion lookup = RotationLookup[side];
        Quaternion newRotation = Quaternion.Euler(IsLookingDown ? 45 : 0, lookup.eulerAngles.y, lookup.eulerAngles.z);
        return (newPosition, newRotation);
    }

    public PlayerController()
    {
        controls = new Dictionary<string, Action>();
        controls["RotateLeft"] = this.RotateLeft;
        controls["Forward"] = this.MoveForward;
        controls["RotateRight"] = this.RotateRight;
        controls["StrafeLeft"] = this.MoveLeft;
        controls["Backward"] = this.MoveBackward;
        controls["StrafeRight"] = this.MoveRight;
        controls["LookDown"] = () => this.LookDown = !this.LookDown;
        controls["Interact"] = this.Interact;

    }

    public void Update()
    {
        foreach (string control in controls.Keys)
        {
            if (Input.GetButtonUp(control))
            {
                controls[control]();
            }
        }

        SmoothUpdatePosition();
    }

    public void SmoothUpdatePosition()
    {
        if (this.ActionQueue.Count == 0)
        {
            return;
        }
        PlayerAction action = this.ActionQueue.Peek();
        if (ActionSpeed <= 0)
        {
            this.ActionQueue.Dequeue();
            this.SetPosition();
            return;
        }

        if (!action.Started)
        {
            action.Start(Time.time, Time.time + ActionSpeed);
        }
        float percentage = Mathf.Clamp((Time.time - action.StartTime) / (action.EndTime - action.StartTime), 0, 1);
        MainCamera.transform.position = Vector3.Lerp(action.StartPosition, action.EndPosition, percentage);
        MainCamera.transform.rotation = Quaternion.Lerp(action.StartRotation, action.EndRotation, percentage);
        if (action.EndTime < Time.time)
        {
            this.ActionQueue.Dequeue();
        }
    }

}

public class PlayerAction
{
    public readonly Vector3 StartPosition;
    public readonly Vector3 EndPosition;
    public readonly Quaternion StartRotation;
    public readonly Quaternion EndRotation;
    public bool Started { get; private set; }
    public float StartTime { get; private set; }
    public float EndTime { get; private set; }

    public PlayerAction(Vector3 StartPosition, Vector3 EndPosition, Quaternion StartRotation, Quaternion EndRotation)
    {
        this.StartPosition = StartPosition;
        this.EndPosition = EndPosition;
        this.StartRotation = StartRotation;
        this.EndRotation = EndRotation;
        this.Started = false;
    }

    public void Start(float StartTime, float EndTime)
    {
        if (this.Started)
        {
            throw new Exception("PlayerAction already started.");
        }
        this.StartTime = StartTime;
        this.EndTime = EndTime;
        Started = true;
    }

}