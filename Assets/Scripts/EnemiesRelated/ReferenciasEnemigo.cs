using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenciasEnemigo : MonoBehaviour
{

    public Rigidbody _rigidbody;
    public Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
}
