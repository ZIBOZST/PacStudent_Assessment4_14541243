using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 用于 UI 显示文字

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // ✅ 让它变成单例，方便全局调用
    public int score = 0;
    public TMP_Text scoreText; // UI 文本对象

    private void Awake()
    {
        // 确保只存在一个 GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogWarning("⚠️ ScoreText 没有绑定到 GameManager 上！");
        }
    }
}
