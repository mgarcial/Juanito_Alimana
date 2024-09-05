using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuscarJugador : IEstado
{
    private Enemigo enemigo;
    private ReferenciasEnemigo referenciasEnemigo;
    private Transform centroDelMundo;
    private playerController jugador;

    public BuscarJugador(Enemigo _enemigo, ReferenciasEnemigo _referenciasEnemigo)
    {
        enemigo = _enemigo;
        referenciasEnemigo = _referenciasEnemigo;
        centroDelMundo = _referenciasEnemigo.centroDelMundo;
        jugador = _referenciasEnemigo._jugador;
    }
    void IEstado.Iniciar()
    {
        throw new System.NotImplementedException();
    }

    void IEstado.Terminar()
    {
        throw new System.NotImplementedException();
    }

    void IEstado.Tick()
    {
        throw new System.NotImplementedException();
    }
}
