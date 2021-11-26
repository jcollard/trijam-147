using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHoverController : MonoBehaviour
{
    public ParticleSystem particles;
    public string Message;
    public void OnMouseEnter()
    {
        this.particles.gameObject.SetActive(true);
        if (Message != null && Message != string.Empty)
        {
            MessageController.Instance.DisplayMessage(Message);
            Message = string.Empty;
        }
    }

    public void OnMouseExit()
    {
        this.particles.gameObject.SetActive(false);
    }
}
