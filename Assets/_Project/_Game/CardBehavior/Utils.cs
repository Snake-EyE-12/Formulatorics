using UnityEngine;

public static class Utils
{
    public static float LerpT(float speed)
    {
        return 1f - Mathf.Exp(-speed * Time.deltaTime);
    }
}
