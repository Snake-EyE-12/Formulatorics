using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickAnywhereInputListener : MonoBehaviour
{

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ServiceLocator.Get<IGroupInteractionControl>().PressDown();
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ServiceLocator.Get<IGroupInteractionControl>().ReleaseDown();
        }
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            ServiceLocator.Get<IGroupInteractionControl>().PerformQuickDeselect();
        }
    }
}