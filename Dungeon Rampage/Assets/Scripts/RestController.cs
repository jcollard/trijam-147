using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestController : MonoBehaviour
{
    
    public void OnMouseDown()
    {
        Adventurer adventurer = GameState.Instance._Adventurer;
        if (adventurer.Health < adventurer.MaxHealth)
        {

            MessageController.Instance.DisplayMessage($"You rest and recover {adventurer.MaxHealth - adventurer.Health} health.");
            adventurer.Health = adventurer.MaxHealth;
        }
        else 
        {
            MessageController.Instance.DisplayMessage("You don't need rest. Come back if you need to recover health.");
        }
    }
}
