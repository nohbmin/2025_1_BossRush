using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public MovePoint up;
    public MovePoint down;
    public MovePoint left;
    public MovePoint right;

    // 시각화용
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (up) Gizmos.DrawLine(transform.position, up.transform.position);
        if (down) Gizmos.DrawLine(transform.position, down.transform.position);
        if (left) Gizmos.DrawLine(transform.position, left.transform.position);
        if (right) Gizmos.DrawLine(transform.position, right.transform.position);
    }
}
