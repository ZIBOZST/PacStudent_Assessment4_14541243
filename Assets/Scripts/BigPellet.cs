using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPellet : MonoBehaviour
{
    public int points = 50; // 吃大豆子得分
    public float frightenedDuration = 8f; // 幽灵进入害怕状态时间

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ✅ 调用 GameManager 加分
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(points);
            }

            // ✅ 如果你之后要让幽灵进入“害怕”状态，可以在这里调用幽灵脚本
            // 比如：FindFirstObjectByType<GhostController>().EnterFrightenedMode(frightenedDuration);
            // 暂时先留空

            // ✅ 销毁大豆子
            Destroy(gameObject);
        }
    }
}
