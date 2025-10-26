using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public LayerMask wallLayer;

    private Vector2 currentDirection = Vector2.zero;
    private Vector2 nextDirection = Vector2.zero;
    private Vector2 targetGridPos;
    private bool isMoving = false;
    private Vector2 spawnPosition;

    private bool canControl = true; // ✅ 控制锁，防止复活时误动

    void Start()
    {
        targetGridPos = RoundToGrid(transform.position);
        transform.position = targetGridPos;
        spawnPosition = targetGridPos;
    }

    void Update()
    {
        if (!canControl) return; // ✅ 暂时禁用输入检测

        // ✅ 获取输入方向
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            input = Vector2.up;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            input = Vector2.down;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            input = Vector2.left;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            input = Vector2.right;

        if (input != Vector2.zero)
            nextDirection = input;

        // ✅ 平滑移动逻辑
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetGridPos, moveSpeed * Time.deltaTime);

            if ((Vector2)transform.position == targetGridPos)
            {
                isMoving = false;

                if (CanMove(nextDirection))
                    currentDirection = nextDirection;

                if (CanMove(currentDirection))
                    Move(currentDirection);
            }
        }
        else
        {
            if (CanMove(nextDirection))
            {
                currentDirection = nextDirection;
                Move(currentDirection);
            }
        }
    }

    void Move(Vector2 dir)
    {
        targetGridPos = RoundToGrid((Vector2)transform.position + dir);
        isMoving = true;
    }

    bool CanMove(Vector2 dir)
    {
        if (dir == Vector2.zero)
            return false;

        Vector2 origin = (Vector2)transform.position + dir * 0.2f;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, 0.4f, wallLayer);
        Debug.DrawRay(origin, dir * 0.4f, Color.red);
        return hit.collider == null;
    }

    Vector2 RoundToGrid(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    // ✅ 复活静止修正 + 清空输入缓冲
    public void Respawn()
    {
        transform.position = spawnPosition;
        targetGridPos = spawnPosition;

        // 🧹 清除所有移动状态
        currentDirection = Vector2.zero;
        nextDirection = Vector2.zero;
        isMoving = false;

        StartCoroutine(ClearInputBuffer());
    }

    private IEnumerator ClearInputBuffer()
    {
        canControl = false;

        // ✅ 等待 1 帧 + 0.1 秒，确保 Unity 清空输入状态
        yield return null;
        yield return new WaitForSeconds(0.1f);

        canControl = true;
    }

    // ✅ 给动画系统访问当前移动方向
    public Vector2 GetCurrentDirection()
    {
        return currentDirection;
    }

    // ✅ 给动画系统判断是否可控制
    public bool CanControl()
    {
        return canControl;
    }
}
