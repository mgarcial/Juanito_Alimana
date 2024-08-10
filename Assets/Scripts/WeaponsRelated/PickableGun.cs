using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableGun : MonoBehaviour
{
    public Gun gunPrefab;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Picking weapon");
        GunController controller = other.GetComponent<GunController>();
        if (controller != null )
        {
            controller.EquipGun(gunPrefab);
            Destroy(gameObject);
        }
    }
}
