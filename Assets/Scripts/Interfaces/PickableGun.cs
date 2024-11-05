using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickableGun 
{
    void PickUpGun(Gun gun);
    bool IsFacingRight();
    bool IsWeaponEquipped();
}
