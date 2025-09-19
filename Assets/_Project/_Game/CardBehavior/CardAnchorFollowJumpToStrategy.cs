using UnityEngine;

public class CardAnchorFollowJumpToStrategy : MonoBehaviour, ICardFollowStrategy
{
    public Vector3 Position(Vector3 current, Vector3 target, float speed)
    {
        return target;
    }
}