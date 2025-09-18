using NaughtyAttributes;
using UnityEngine;

public class CardGameManagerTester : MonoBehaviour
{
    private CardSelectionController controller;
    [SerializeField] private CardController cardControllerPrefab;

    [SerializeField] private Transform cardParent;
    [Button]
    public void SpawnCard()
    {
        new CardBuilder(cardControllerPrefab)
            .WithParent(cardParent)
            .Build();
    }
}