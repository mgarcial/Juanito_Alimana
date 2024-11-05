using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


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
    [SerializeField] HealthbarBehaviour healthBar;

    [Header("Checks")]
    private Transform playerPos;
    [SerializeField] private pedroStates state;

    [Header("AttackPattern")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    private Vector3 leftHandInitialPos;
    private Vector3 rightHandInitialPos;
    public float attackSpeed = 2f;
    private bool isLeftHandTurn = true;
    public float attackInterval = 1.5f;
    [SerializeField] private Transform[] attackPositions;

    [SerializeField] private Transform boss;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject hintPanel;
    public Vector3 growSize = new Vector3(5f, 5f, 5f);
    public float growthDuration = 2f;
    private Vector3 initialSize;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    public bool isPushOnly => true;

    private int attackCount;
    private int maxAttacks;

    private float tiredDuration = 5f;

    private void Awake()
    { 
        StartCoroutine(Awaken());
        state = pedroStates.Awaken;
    }
    private void Start()
    {
        initialSize = boss.localScale;
        hitPoints = maxHitPoints;
        healthBar.SetHealth(hitPoints, maxHitPoints);
        left.SetActive(false);
        right.SetActive(false);
    }

    private void Death()
    {
        state = pedroStates.Dead;
        WinGame();
    }

    private IEnumerator Attack()
    {
        //Make him attack a few times
        while (attackCount > 0 && state == pedroStates.Normal)
        {
            PerformAttack();
            attackCount--;
            yield return new WaitForSeconds(attackInterval);
        }
        if (attackCount <= 0)
        {
            //then it gets tired
            state = pedroStates.Tired;
            StartCoroutine(TiredState());
        }
    }
    private void PerformAttack()
    {
        Transform randomAttackPosition = attackPositions[Random.Range(0, attackPositions.Length)];

        if (isLeftHandTurn)
        {
            StartCoroutine(SlapHand(leftHand, leftHandInitialPos, randomAttackPosition.position));
        }
        else
        {
            StartCoroutine(SlapHand(rightHand, rightHandInitialPos, randomAttackPosition.position));
        }
        isLeftHandTurn = !isLeftHandTurn;
    }
    private IEnumerator SlapHand(Transform hand, Vector3 initialPosition, Vector3 targetPosition)
    {
        while (Vector3.Distance(hand.position, targetPosition) > 0.1f)
        {
            hand.position = Vector3.MoveTowards(hand.position, targetPosition, attackSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        while (Vector3.Distance(hand.position, initialPosition) > 0.1f)
        {
            hand.position = Vector3.MoveTowards(hand.position, initialPosition, attackSpeed * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator TiredState()
    {
        Debug.Log("El boss está cansado...");
        left.SetActive(true);
        right.SetActive(true);
        leftHand.gameObject.SetActive(false);
        rightHand.gameObject.SetActive(false);
        hintPanel.SetActive(true);  
        //trigger animacion de caerse
        yield return new WaitForSeconds(tiredDuration);
        Debug.Log("El boss ha recuperado su energía");
        state = pedroStates.Normal;
        GenerateAttacks();
        hintPanel.SetActive(false);

        StartCoroutine(Attack());
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
            leftHandInitialPos = leftHand.position;
            rightHandInitialPos = rightHand.position;
            state = pedroStates.Normal;
            GenerateAttacks();
            StartCoroutine(Attack());
        }
    }
    private void GenerateAttacks()
    {
        maxAttacks = Random.Range(3, 6);
        attackCount = maxAttacks;
        left.SetActive(false);
        right.SetActive(false);
        leftHand.gameObject.SetActive(true);
        rightHand.gameObject.SetActive(true);
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
