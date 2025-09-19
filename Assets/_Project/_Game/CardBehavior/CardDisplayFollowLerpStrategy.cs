using UnityEngine;

public class CardFollowLerpStrategy : MonoBehaviour, ICardFollowStrategy
{
    public Vector3 Position(Vector3 current, Vector3 target, float speed)
    {
        return Vector3.Lerp(current, target, speed);
    }
}