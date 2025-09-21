using UnityEngine;

public class FollowStrategyTranslationNone : MonoBehaviour, FollowTranslationalStrategy
{
    public Vector3 CalculatePosition(Vector3 current, Vector3 target)
    {
        return current;
    }
}