using UnityEngine;

public class FollowStrategyMoveTowards : MonoBehaviour, FollowTranslationalStrategy
{
    [field:SerializeField] public float speed { get; set; }
    public Vector3 CalculatePosition(Vector3 current, Vector3 target)
    {
        return Vector3.MoveTowards(current, target, speed * Time.deltaTime);
    }
}


public interface FollowTranslationalStrategy
{
    public Vector3 CalculatePosition(Vector3 current, Vector3 target);
}
