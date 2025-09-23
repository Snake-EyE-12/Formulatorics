using UnityEngine;


public class FollowStrategyRotationLerp : MonoBehaviour, FollowRotationalStrategy
{
    [field:SerializeField] public float speed { get; set; }
    
    public Quaternion CalculateRotation(Quaternion current, Quaternion target)
    {
        return Quaternion.Lerp(current, target, Utils.LerpT(speed));
    }
}


public interface FollowRotationalStrategy
{
    public Quaternion CalculateRotation(Quaternion current, Quaternion target);
}
