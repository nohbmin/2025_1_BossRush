using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public MovePoint up;
    public MovePoint down;
    public MovePoint left;
    public MovePoint right;

    /// <summary>
    /// �Է� ���⿡ ���� ����� MovePoint�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="dir">Vector2(Up/Down/Left/Right �� �ϳ�)</param>
    /// <returns>����� MovePoint �Ǵ� null</returns>
    public MovePoint GetNeighbor(Vector2 dir)
    {
        if (dir == Vector2.up) return up;
        if (dir == Vector2.down) return down;
        if (dir == Vector2.left) return left;
        if (dir == Vector2.right) return right;

        return null;
    }

    // �ð�ȭ��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (up) Gizmos.DrawLine(transform.position, up.transform.position);
        if (down) Gizmos.DrawLine(transform.position, down.transform.position);
        if (left) Gizmos.DrawLine(transform.position, left.transform.position);
        if (right) Gizmos.DrawLine(transform.position, right.transform.position);
    }
}
