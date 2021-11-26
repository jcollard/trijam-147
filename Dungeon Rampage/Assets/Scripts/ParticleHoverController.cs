using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHoverController : MonoBehaviour
{
    public ParticleSystem particles;
    public void OnMouseEnter()
    {
        this.particles.gameObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        this.particles.gameObject.SetActive(false);
    }
}
