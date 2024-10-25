using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossBehaviour : MonoBehaviour
{
    private enum pedroStates
    {
        Tired,
        Enraged,
        Normal, 
        Awaken,
        Dead
    }

    [Header("Stats")]
    private int hitPoints;
    [SerializeField] private int maxHitPoints;
    [Header("Checks")]
    private Transform playerPos;
    HealthbarBehaviour healthBar;
    [SerializeField] private pedroStates state;

    private void Awake()
    {
        state = pedroStates.Awaken;
        healthBar = GetComponent<HealthbarBehaviour>();
    }

    private void Update()
    {
        switch(state)
        {
            case pedroStates.Awaken:
                Awaken();
            break;
        }
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

    void Awaken()
    {
        //Trigger appeareance animation
    }

}
