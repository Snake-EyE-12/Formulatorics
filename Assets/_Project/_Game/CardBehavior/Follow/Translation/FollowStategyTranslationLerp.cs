using UnityEngine;

public class FollowStrategyLerp : MonoBehaviour, FollowTranslationalStrategy
{
    [field:SerializeField] public float speed { get; set; }
    public Vector3 CalculatePosition(Vector3 current, Vector3 target)
    {
        float t = 1f - Mathf.Exp(-speed * Time.deltaTime);
        return Vector3.Lerp(current, target, t);
    }
}