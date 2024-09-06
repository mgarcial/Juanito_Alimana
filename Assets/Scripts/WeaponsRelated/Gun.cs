using UnityEngine;

public abstract class Gun : MonoBehaviour, IPooledObject
{

    [Header("GunStats")]
    [SerializeField] private float range = 10f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float impactForce = 200f;

    public Transform firePoint;

    [SerializeField] private float timeToFire = 0f;

    private IPickableGun gunHolder;

    public float Range
    {
        get { return range; }
    }

    private void OnTriggerEnter(Collider other)
    {
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

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }
}
