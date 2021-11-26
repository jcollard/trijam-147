using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDisplayController : MonoBehaviour
{
    public UnityEngine.UI.Text Text;
    public float Duration;
    public Vector3 EndPosition;
    private Vector3 StartPosition;
    private float StartTime;
    private float EndTime;
    void Start()
    {
        StartPosition = new Vector3(0,0,0);
        StartTime = Time.time;
        EndTime = StartTime + Duration;
    }
    // Update is called once per frame
    void Update()
    {
        
        float percent = Mathf.Clamp((Time.time - StartTime) / (EndTime - StartTime), 0, 1);
        this.transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, percent);
        if (percent >= 1)
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }
}
