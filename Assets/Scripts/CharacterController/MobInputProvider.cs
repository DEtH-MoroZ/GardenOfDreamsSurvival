using UnityEngine;

public class MobInputProvider : MonoBehaviour, IMovementInputProvider
{
    [Header("AI Settings")]
    public Transform target;
    public float stopDistance = 0.5f;

    public Vector2 GetMoveInput()
    {
        if (target == null) return Vector2.zero;

        Vector2 direction = (target.position - transform.position);
        float distance = direction.magnitude;

        if (distance <= stopDistance) return Vector2.zero;

        return direction.normalized;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}