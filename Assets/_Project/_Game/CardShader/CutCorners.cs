using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class ShaderSizeSetter : MonoBehaviour
{
    private RectTransform rectTransform;
    private Image image;
    private Material runtimeMaterial;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        BuildShader();
        Canvas.ForceUpdateCanvases();
    }
#if UNITY_EDITOR
    [Button]
    public void Apply()
    {
        Awake();
        Resize();
    }

    private void OnValidate()
    {
        Resize();
    }
#endif

    private void Update()
    {
        Resize();
    }

    private void Resize()
    {
        Vector2 size = rectTransform.rect.size;
        runtimeMaterial.SetVector("_RectSize", size);
    }

    private void BuildShader()
    {
        runtimeMaterial = new Material(image.material);
        image.material = runtimeMaterial;
    }
}