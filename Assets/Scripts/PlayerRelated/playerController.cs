using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerController : MonoBehaviour, IPickableGun
{

    private Gun currentGun;
    public Transform weaponHolder;
    private bool isEquipped = false;
    public bool playerFacingRight;
    
    public void PickUpGun(Gun gun)
    {
        if (isEquipped)
        {
            Destroy(currentGun.gameObject);
            Debug.Log($"Destroying {currentGun}");
        }

        currentGun = Instantiate(gun, weaponHolder.position, weaponHolder.rotation, weaponHolder);

        currentGun.firePoint = currentGun.transform.Find("FirePoint");
        currentGun.SetGunHolder(this);
        isEquipped = true;
        Debug.Log("Picked up gun: " + currentGun);
    }

    public bool IsFacingRight()
    {
        return playerFacingRight;
    }
}
