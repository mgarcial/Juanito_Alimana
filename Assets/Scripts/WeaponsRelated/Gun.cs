using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Gun : MonoBehaviour, IPooledObject
{

    [Header("GunStats that change")]
    [SerializeField] private int range = 10;
    [SerializeField] private float timeBetweenShooting;
    [Tooltip("Este es para la semi auto, los otros en 0")][SerializeField] private float timeBetweenShots;
    [SerializeField] private float impactForce = 200f;
    [SerializeField] private float recoilForce = 5f;
    [SerializeField] private float reloadTime;
    [SerializeField] private int magazineSize;
    public int bulletPerTap;
    
    private int bulletsLeft;
    [HideInInspector] public int bulletsShot;
     public bool shooting, canShoot, reloading;
    [Header("Assign this")]
    public Transform firePoint;

    [SerializeField] protected IPickableGun gunHolder;
    [SerializeField] private LayerMask layerShooted;

    [Header("Sounds")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;

    [SerializeField] GameObject bulletTrailPrefab;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        canShoot = true;
    }
    public float Range
    {
        get { return range; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"i touched{other.name} ");
        if (other.CompareTag("Platforms")) return;
        gunHolder = other.GetComponent<IPickableGun>();
        if (gunHolder != null && !gunHolder.IsWeaponEquipped())
        {
            gunHolder.PickUpGun(this);
            //Debug.Log($"I'm the gun holder {gunHolder}");
            gameObject.SetActive(false);            
        }
    }
    public void SetGunHolder(IPickableGun holder)
    {
        gunHolder = holder;
    }

    public virtual void Shoot()
    {
        if (bulletsLeft >= bulletsShot)
        {
            //Debug.Log("Shooting");
            RaycastShoot();
            Rigidbody2D holder = GetComponentInParent<Rigidbody2D>();
            Vector3 dir = firePoint.right;
            holder.AddForce(-dir * recoilForce, ForceMode2D.Impulse);
            AudioManager.GetInstance().PlayShootSound(shootSound);
            bulletsLeft--;
            bulletsShot--;
            Invoke("ResetShot", timeBetweenShooting);
            if(bulletsShot > 0 && bulletsLeft > 0) 
            Invoke("Shoot", timeBetweenShots); 
        }
        else
        {
            Reload();
        }
    }
    public void ResetShot()
    {
        canShoot = true;
    }

    public void Reload()
    {
        reloading = true;
        AudioManager.GetInstance().PlayReloadSound(reloadSound);
        Invoke("ReloadFinished", reloadTime);
    }
    public void ReloadFinished()
    {
        reloading = false;
        bulletsLeft = magazineSize;
        canShoot = true;
    }
    private void RaycastShoot()
    {

        //Debug.Log("Shooting for real");
        if (gunHolder == null)
        {
            Debug.LogError("GunHolder is null when trying to shoot!");
        }
        Vector3 dir =  firePoint.right; 
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
