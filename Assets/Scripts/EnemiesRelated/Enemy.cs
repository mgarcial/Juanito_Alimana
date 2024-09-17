using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public GameObject hitEffects;
    public void TakeHit()
    {
        if (hitEffects != null)
        {
            hitEffects.SetActive(true);
            Invoke("DeactivateParticles", 1f);
        }
    }
    private void DeactivateParticles()
    {
        hitEffects.SetActive(false);
    }

}
