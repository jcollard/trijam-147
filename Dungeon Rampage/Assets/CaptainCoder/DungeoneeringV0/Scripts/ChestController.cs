using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaptainCoder.TileBuilder;

public class ChestController : MonoBehaviour, ITileObject
{

    private Transform Lid
    {
        get => this.transform.Find("Lid");
    }

    private Vector3 EndPosition = new Vector3(0, 2.5f, 0.5f);
    private Quaternion EndRotation = Quaternion.Euler(-45, 180, 0);

    private Vector3 StartPosition = new Vector3(0, 1.5f, 0);
    private Quaternion StartRotation = Quaternion.Euler(0, 180, 0);
    public float OpenSpeed = 0.5f;

    [SerializeField]
    private bool IsFinished = false;
    public bool IsOpen
    {
        get => StartTime > 0;
        set
        {
            if (value)
            {
                StartTime = 1;
                EndTime = 1;
            }
            else
            {
                StartTime = -1;
                EndTime = -1;
            }
            Set();
        }
    }

    public char TextChar => 'm';

    [SerializeField]
    public int X, Y;
    
    public (int x, int y) Position 
    {
        get => (X, Y); 
        set { 
            X = value.x;
            Y = value.y;
        }
    }

    private float StartTime = -1f;
    private float EndTime = -1f;

    public void Update()
    {
        if (IsFinished || !IsOpen)
        {
            return;
        }
        Set();
    }

    private void Set()
    {
        if (!IsOpen)
        {
            Lid.localPosition = StartPosition;
            Lid.localRotation = StartRotation;
            IsFinished = false;
            return;
        }

        float percent = (Time.time - StartTime) / (EndTime - StartTime);
        Lid.localPosition = Vector3.Lerp(StartPosition, EndPosition, percent);
        Lid.localRotation = Quaternion.Lerp(StartRotation, EndRotation, percent);
        if (percent >= 1)
        {
            IsFinished = true;
        }
    }

    public void Open()
    {
        if (PlayerController.INSTANCE.Position != this.Position)
        {
            return;
        }
        if (!this.IsOpen)
        {
            this.StartTime = Time.time;
            this.EndTime = Time.time + this.OpenSpeed;
        }
    }

    public void Interact()
    {
        this.Open();
    }

    public ITileObject Spawn(ITile parent)
    {
        TileController tile = (TileController)parent;
        GameObject obj = UnityEngine.Object.Instantiate(this.gameObject);
        obj.SetActive(true);
        ITileObject tileObj = obj.GetComponent<ITileObject>();
        tileObj.Position = parent.Position;
        obj.transform.parent = tile.transform;
        obj.transform.localPosition = new Vector3(); 
        return tileObj;
    }
}
