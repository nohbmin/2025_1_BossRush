using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public MovePoint up;
    public MovePoint down;
    public MovePoint left;
    public MovePoint right;

    /// <summary>
    /// 입력 방향에 따라 연결된 MovePoint를 반환합니다.
    /// </summary>
    /// <param name="dir">Vector2(Up/Down/Left/Right 중 하나)</param>
    /// <returns>연결된 MovePoint 또는 null</returns>
    public MovePoint GetNeighbor(Vector2 dir)
    {
        if (dir == Vector2.up) return up;
        if (dir == Vector2.down) return down;
        if (dir == Vector2.left) return left;
        if (dir == Vector2.right) return right;

        return null;
    }

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
