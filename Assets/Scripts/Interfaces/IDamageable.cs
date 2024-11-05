using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeHit(int dmg);
    public bool isPushOnly { get; }
}
