using UnityEngine;

public class FollowStrategyRotationNone : MonoBehaviour, FollowRotationalStrategy
{
    
    public Quaternion CalculateRotation(Quaternion current, Quaternion target)
    {
        return current;
    }
}