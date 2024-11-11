using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
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
