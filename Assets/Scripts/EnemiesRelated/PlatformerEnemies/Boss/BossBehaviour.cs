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

    [SerializeField] private Transform boss;
    [SerializeField] private GameObject WinPanel;
    public Vector3 growSize = new Vector3(5f, 5f, 5f);
    public float growthDuration = 2f;
    private Vector3 initialSize;
    public bool isPushOnly => true;

    private void Awake()
    {
        state = pedroStates.Awaken;
        healthBar = GetComponentInChildren<HealthbarBehaviour>();
    }
    private void Start()
    {
        initialSize = boss.localScale;
        hitPoints = maxHitPoints;
    }

    private void Update()
    {
        switch(state)
        {
            case pedroStates.Awaken:
                StartCoroutine(Awaken());
            break;
            case pedroStates.Normal:

            break;
        }
    }
    private void Death()
    {
        state = pedroStates.Dead;
        WinGame();
    }

    private IEnumerator Awaken()
    {
        float elapsedTime = 0;
        while (elapsedTime < growthDuration)
        {
            boss.localScale = Vector3.Lerp(initialSize, growSize, elapsedTime / growthDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        boss.localScale = growSize;
        if (boss.localScale == growSize) 
        {
            state = pedroStates.Normal;
        }
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
