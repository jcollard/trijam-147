using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainingTextUpdater : MonoBehaviour
{
    
    public UnityEngine.UI.Text ChestText;
    public UnityEngine.UI.Text BarrelText;
    public GameObject WinScreen;
    

    // Update is called once per frame
    void Update()
    {
        ChestText.text = $"Chests Remaining: {TileMapController.Chests}";
        BarrelText.text = $"Barrels Remaining: {TileMapController.Barrels}";
        if (TileMapController.Chests == 0 && TileMapController.Barrels == 0)
        {
            WinScreen.SetActive(true);
            this.gameObject.SetActive(false);
            AudioController.Instance.Music.Stop();
            AudioController.Instance.Fanfare.Play();
        }
    }
}
