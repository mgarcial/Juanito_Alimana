using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform weaponHolder;
    private Gun currentGun;

    private void Update()
    {
        if(Input.GetButtonDown("Fire1") && currentGun != null)
        {
            currentGun.Shoot();
        }
    }

    public void EquipGun(Gun gun)
    {
        if (currentGun != null)
        {
            Destroy(currentGun.gameObject);
        }

        currentGun = Instantiate(gun,weaponHolder.position,weaponHolder.rotation,weaponHolder);
        currentGun.firePoint = currentGun.transform.Find("FirePoint");
    }
}
