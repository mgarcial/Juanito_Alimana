using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    private playerController player;
    [Header("GunStats")]
    [SerializeField] private float range = 10f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float impactForce = 200f;

    public Transform firePoint;

    [SerializeField] private float timeToFire = 0f;

    private void Start()
    {
        if (player == null)
        {
            player = GetComponentInParent<playerController>();
        }
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
        Vector3 dir = player.playerFacingRight ? Vector3.right : Vector3.left; 
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
