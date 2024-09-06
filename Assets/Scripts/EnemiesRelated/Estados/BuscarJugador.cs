using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuscarJugador : IEstado
{
    private Enemigo enemigo;
    private ReferenciasEnemigo referenciasEnemigo;
    private Transform jugador;

    public BuscarJugador(Enemigo _enemigo, ReferenciasEnemigo _referenciasEnemigo)
    {
        enemigo = _enemigo;
        referenciasEnemigo = _referenciasEnemigo;
        jugador = _referenciasEnemigo._jugador;
    }
    void IEstado.Iniciar()
    {
    }

    void IEstado.Terminar()
    {
    }

    void IEstado.Tick()
    {
        var direccionMov = jugador.transform.position - referenciasEnemigo._transform.position;
        referenciasEnemigo._rigidbody.AddForce(direccionMov.normalized*enemigo.velocidadHorizontal, ForceMode.VelocityChange);
    }
}
