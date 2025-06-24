using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    [SerializeField] private Vector2 validDirection; // Vector2.up/down/left/right
    [SerializeField] private MovePoint targetPoint;
    public void EnableWarp() => gameObject.SetActive(true);
    public void DisableWarp() => gameObject.SetActive(false);

    public bool IsValidDirection(Vector2 input)
    {
        if (validDirection == input)
        {

            return true;
        }
        return false;
    }

    public MovePoint GetTargetPoint()
    {
        return targetPoint;
    }
}
