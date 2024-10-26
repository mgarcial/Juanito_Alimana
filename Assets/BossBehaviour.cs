using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossBehaviour : MonoBehaviour, IDamageable
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
    [SerializeField] private int hitPoints;
    [SerializeField] private int maxHitPoints;
    [Header("Checks")]
    private Transform playerPos;
    HealthbarBehaviour healthBar;
    [SerializeField] private pedroStates state;

    [SerializeField] private GameObject WinPanel;

    public bool isPushOnly => throw new System.NotImplementedException();

    private void Awake()
    {
        state = pedroStates.Awaken;
        healthBar = GetComponentInChildren<HealthbarBehaviour>();
    }
    private void Start()
    {
        hitPoints = maxHitPoints;
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
    private void Death()
    {
        state = pedroStates.Dead;
        WinGame();
    }

    void Awaken()
    {
        //Trigger appeareance animation
    }

    void WinGame()
    {
        WinPanel.SetActive(true);
    }

    public void TakeHit(int dmg)
    {
        hitPoints -= dmg;
        healthBar.SetHealth(hitPoints, maxHitPoints);
        if (hitPoints <= 0)
        {
            AudioManager.GetInstance().PlayEnemyDeath();
            Invoke("Death", 1f);
        }
    }
}
