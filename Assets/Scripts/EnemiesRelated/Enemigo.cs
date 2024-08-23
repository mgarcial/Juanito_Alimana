using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    private Maquina_Estado maquinaDeEstado;
    private ReferenciasEnemigo misReferencias;

    private void Awake()
    {
        //Setup 
        misReferencias = new ReferenciasEnemigo();
        maquinaDeEstado = new Maquina_Estado();

        //definici�n de los estados que se usar�n

        //definici�n de las condiciones para cambiar de estado
    }

    private void Update() => maquinaDeEstado.Tick();
}
