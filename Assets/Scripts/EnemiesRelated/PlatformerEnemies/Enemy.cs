using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPickableGun, IDamageable
{
    [SerializeField] private float detectionRange = 10f; 
    [SerializeField] private Gun enemyGun;
    private bool isEquipped = false;
    private bool enemyFacingRight;
    private CharacterController player;
    private EnemyPatrol enemy;
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
        Gizmos.DrawWireSphere(transform.position, detectionRange); 
    }
    public bool IsFacingRight()
    {
        enemyFacingRight = enemy.IsMovingRight();
        return enemyFacingRight;
    }
}
