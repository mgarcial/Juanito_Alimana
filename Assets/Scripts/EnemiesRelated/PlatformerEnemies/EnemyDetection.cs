using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour, IPickableGun
{
   [SerializeField] private float detectionRange = 10f; 
    public LayerMask playerLayer;
    [SerializeField] private Gun enemyGun;
    private bool isEquipped = false;
    private bool enemyFacingRight;
    private Transform player;
    private EnemyPatrol enemy;
    [SerializeField] private Transform weaponHolder;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Jugador").transform;
        enemy = GetComponent<EnemyPatrol>();
    }

    private void Update()
    {
        DetectPlayerAndShoot(); 
    }

    private void DetectPlayerAndShoot()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            Debug.Log($"Found{player}");
            enemyGun.Shoot();
        }
    }

    public void PickUpGun(Gun gun)
    {
        if (isEquipped)
        {
            Destroy(enemyGun.gameObject);
            Debug.Log($"Destroying {enemyGun}");
        }

        enemyGun = Instantiate(gun, weaponHolder.position, weaponHolder.rotation, weaponHolder);

        enemyGun.firePoint = enemyGun.transform.Find("FirePoint");
        enemyGun.SetGunHolder(this);
        isEquipped = true;
        Debug.Log("Picked up gun: " + enemyGun);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, detectionRange); 
    }
    public bool IsFacingRight()
    {
        enemyFacingRight = enemy.IsMovingRight();
        return enemyFacingRight;
    }
}
