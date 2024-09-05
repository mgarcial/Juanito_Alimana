using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    private Maquina_Estado maquinaDeEstado;
    private ReferenciasEnemigo misReferencias;

    public int saltos = 2;
    public float tiempoEntreSaltos = 5f;
    public float maximaVelMovimiento = 15f;
    public float velocidadBuscarPiso = 3f;
    public Vector3 direccionSalto = new Vector3(1,1,0);

    [Header("Buscar piso")]
    [SerializeField] private LayerMask layerPlataforma;
    private Ray checkPiso;

    private void Awake()
    {
        //Setup 
        misReferencias = GetComponent<ReferenciasEnemigo>();
        maquinaDeEstado = new Maquina_Estado();
        checkPiso = new Ray(misReferencias._transform.position, -misReferencias._transform.up);


        //definición de los estados que se usarán
        var buscarPiso = new BuscarPiso(this, misReferencias);
        var buscarJugador = new BuscarJugador(this, misReferencias);

        //Trancisiones de estados
        maquinaDeEstado.AgregarCualquierTrancision(buscarPiso, NoTienePiso());

        maquinaDeEstado.AgregarTrancision(buscarPiso, buscarJugador, TienePiso());


        //definición de las condiciones para cambiar de estado
        Func<bool> NoTienePiso() => () => !Physics.Raycast(checkPiso, Mathf.Infinity, layerPlataforma);
        Func<bool> TienePiso() => () => Physics.Raycast(checkPiso, Mathf.Infinity, layerPlataforma);
    }

    private void Update() => maquinaDeEstado.Tick();
}
