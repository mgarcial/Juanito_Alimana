using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class Maquina_Estado
{
    private IEstado estadoActual;

    private Dictionary<Type, List<Trancision>> trancisiones = new Dictionary<Type, List<Trancision>>();
    private List<Trancision> trancisionesActuales = new List<Trancision>();
    private List<Trancision> cualquierTrancision = new List<Trancision>();

    private static List<Trancision> trancisionesVacias = new List<Trancision>(0);

    public void Tick()
    {
        var trancision = RecibirTrancision();
        if (trancision != null)
            SetearEstado(trancision.A);

        estadoActual?.Tick();
    }

    private void SetearEstado(IEstado estado)
    {
        if (estado == estadoActual)
            return;

        estadoActual?.Terminar();
        estadoActual = estado;

        trancisiones.TryGetValue(estadoActual.GetType(), out trancisionesActuales);
        if (trancisionesActuales == null)
            trancisionesActuales = trancisionesVacias;

        estadoActual?.Iniciar();
    }

    public void AgregarTrancision(IEstado desde, IEstado a, Func<bool> condicion)
    {
        if(trancisiones.TryGetValue(desde.GetType(), out var _transiciones) == false)
        {
            _transiciones = new List<Trancision>();
            trancisiones[desde.GetType()] = _transiciones;
        }

        _transiciones.Add(new Trancision(a, condicion));
    }

    public void AgregarCualquierTrancision(IEstado estado, Func<bool> condicion)
    {
        cualquierTrancision.Add(new Trancision(estado, condicion));
    }

    private Trancision RecibirTrancision()
    {
        foreach (var transicion in cualquierTrancision)
            if (transicion.condicion())
                return transicion;

        foreach (var trancision in trancisionesActuales)
            if (trancision.condicion())
                return trancision;

        return null;
    }

    private class Trancision
    {
        public Func<bool> condicion { get; }
        public IEstado A { get; }

        public Trancision(IEstado a, Func<bool> _condicion)
        {
            A = a;
            condicion = _condicion;
        }
    }
}
