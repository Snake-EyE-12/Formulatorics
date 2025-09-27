using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class ShaderSizeSetter : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;
    private Material runtimeMaterial;

    private void Awake()
    {
        BuildShader();
    }

    private void Update()
    {
        Resize();
    }

    private void Resize()
    {
        if (rectTransform == null || rectTransform.rect == null) return;
        Vector2 size = rectTransform.rect.size;
        runtimeMaterial.SetVector("_RectSize", size);
    }

    private void BuildShader()
    {
        runtimeMaterial = new Material(image.material);
        image.material = runtimeMaterial;
    }
}