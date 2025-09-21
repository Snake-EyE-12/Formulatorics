using System;
using System.Collections.Generic;
using Cobra.DesignPattern;
using UnityEngine;

public class ZoneContainer : MonoBehaviour, IZoneControl
{
    private void Awake()
    {
        ServiceLocator.Register<IZoneControl>(this);
    }

    public void Register(IZone zone)
    {
        zones.Add(zone);
    }

    public void Deregister(IZone zone)
    {
        zones.Remove(zone);
    }

    public List<IZone> zones = new();
    public List<IZone> Zones => zones;

}

public interface IZoneControl : IRegistrar<IZone>, IService
{
    public List<IZone> Zones { get; }
}

public interface IRegistrar<T>
{
    public void Register(T candidate);
    public void Deregister(T candidate);
}