using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuscarPiso : IEstado
{
    private Enemigo enemigo;
    private ReferenciasEnemigo referenciasEnemigo;
    private Transform centroDelMundo;

    private float timerSaltos;

    public BuscarPiso(Enemigo _enemigo, ReferenciasEnemigo _misReferencias)
    {
        enemigo = _enemigo;
        referenciasEnemigo = _misReferencias;
        centroDelMundo = _misReferencias.centroDelMundo;
    }

    public void Iniciar()
    {
        timerSaltos = enemigo.tiempoEntreSaltos;
        referenciasEnemigo._rigidbody.maxLinearVelocity = enemigo.maximaVelMovimiento;
    }

    public void Terminar()
    {
        throw new System.NotImplementedException();
    }

    public void Tick()
    {
        timerSaltos -= Time.deltaTime;

        if (referenciasEnemigo._transform.position.x - centroDelMundo.position.x < 0)
        {
            referenciasEnemigo._rigidbody.AddForce(new Vector3(enemigo.velocidadBuscarPiso, 0, 0));
        }
        else
        {
            referenciasEnemigo._rigidbody.AddForce(new Vector3(-enemigo.velocidadBuscarPiso, 0, 0));
        }

        if(enemigo.saltos > 0 && timerSaltos<0f)
        {
            timerSaltos = enemigo.tiempoEntreSaltos;

            if (referenciasEnemigo._transform.position.x - centroDelMundo.position.x < 0)
            {
                referenciasEnemigo._rigidbody.velocity = Vector3.zero;
                referenciasEnemigo._rigidbody.angularVelocity = Vector3.zero;
                referenciasEnemigo._rigidbody.AddForce(enemigo.direccionSalto, ForceMode.Impulse);
                enemigo.saltos--;
            }
            else
            {
                referenciasEnemigo._rigidbody.velocity = Vector3.zero;
                referenciasEnemigo._rigidbody.angularVelocity = Vector3.zero;
                referenciasEnemigo._rigidbody.AddForce(new Vector3(-enemigo.direccionSalto.x, enemigo.direccionSalto.y, enemigo.direccionSalto.z), ForceMode.Impulse);
                enemigo.saltos--;
            }
        }
    }
}
