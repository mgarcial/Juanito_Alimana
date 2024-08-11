using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform weaponHolder;
    private Gun currentGun;
    private bool isEquipped = false;
    public Gun CurrentGun { get => currentGun; set => currentGun = value; }
    private void Update()
    {
        if(Input.GetButtonDown("Fire1") && currentGun != null)
        {
            currentGun.Shoot();
        }
    }

    public void EquipGun(Gun gun)
    {
        if (isEquipped)
        {
            Destroy(currentGun.gameObject);
            Debug.Log($"Destroying {currentGun}");
        }

        currentGun = Instantiate(gun,weaponHolder.position,weaponHolder.rotation,weaponHolder);

        currentGun.firePoint = currentGun.transform.Find("FirePoint");
        isEquipped = true;
    }
}
