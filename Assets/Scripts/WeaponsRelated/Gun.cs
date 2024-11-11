using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Gun : MonoBehaviour, IPooledObject
{

    [Header("GunStats that change")]
    [SerializeField] private int range = 10;
    [SerializeField] private float timeBetweenShooting;
    [Tooltip("Este es para la semi auto, los otros en 0")][SerializeField] private float timeBetweenShots;
    [SerializeField] private float impactForce = 200f;
    [SerializeField] private float recoilForce = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float reloadTime;
    [SerializeField] private int magazineSize;
    [SerializeField] private int weaponDamage;
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

    public TextMeshProUGUI ammoDisplay;
    public Image reloadIcon; 

    private void Awake()
    {
        bulletsLeft = magazineSize;
        canShoot = true;
        UpdateAmmoDisplay();
    }

    public float Range
    {
        get { return range; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"i touched{other.name} ");
        if (other.GetComponent<IPickableGun>() == null) return;
        gunHolder = other.GetComponent<IPickableGun>();
        if (gunHolder != null && !gunHolder.IsWeaponEquipped() || other.CompareTag("Player"))
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
            UpdateAmmoDisplay();
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
         UpdateAmmoDisplay();
        AudioManager.GetInstance().PlayReloadSound(reloadSound);
        Invoke("ReloadFinished", reloadTime);
    }
    public void ReloadFinished()
    {
        reloading = false;
        bulletsLeft = magazineSize;
        canShoot = true;
        UpdateAmmoDisplay();
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
        trailScript.targetPosition = targetPosition;
        if (hit.collider)
        {
            Debug.Log(hit.transform.name);
            targetPosition = hit.point;
            trailScript.targetPosition = targetPosition;
            IDamageable damageable = hit.transform.GetComponentInParent<IDamageable>();
            NoShieldEnemy noShieldEnemy = hit.transform.GetComponentInParent<NoShieldEnemy>();
            if (damageable != null)
            { 
                damageable.TakeHit(weaponDamage);
                EnemyPatrol enemy = hit.transform.GetComponentInChildren<EnemyPatrol>();
                if (enemy != null)
                {
                    hit.rigidbody.velocity = Vector3.zero;
                    enemy.StopPatrol(knockbackDuration);
                    hit.rigidbody.AddForce(-hit.normal * impactForce, ForceMode2D.Impulse);
                }
                else if (noShieldEnemy != null)
                {
                    noShieldEnemy.TakeHit(weaponDamage);
                }
            }
        }
        
    }

    private void UpdateAmmoDisplay()
    {
        if (ammoDisplay != null && reloadIcon != null)
        {
            if(reloading)
            {
                ammoDisplay.gameObject.SetActive(false); 
                reloadIcon.gameObject.SetActive(true);
            }
            else
            {
                ammoDisplay.gameObject.SetActive(true); 
                reloadIcon.gameObject.SetActive(false);
                ammoDisplay.text = $"{bulletsLeft} <sprite name=\"bulletIcon\">";
            }
        }
    }
}
