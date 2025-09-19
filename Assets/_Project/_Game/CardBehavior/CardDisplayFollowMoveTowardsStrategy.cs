using UnityEngine;

public class CardFollowMoveTowardsStrategy : MonoBehaviour, ICardFollowStrategy
{
    public Vector3 Position(Vector3 current, Vector3 target, float speed)
    {
        return Vector3.MoveTowards(current, target, speed);
    }
}


public interface ICardFollowStrategy
{
    public Vector3 Position(Vector3 current, Vector3 target, float speed);
}