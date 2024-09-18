using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPickableGun, IDamageable
{
    [Header("Enemy stats")]
    [SerializeField] private float shootingRange = 6f;
    [Header("Enemy Checks")]
    [SerializeField] private Gun enemyGun;
    [SerializeField] private bool isEquipped = false;
    [SerializeField] private bool enemyFacingRight;
    private EnemyPatrol enemy;
    [Header("Things to assign")]
    public GameObject hitEffects;
    [SerializeField] private Transform weaponHolder;

    private void Start()
    {
        enemy = GetComponent<EnemyPatrol>();
    }

    private void Update()
    {
        DetectPlayerAndShoot(); 
    }

    private void DetectPlayerAndShoot()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, CharacterController.Instance.GetPosition());
        if (distanceToPlayer <= shootingRange)
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, shootingRange); 
    }
    public bool IsFacingRight()
    {
        enemyFacingRight = enemy.IsMovingRight();
        return enemyFacingRight;
    }
}
