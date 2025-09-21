using UnityEngine;

public class FollowStrategyTranslationSmoothDamp : MonoBehaviour, FollowTranslationalStrategy
{
    [field: SerializeField] private float smoothing = 0.3f;
    private Vector3 refVec;
    public Vector3 CalculatePosition(Vector3 current, Vector3 target)
    {
        return Vector3.SmoothDamp(current, target, ref refVec, smoothing);
    }
}