using UnityEngine;
using UnityEngine.UI;

public class ImageColorRandomizer : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Image>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
}