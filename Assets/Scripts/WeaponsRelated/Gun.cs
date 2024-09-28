using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Gun : MonoBehaviour, IPooledObject
{

    [Header("GunStats")]
    [SerializeField] private int range = 10;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float impactForce = 200f;

    [Header("Assign this")]
    public Transform firePoint;

    [SerializeField] private float timeToFire = 0f;

    [SerializeField] protected IPickableGun gunHolder;
    [SerializeField] private LayerMask layerShooted;

    [Header("Sounds")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;

    [SerializeField] GameObject bulletTrailPrefab;
    public float Range
    {
        get { return range; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"i touched{other.name} ");
        if (other.CompareTag("Platforms")) return;
        gunHolder = other.GetComponent<IPickableGun>();
        if (gunHolder != null && !gunHolder.IsWeaponEquipped())
        {
            gunHolder.PickUpGun(this);
            Debug.Log($"I'm the gun holder {gunHolder}");
            gameObject.SetActive(false);            
        }
    }
    public void SetGunHolder(IPickableGun holder)
    {
        gunHolder = holder;
    }

    public virtual void Shoot()
    {
        if (Time.time > timeToFire )
        {
            Debug.Log("Shooting");
            timeToFire = Time.time+1f/fireRate;
            RaycastShoot();
            AudioManager.GetInstance().PlayShootSound(shootSound);
        }
    }

    private void RaycastShoot()
    {

        Debug.Log("Shooting for real");
        if (gunHolder == null)
        {
            Debug.LogError("GunHolder is null when trying to shoot!");
        }
        Vector3 dir = gunHolder.IsFacingRight() ? Vector3.right : Vector3.left; 
        Debug.DrawRay(firePoint.position, dir * range, Color.green, 3.0f);

        GameObject bulletTrail = Instantiate(bulletTrailPrefab, firePoint.position, Quaternion.identity);
        BulletTrail trailScript = bulletTrail.GetComponent<BulletTrail>();
        Vector3 targetPosition = firePoint.position + dir * range;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, dir, range, layerShooted);
        if (hit.collider)
        {
            Debug.Log(hit.transform.name);
            targetPosition = hit.point;
            IDamageable damageable = hit.transform.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeHit();
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
        trailScript.targetPosition = targetPosition;
    }   
}
