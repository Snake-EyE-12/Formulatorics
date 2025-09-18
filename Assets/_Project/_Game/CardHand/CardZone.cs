using System.Collections.Generic;
using Cobra.Utilities;
using UnityEngine;

public class CardZone : MonoBehaviour
{
    [SerializeField] private Curve heightCurve;
    [SerializeField] private Curve rotationCurve;

    private List<CardSlotAtm> cardSlots = new();

    public void Add(CardSlotAtm cardSlotAtm)
    {
        cardSlots.Add(cardSlotAtm);
        cardSlotAtm.transform.SetParent(this.transform);
        OnSlotsUpdated();
    }

    private void OnSlotsUpdated()
    {
        float divisor = (cardSlots.Count - 1);
        for (int i = 0; i < cardSlots.Count; i++)
        {
            cardSlots[i].SetOrientation(new Orientation()
            {
                angle = rotationCurve.Evaluate(i / divisor), 
                height = heightCurve.Evaluate(i / divisor)
            });
        }
    }
}