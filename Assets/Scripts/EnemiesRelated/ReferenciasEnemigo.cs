using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenciasEnemigo : MonoBehaviour
{

    public Transform _transform;
    public Rigidbody _rigidbody;
    public Animator _animator;
    public BoxCollider revisiónSalto;
    public Transform centroDelMundo;
    public playerController _jugador;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        revisiónSalto = GetComponent<BoxCollider>();
    }
}
