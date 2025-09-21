using UnityEngine;

public class FollowStrategyTranslationInstant : MonoBehaviour, FollowTranslationalStrategy
{
    public Vector3 CalculatePosition(Vector3 current, Vector3 target)
    {
        return target;
    }
}