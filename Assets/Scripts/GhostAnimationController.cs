using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimationController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GhostMovement ghostMovement;

    private Vector2 lastMoveDir = Vector2.down; // 初始朝下

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ghostMovement = GetComponent<GhostMovement>();
    }

    void Update()
    {
        // 从 GhostMovement 获取方向（记得 GhostMovement 里要加个 GetMoveDirection()）
        Vector2 dir = ghostMovement.GetMoveDirection();

        // ✅ 防止静止时乱切动画
        if (dir.magnitude > 0.05f)
        {
            lastMoveDir = dir.normalized;
        }

        // ✅ 更新 Animator 参数
        animator.SetFloat("MoveX", lastMoveDir.x);
        animator.SetFloat("MoveY", lastMoveDir.y);
    }
}
