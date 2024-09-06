using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenciasEnemigo : MonoBehaviour
{

    [HideInInspector] public Transform _transform;
    [HideInInspector] public Rigidbody _rigidbody;
    [HideInInspector] public Animator _animator;
    [HideInInspector] public BoxCollider revisiónSalto;
    public Transform centroDelMundo;
    public Transform _jugador;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        revisiónSalto = GetComponent<BoxCollider>();
    }
}
