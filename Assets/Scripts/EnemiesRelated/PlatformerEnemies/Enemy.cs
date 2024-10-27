using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPickableGun, IDamageable
{

    [Header("Enemy stats")]
    [SerializeField] private float shootingRange = 6f;
    [Header("Enemy Checks")]
    [SerializeField] private Gun enemyGun;
    [SerializeField] private bool isGunEquipped = false;
    [SerializeField] private bool enemyFacingRight;
    private EnemyPatrol enemy;
    [Header("Things to assign")]
    public GameObject hitEffects;
    [SerializeField] private Transform weaponHolder;
    public bool enemyHasHealth = false;

    public bool isPushOnly => true;

    private void Start()
    {
        enemy = GetComponent<EnemyPatrol>();
    }

    private void Update()
    {
        if (enemyGun != null && enemyGun.canShoot)
        {
            DetectPlayerAndShoot();
        }
    }

    private void DetectPlayerAndShoot()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.Instance.GetPosition());
        if (distanceToPlayer <= shootingRange)
        {
            enemyGun.bulletsShot = enemyGun.bulletPerTap;
            enemyGun.Shoot();
            enemyGun.canShoot = false;
        }
    }

    public void PickUpGun(Gun gun)
    {
        if (isGunEquipped)
        {
            Destroy(enemyGun.gameObject);
            Debug.Log($"Destroying {enemyGun}");
            isGunEquipped=false;
        }

        if (!isGunEquipped) 
        {
            enemyGun = Instantiate(gun, weaponHolder.position, weaponHolder.rotation, weaponHolder);
            enemyGun.SetGunHolder(this);
            isGunEquipped = true; 
        }
       
        Debug.Log("Picked up gun: " + enemyGun);
    }
    public bool IsWeaponEquipped()
    {
        return isGunEquipped;
    }
    public void TakeHit(int dmg)
    {
        if (hitEffects != null)
        {
            AudioManager.GetInstance().PlayHitEnemySound();
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
