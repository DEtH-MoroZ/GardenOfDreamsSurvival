using UnityEngine;

public class BipedLocomotion2D : MonoBehaviour
{
    [Header("Foot Placement Settings")]
    public Transform targetTransform; // The body/hip transform to follow
    public Transform leftLegIk;
    public Transform rightLegIk;
    public Transform rigRoot;
    public float strideLength = 1f; // Distance between steps
    public float stepHeight = 0.5f; // How high feet lift during step
    public float footSpacing = 0.3f; // Sideways distance from center
    public float stepDuration = 0.5f; // Time to complete one step
    public float stepVelocityDecrease = 10f;
    public LayerMask groundLayer = 1; // Layer mask for ground detection

    [Header("Debug Visualization")]
    public bool showDebugGizmos = true;
    public Color leftFootColor = Color.red;
    public Color rightFootColor = Color.blue;

    // Current foot positions
    private Vector3 leftFootPosition;
    private Vector3 rightFootPosition;
    private Vector3 lastTargetPosition;

    // Step timing
    private float stepTimer;
    private bool isLeftFootLeading = true;

    private Vector3 startLeft;
    private Vector3 startRight;
    private Transform _leftLegIK;
    private Transform _rightLegIK;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        startRight = leftLegIk.localPosition;
        startLeft = rightLegIk.localPosition;
        _rightLegIK = rightLegIk;
        _leftLegIK = leftLegIk;
    }

    void Start()
    {
        if (targetTransform == null)
            targetTransform = transform;

        InitializeFootPositions();
    }

    void FixedUpdate()
    {
        CalculateFootPlacement();

        if (_rb.velocity.magnitude > 0.1f) 
        {
            _leftLegIK.position = leftFootPosition;
            _rightLegIK.transform.position = rightFootPosition;
        }
        else
        {
            _leftLegIK.localPosition = startLeft;
            _rightLegIK.localPosition = startRight;
        }

        float movementDirection = Mathf.Sign(_rb.velocity.x);

        Vector3 currentRotation = rigRoot.eulerAngles;

        if (movementDirection > 0)
        {
            _leftLegIK = rightLegIk;
            _rightLegIK = leftLegIk;
            currentRotation.y = 0f;
        }
        else if (movementDirection < 0)
        {
            _rightLegIK = rightLegIk;
            _leftLegIK = leftLegIk;
            currentRotation.y = 180f;
        }

        rigRoot.eulerAngles = currentRotation;
    }

    void InitializeFootPositions()
    {
        Vector3 startPos = targetTransform.position;
        leftFootPosition = startPos + targetTransform.right * -footSpacing;
        rightFootPosition = startPos + targetTransform.right * footSpacing;
        lastTargetPosition = startPos;
    }

    void CalculateFootPlacement()
    {
        if (targetTransform == null) return;

        Vector3 currentTargetPos = targetTransform.position;
        Vector3 movement = currentTargetPos - lastTargetPosition;

        // Update step timer
        stepTimer += Time.fixedDeltaTime;
        float stepProgress = Mathf.Clamp01(stepTimer / stepDuration);

        // Check if it's time to take a new step
        if (stepProgress >= 1f || movement.magnitude > strideLength * 0.5f)
        {
            TakeStep();
            stepTimer = 0f;
        }
        else
        {
            // Smoothly move feet during step
            UpdateFootMovement(stepProgress);
        }

        lastTargetPosition = currentTargetPos;
    }

    void TakeStep()
    {
        Vector3 targetPos = targetTransform.position;
        Vector3 forward = targetTransform.forward;

        // Calculate ideal foot positions
        Vector3 idealLeftFootPos = targetPos +
            forward * strideLength * 0.5f +
            targetTransform.right * (-footSpacing) + (Vector3)_rb.velocity/ stepVelocityDecrease;

        Vector3 idealRightFootPos = targetPos +
            forward * strideLength * 0.5f +
            targetTransform.right * (footSpacing) + (Vector3)_rb.velocity/ stepVelocityDecrease;

        // Raycast to find actual ground positions
        idealLeftFootPos = GetGroundPosition(idealLeftFootPos);
        idealRightFootPos = GetGroundPosition(idealRightFootPos);

        // Alternate which foot moves
        if (isLeftFootLeading)
        {
            leftFootPosition = idealLeftFootPos;
        }
        else
        {
            rightFootPosition = idealRightFootPos;
        }

        isLeftFootLeading = !isLeftFootLeading;
    }

    void UpdateFootMovement(float progress)
    {
        Vector3 targetPos = targetTransform.position;

        // Calculate current foot positions with arc movement
        if (isLeftFootLeading)
        {
            Vector3 idealPos = targetPos + targetTransform.right * -footSpacing;
            leftFootPosition = Vector3.Lerp(leftFootPosition, idealPos, progress);

            // Add step height arc
            float arcHeight = Mathf.Sin(progress * Mathf.PI) * stepHeight;
            leftFootPosition.y += arcHeight;
        }
        else
        {
            Vector3 idealPos = targetPos + targetTransform.right * footSpacing;
            rightFootPosition = Vector3.Lerp(rightFootPosition, idealPos, progress);

            // Add step height arc
            float arcHeight = Mathf.Sin(progress * Mathf.PI) * stepHeight;
            rightFootPosition.y += arcHeight;
        }

        // Keep stationary foot on ground
        if (isLeftFootLeading)
        {
            rightFootPosition = GetGroundPosition(rightFootPosition);
        }
        else
        {
            leftFootPosition = GetGroundPosition(leftFootPosition);
        }
    }

    Vector3 GetGroundPosition(Vector3 position)
    {
        RaycastHit hit;
        Vector3 rayStart = position + Vector3.up * 2f;

        if (Physics.Raycast(rayStart, Vector3.down, out hit, 4f, groundLayer))
        {
            return hit.point;
        }

        return position;
    }

    // Public methods to get foot positions
    public Vector3 GetLeftFootPosition()
    {
        return leftFootPosition;
    }

    public Vector3 GetRightFootPosition()
    {
        return rightFootPosition;
    }

    public float GetStepProgress()
    {
        return Mathf.Clamp01(stepTimer / stepDuration);
    }

    public bool IsLeftFootLeading()
    {
        return isLeftFootLeading;
    }

    // Debug visualization
    void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;

        // Draw left foot
        Gizmos.color = leftFootColor;
        Gizmos.DrawSphere(leftFootPosition, 0.1f);
        Gizmos.DrawWireCube(leftFootPosition, new Vector3(0.2f, 0.05f, 0.4f));

        // Draw right foot
        Gizmos.color = rightFootColor;
        Gizmos.DrawSphere(rightFootPosition, 0.1f);
        Gizmos.DrawWireCube(rightFootPosition, new Vector3(0.2f, 0.05f, 0.4f));

        // Draw stride area
        if (targetTransform != null)
        {
            Gizmos.color = Color.white;
            Vector3 center = targetTransform.position + targetTransform.forward * strideLength * 0.5f;
            Gizmos.DrawWireCube(center, new Vector3(footSpacing * 2f, 0.1f, strideLength));
        }
    }
}