using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveDuration = 0.5f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float inputBufferTime = 0.2f;

    private MovePoint currentPoint;
    private bool isMoving = false;

    private Vector2 inputDirection = Vector2.zero;
    private Vector2 bufferedInput = Vector2.zero;
    private float bufferTimer = 0f;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 dir = context.ReadValue<Vector2>();
            if (!isMoving)
                TryMove(dir);
            else
            {
                bufferedInput = dir;
                bufferTimer = inputBufferTime;
            }
        }
    }

    void Start()
    {
        currentPoint = FindClosestMovePoint();
        if (currentPoint != null)
        {
            transform.position = currentPoint.transform.position;
        }
        else
        {
            Debug.LogError("No MovePoint found nearby.");
        }
    }

    void Update()
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
                TryMove(bufferedInput);
                bufferedInput = Vector2.zero;
            }
        }
    }

    void TryMove(Vector2 direction)
    {
        direction = direction.normalized;
        MovePoint target = null;

        if (direction.y > 0.5f) target = currentPoint.up;
        else if (direction.y < -0.5f) target = currentPoint.down;
        else if (direction.x < -0.5f) target = currentPoint.left;
        else if (direction.x > 0.5f) target = currentPoint.right;

        if (target != null)
        {
            StartCoroutine(MoveToPoint(target));
        }
        else
        {
            StartCoroutine(Shake());
        }
    }

    System.Collections.IEnumerator MoveToPoint(MovePoint targetPoint)
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
    }

    System.Collections.IEnumerator Shake()
    {
        isMoving = true;
        Vector3 originalPos = transform.position;
        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float strength = 0.05f;
            transform.position = originalPos + Random.insideUnitSphere * strength;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
        isMoving = false;
    }

    MovePoint FindClosestMovePoint()
    {
        MovePoint[] points = FindObjectsOfType<MovePoint>();
        MovePoint closest = null;
        float minDist = Mathf.Infinity;

        foreach (var point in points)
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
