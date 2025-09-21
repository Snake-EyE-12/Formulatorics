using UnityEngine;


public class FollowStrategyRotationLerp : MonoBehaviour, FollowRotationalStrategy
{
    [field:SerializeField] public float speed { get; set; }
    
    public Quaternion CalculateRotation(Quaternion current, Quaternion target)
    {
        float t = 1f - Mathf.Exp(-speed * Time.deltaTime);
        return Quaternion.Lerp(current, target, t);
    }
}


public interface FollowRotationalStrategy
{
    public Quaternion CalculateRotation(Quaternion current, Quaternion target);
}
