using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 玩家移动速度
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // 防止碰撞时旋转
    }

    void Update()
    {
        // 获取输入（上下左右）
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize(); // 防止斜方向太快
    }

    void FixedUpdate()
    {
        // 计算目标位置
        Vector2 targetPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        // 使用 MoveTowards 确保不会超出物理检测范围
        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime));
    }

    // 用于测试是否检测到碰撞
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("碰到: " + collision.gameObject.name);
    }
}
