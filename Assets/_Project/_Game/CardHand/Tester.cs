using NaughtyAttributes;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private CardZone zone;

    [SerializeField] private Transform c;
    [Button]
    public void AddSlot()
    {
        GameObject e = Instantiate(cardAllPrefab, c);
        CardSlotAtm slotAtm = e.GetComponentInChildren<CardSlotAtm>();
        zone.Add(slotAtm);
    }
    
    [SerializeField] private GameObject cardAllPrefab;
}