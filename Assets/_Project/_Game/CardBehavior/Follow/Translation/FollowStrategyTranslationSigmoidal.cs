using UnityEngine;

public class FollowStrategyTranslationSigmoidal : MonoBehaviour, FollowTranslationalStrategy
{
    [field:SerializeField] public float speed { get; set; }
    private float tolerance = 0.001f;
    public Vector3 CalculatePosition(Vector3 current, Vector3 target)
    {
        Vector3 delta = target - current;
        float distance = delta.magnitude;

        if (distance < tolerance) return target;

        float t = Mathf.Clamp01(speed * Time.deltaTime);

        t = t * t * (3f - 2f * t);

        return Vector3.Lerp(current, target, t);
    }
}