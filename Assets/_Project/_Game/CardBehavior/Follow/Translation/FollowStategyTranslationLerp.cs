using UnityEngine;

public class FollowStrategyLerp : MonoBehaviour, FollowTranslationalStrategy
{
    [field:SerializeField] public float speed { get; set; }
    public Vector3 CalculatePosition(Vector3 current, Vector3 target)
    {
        return Vector3.Lerp(current, target, Utils.LerpT(speed));
    }
}