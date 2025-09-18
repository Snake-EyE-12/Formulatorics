using UnityEngine;

public class CardSlotAtm : MonoBehaviour
{
    [SerializeField] private Transform offset;

    private Orientation baseOrientation;
    public void SetOrientation(Orientation orientation)
    {
        baseOrientation = orientation;
        OnOffsetUpdated();
    }

    private void OnOffsetUpdated()
    {
        offset.position = transform.position + (baseOrientation.height * Vector3.up);
        offset.rotation = Quaternion.Euler(0, 0, baseOrientation.angle);
    }
}

public struct Orientation
{
    public float angle;
    public float height;
}