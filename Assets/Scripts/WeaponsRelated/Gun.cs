using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [Header("GunStats")]
    [SerializeField] private float range = 10f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float impactForce = 20f;

    public Transform firePoint;

    [SerializeField] private float timeToFire = 0f;

    public virtual void Shoot()
    {
        if (Time.time > timeToFire)
        {
            timeToFire = Time.time+1f/fireRate;
            RaycastShoot();
        }
    }

    private void RaycastShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }
}
