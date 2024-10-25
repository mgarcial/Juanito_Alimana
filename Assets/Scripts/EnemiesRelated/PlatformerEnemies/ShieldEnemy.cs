using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoShieldEnemy : Enemy
{
    [SerializeField] float maxHitPoints;
    private float hitPoints;
    public HealthbarBehaviour healthBar;

    public float HitPoints
    {
        get { return maxHitPoints; }
        set { maxHitPoints = value; }
    }
    public bool IsDead()
    {
        return hitPoints <= 0;
    }

    private void Start()
    {
        hitPoints = maxHitPoints;
        healthBar.SetHealth(hitPoints, maxHitPoints);
    }

    internal void TakeDamage(int dmg)
    {
        hitPoints -= dmg;
        healthBar.SetHealth(hitPoints, maxHitPoints);
        if (hitPoints <= 0)
        {
            AudioManager.GetInstance().PlayEnemyDeath();
            gameObject.SetActive(false);
            Invoke("Death", 1f);
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
