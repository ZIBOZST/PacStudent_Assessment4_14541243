using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 获取当前移动输入
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 选择播放的动画方向
        if (moveX == 0 && moveY == 0)
        {
            // 不移动：保持当前动画最后一帧
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;

            if (Mathf.Abs(moveX) > Mathf.Abs(moveY))
            {
                if (moveX > 0)
                    animator.Play("Right");
                else
                    animator.Play("Left");
            }
            else
            {
                if (moveY > 0)
                    animator.Play("Back");
                else
                    animator.Play("Front");
            }
        }
    }
}
