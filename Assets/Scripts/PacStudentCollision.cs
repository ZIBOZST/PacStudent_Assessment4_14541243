using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentCollision : MonoBehaviour
{
    private Vector3 respawnPosition = new Vector3(-1f, 9.866f, -0.1f);
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;
    private PacStudentController controller; // ✅ 加上对控制器的引用

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<PacStudentController>(); // ✅ 初始化控制器引用
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ghost") && !isInvincible)
        {
            Debug.Log("💀 Player hit by ghost!");

            // ✅ 扣除生命
            if (GameManager.Instance != null)
                GameManager.Instance.LoseLife();

            // ✅ 通知 Controller 复活（这一步是关键）
            if (controller != null)
                controller.Respawn();

            // ✅ 再传送回固定位置
            transform.position = respawnPosition;

            // ✅ 开启无敌闪烁
            StartCoroutine(InvincibilityFlash());
        }
    }

    IEnumerator InvincibilityFlash()
    {
        isInvincible = true;

        float duration = 1f;
        float flashInterval = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}
