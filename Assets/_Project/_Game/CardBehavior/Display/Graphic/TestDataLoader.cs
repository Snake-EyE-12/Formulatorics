using System;
using NaughtyAttributes;
using UnityEngine;

public class TestDataLoader : MonoBehaviour
{
    [SerializeField] private CardHandler handler;

    [Button]
    public void Load()
    {
        handler.LoadCardData(card);
    }

    [SerializeField] private CardData card;
}

[Serializable]
public class CardData : ICardData
{
    [SerializeField] private string symbol;
    [SerializeField] private Sprite background;
    public string Symbol() => symbol;

    public Sprite BackgroundImage() => background;
}