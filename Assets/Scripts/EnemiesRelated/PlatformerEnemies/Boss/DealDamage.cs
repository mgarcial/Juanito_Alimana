using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable player = collision.GetComponent<IDamageable>();
        PlayerController playerInvul = collision.GetComponent<PlayerController>();  
        if (player != null && !playerInvul.isInvulnerable)
        {
            Debug.Log("hi");
            player.TakeHit(5);
        }
    }
}
