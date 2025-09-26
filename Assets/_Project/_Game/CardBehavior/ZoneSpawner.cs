using Cobra.DesignPattern;
using UnityEngine;

public class ZoneSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool<BonusZone> zonePool;
    [SerializeField] private Transform parent;

    public void SpawnZone()
    {
        zonePool.Summon();
    }

    private void GetZoneSpawnLocation()
    {
        
    }
}

public class BonusZone : MonoBehaviour, Poolable<BonusZone>
{
    public ObjectPool<BonusZone> Pool { get; set; }
    public void OnSummon()
    {
        
    }

    public void OnRelease()
    {
        
    }
}