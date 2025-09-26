using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDataDisplay : MonoBehaviour, ICardDataDisplay
{
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text topSymbol;
    [SerializeField] private TMP_Text botSymbol;
    public void Display(ICardData data)
    {
        background.sprite = data.BackgroundImage();
        topSymbol.text = data.Symbol();
        botSymbol.text = data.Symbol();
    }
}

public interface ICardDataDisplay
{
    public void Display(ICardData data);
}

public interface ICardData
{
    public string Symbol();
    public Sprite BackgroundImage();
}