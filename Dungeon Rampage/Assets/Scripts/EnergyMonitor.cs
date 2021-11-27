using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyMonitor : MonoBehaviour
{
    
    void Update()
    {
        Adventurer adventure = GameState.Instance._Adventurer;
        float percent = (1f - (((float)adventure.Health) / adventure.MaxHealth)) * 568;
        // Debug.Log(percent);
        RectTransform t = this.GetComponent<RectTransform>();
        // t.anchorMin = new Vector2(0, 0.5f);
        // t.anchorMax = new Vector2(1, 0.5f);
        // t.offsetMin = new Vector2(0, t.offsetMin.y);
        t.offsetMax = new Vector2(-percent,  t.offsetMax.y);
    }
}
