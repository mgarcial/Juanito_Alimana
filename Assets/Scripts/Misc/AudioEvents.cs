using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AudioEvents 
{
    public static UnityEvent OnShoot = new UnityEvent();
    public static UnityEvent OnJump = new UnityEvent();
    public static UnityEvent OnGameOver = new UnityEvent();
}
