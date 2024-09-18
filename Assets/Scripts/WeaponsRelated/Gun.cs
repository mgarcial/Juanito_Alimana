using UnityEngine;
using UnityEngine.UIElements;

public abstract class Gun : MonoBehaviour, IPooledObject
{

    [Header("GunStats")]
    [SerializeField] private float range = 10f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float impactForce = 200f;

    public Transform firePoint;

    [SerializeField] private float timeToFire = 0f;

    protected IPickableGun gunHolder;

    [Header("Sounds")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;

    public float Range
    {
        get { return range; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Platforms")) return;
        transform.Rotate(new Vector3(0f, -90f, 0f));
        gunHolder = other.GetComponent<IPickableGun>();
        if (gunHolder != null )
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
        if (Time.time > timeToFire)
        {
            Debug.Log("Shooting");
            timeToFire = Time.time+1f/fireRate;
            RaycastShoot();
            AudioManager.GetInstance().PlayShootSound(shootSound);
        }
    }

    private void RaycastShoot()
    {

        RaycastHit hit;
        Debug.Log("Shooting for real");
        if (gunHolder == null)
        {
            Debug.LogError("GunHolder is null when trying to shoot!");
            return;
        }
        Vector3 dir = gunHolder.IsFacingRight() ? Vector3.right : Vector3.left; 
        Debug.DrawRay(firePoint.position, dir * range, Color.green, 3.0f);

        if (Physics.Raycast(firePoint.position, dir, out hit, range))
        {
            Debug.Log(hit.transform.name);
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeHit();
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }
}
