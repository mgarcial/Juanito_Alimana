using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    private Maquina_Estado maquinaDeEstado;
    private ReferenciasEnemigo misReferencias;

    //Generales
    public int saltos = 2;
    public float tiempoEntreSaltos;
    public float maximaVelMovimiento;
    public Vector3 direccionSalto;
    public float velocidadHorizontal;

    //Cosas del arma
    public Transform LugarDelArma;
    public Gun ArmaInicial;
    private bool Equipado = true;
    //private Gun armaActual;

    //Parametros del estado buscar piso
    [SerializeField] private LayerMask layerPlataforma;

    //Parametros del estado buscar al jugador

    private void Awake()
    {
        //Setup 
        misReferencias = GetComponent<ReferenciasEnemigo>();
        maquinaDeEstado = new Maquina_Estado();
        //armaActual = ArmaInicial;
        //Instantiate(ArmaInicial, LugarDelArma.position, LugarDelArma.rotation, LugarDelArma);

        //definición de los estados que se usarán
        var buscarPiso = new BuscarPiso(this, misReferencias);
        var buscarJugador = new BuscarJugador(this, misReferencias);

        //Trancisiones de estados
        maquinaDeEstado.AgregarCualquierTrancision(buscarPiso, NoTienePiso());

        maquinaDeEstado.AgregarTrancision(buscarPiso, buscarJugador, TienePiso());


        //definición de las condiciones para cambiar de estado
        Func<bool> NoTienePiso() => () => !Physics.Raycast(misReferencias._transform.position, -misReferencias._transform.up, Mathf.Infinity, layerPlataforma);
        Func<bool> TienePiso() => () => Physics.Raycast(misReferencias._transform.position, -misReferencias._transform.up, Mathf.Infinity, layerPlataforma);
    }

    private void Update()
    {
        maquinaDeEstado.Tick();
    }

    /*public void PickUpGun(Gun arma)
    {
        if (Equipado)
        {
            Destroy(armaActual.gameObject);
            Debug.Log($"Destroying {armaActual}");
        }

        armaActual = Instantiate(arma, LugarDelArma.position, LugarDelArma.rotation, LugarDelArma);

        armaActual.firePoint = armaActual.transform.Find("FirePoint");
        armaActual.SetGunHolder(this);
        Equipado = true;
        Debug.Log("Picked up gun: " + armaActual);
    }

    public bool IsFacingRight()
    {
        throw new NotImplementedException();
    }*/
}
