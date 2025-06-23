using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    public float moveDuration = 0.5f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Input Buffer")]
    public float inputBufferTime = 0.2f;

    [Header("Warp Settings")]
    public LayerMask warpLayer;

    private MovePoint currentPoint;
    private bool isMoving = false;
    private Coroutine moveCoroutine = null;

    private Vector2 bufferedInput = Vector2.zero;
    private float bufferTimer = 0f;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 input = context.ReadValue<Vector2>().normalized;

        // �̵� ���̸� ������ �õ��ϰ� �ȵǸ� ���ۿ� ����
        if (isMoving)
        {
            if (!TryWarp(input))
            {
                bufferedInput = input;
                bufferTimer = inputBufferTime;
            }
        }
        else
        {
            // ���� �켱 �� ������ �Ϲ� �̵�
            if (!TryWarp(input))
            {
                TryMove(input);
            }
        }
    }

    private void Start()
    {
        currentPoint = FindClosestMovePoint();
        if (currentPoint != null)
        {
            transform.position = currentPoint.transform.position;
        }
        else
        {
            Debug.LogError("No MovePoint found near the player.");
        }
    }

    private void Update()
    {
        if (!isMoving && bufferedInput != Vector2.zero)
        {
            bufferTimer -= Time.deltaTime;
            if (bufferTimer <= 0f)
            {
                bufferedInput = Vector2.zero;
            }
            else
            {
                if (!TryWarp(bufferedInput))
                {
                    TryMove(bufferedInput);
                }
                bufferedInput = Vector2.zero;
            }
        }
    }

    private void TryMove(Vector2 input)
    {
        if (IsMovable(input, out MovePoint nextPoint))
        {
            moveCoroutine = StartCoroutine(MoveToPoint(nextPoint));
        }
    }

    private bool IsMovable(Vector2 input, out MovePoint nextPoint)
    {
        nextPoint = currentPoint.GetNeighbor(input);
        return nextPoint != null;
    }

    private IEnumerator MoveToPoint(MovePoint targetPoint)
    {
        isMoving = true;

        Vector3 start = transform.position;
        Vector3 end = targetPoint.transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            float easedT = moveCurve.Evaluate(t);
            transform.position = Vector3.Lerp(start, end, easedT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        currentPoint = targetPoint;
        isMoving = false;
        moveCoroutine = null;
    }

    private bool TryWarp(Vector2 input)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 1f, warpLayer);
        foreach (var hit in hits)
        {
            WarpPoint warpPoint = hit.GetComponent<WarpPoint>();
            if (warpPoint != null && warpPoint.IsValidDirection(input))
            {
                MovePoint targetPoint = warpPoint.GetTargetPoint();
                if (targetPoint != null)
                {
                    if (moveCoroutine != null)
                    {
                        StopCoroutine(moveCoroutine);
                        moveCoroutine = null;
                    }

                    transform.position = targetPoint.transform.position;
                    currentPoint = targetPoint;
                    isMoving = false;
                    return true;
                }
            }
        }
        return false;
    }

    private MovePoint FindClosestMovePoint()
    {
        MovePoint[] allPoints = FindObjectsByType<MovePoint>(FindObjectsSortMode.None); // 결과 항상 정렬 필요 x (수정본)
        MovePoint closest = null;
        float minDist = Mathf.Infinity;

        foreach (var point in allPoints)
        {
            float dist = Vector3.Distance(transform.position, point.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = point;
            }
        }

        return closest;
    }
}
