using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentAnimator : MonoBehaviour
{
    private Animator animator;
    private PacStudentController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PacStudentController>();
    }

    void Update()
    {
        if (controller == null || animator == null) return;

        Vector2 dir = controller.GetCurrentDirection();

        // ✅ 如果还不能控制（复活静止状态），动画停在最后一帧
        if (!controller.CanControl())
        {
            animator.speed = 0;
            return;
        }

        // ✅ 没移动就停动画
        if (dir == Vector2.zero)
        {
            animator.speed = 0;
            return;
        }

        animator.speed = 1;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
                animator.Play("Right");
            else
                animator.Play("Left");
        }
        else
        {
            if (dir.y > 0)
                animator.Play("Back");
            else
                animator.Play("Front");
        }
    }
}
