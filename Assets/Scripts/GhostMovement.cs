using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2.5f;
    public float wallCheckDistance = 0.35f;
    public float intersectionCheckDistance = 0.55f;
    [Range(0f, 1f)] public float turnChance = 0.3f;

    private Rigidbody2D rb;
    private Animator animator; 
    private Vector2 moveDirection;
    private Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
        PickNewDirection();
    }

    void FixedUpdate()
    {
        // ✅ 前进或转向
        if (!IsWallAhead())
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            PickNewDirection();
        }

        // ✅ 路口随机转弯
        if (IsAtIntersection() && Random.value < turnChance)
        {
            PickNewDirection();
        }

        UpdateAnimation(); 
    }

    bool IsWallAhead()
    {
        Vector2 checkPos = rb.position + moveDirection * 0.05f;
        return Physics2D.Raycast(checkPos, moveDirection, wallCheckDistance, LayerMask.GetMask("Wall"));
    }

    bool IsAtIntersection()
    {
        Vector2 left = new Vector2(-moveDirection.y, moveDirection.x);
        Vector2 right = new Vector2(moveDirection.y, -moveDirection.x);

        bool canTurnLeft = !Physics2D.Raycast(transform.position, left, intersectionCheckDistance, LayerMask.GetMask("Wall"));
        bool canTurnRight = !Physics2D.Raycast(transform.position, right, intersectionCheckDistance, LayerMask.GetMask("Wall"));

        return canTurnLeft || canTurnRight;
    }

    void PickNewDirection()
    {
        Vector2 oldDir = moveDirection;
        List<Vector2> validDirs = new List<Vector2>();

        foreach (Vector2 dir in directions)
        {
            if (dir == -oldDir) continue;

            Vector2 checkPos = rb.position + dir * 0.05f;
            if (!Physics2D.Raycast(checkPos, dir, wallCheckDistance, LayerMask.GetMask("Wall")))
            {
                validDirs.Add(dir);
            }
        }

        if (validDirs.Count > 0)
        {
            moveDirection = validDirs[Random.Range(0, validDirs.Count)];
        }
        else
        {
            moveDirection = -oldDir; // 实在没路就掉头
        }
    }

    // ✅ 动画控制
    void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);
    }

    // ✅ 调试可视化射线
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)moveDirection * wallCheckDistance);

        Gizmos.color = Color.yellow;
        Vector2 left = new Vector2(-moveDirection.y, moveDirection.x);
        Vector2 right = new Vector2(moveDirection.y, -moveDirection.x);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)left * intersectionCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)right * intersectionCheckDistance);
    }

    // ✅ 提供方向给其他脚本访问
    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }
}
