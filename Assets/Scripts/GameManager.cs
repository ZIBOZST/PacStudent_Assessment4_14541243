using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // ✅ 全局单例

    [Header("Game Data")]
    public int score = 0;
    public int lives = 3;
    public int level = 1;

    [Header("UI Elements")]
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text levelText;
    public TMP_Text timerText;

    [Header("Lives UI")]
    public Transform livesPanel;          // ❤️ 生命图标父节点
    public GameObject lifeIconPrefab;     // ❤️ 生命图标 prefab

    private List<GameObject> lifeIcons = new List<GameObject>();

    private float elapsedTime = 0f;
    private bool isGameOver = false;
    private bool isPaused = false;

    private void Awake()
    {
        // ✅ 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切换场景不销毁
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        ResetGame();
        UpdateHUD();
    }

    private void Update()
    {
        if (isGameOver || isPaused) return;

        elapsedTime += Time.deltaTime;
        UpdateTimerUI();
    }

    // ✅ 初始化 / 重置数据
    public void ResetGame()
    {
        score = 0;
        lives = 3;
        level = 1;
        elapsedTime = 0f;
        isGameOver = false;
        isPaused = false;

        UpdateHUD();
    }

    // ✅ 加分
    public void AddScore(int amount)
    {
        score += amount;
        UpdateHUD();
    }

    // ✅ 掉命（被幽灵碰到）
    public void LoseLife()
    {
        if (isGameOver) return;

        lives--;
        if (lives < 0) lives = 0;

        UpdateHUD();

        // ❤️ 播放生命消失动画
        StartCoroutine(RemoveLifeIconAnimated());

        if (lives <= 0)
        {
            GameOver();
        }
    }

    // ✅ 进入下一关
    public void NextLevel()
    {
        level++;
        UpdateHUD();

        // 可扩展为多关卡场景
        // SceneManager.LoadScene("Level" + level);
    }

    // ✅ 更新分数 / 生命 / 关卡 UI
    private void UpdateHUD()
    {
        if (scoreText) scoreText.text = $"SCORE : {score}";
        if (livesText) livesText.text = $"LIVES : {lives}";
        if (levelText) levelText.text = $"LEVEL : {level}";

        UpdateLivesUI();
    }

    // ✅ 更新时间 UI
    private void UpdateTimerUI()
    {
        if (timerText)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = $"TIME : {minutes:00}:{seconds:00}";
        }
    }

    // ❤️ 更新生命图标（动态数量）
    private void UpdateLivesUI()
    {
        if (livesPanel == null || lifeIconPrefab == null) return;

        // 先清空旧的图标
        foreach (var icon in lifeIcons)
            Destroy(icon);
        lifeIcons.Clear();

        // 重新生成图标
        for (int i = 0; i < lives; i++)
        {
            GameObject icon = Instantiate(lifeIconPrefab, livesPanel);
            lifeIcons.Add(icon);
        }
    }

    // ❤️ 动画：生命消失时缩小 & 淡出
    private IEnumerator RemoveLifeIconAnimated()
    {
        if (lifeIcons.Count == 0) yield break;

        GameObject lastIcon = lifeIcons[lifeIcons.Count - 1];
        lifeIcons.Remove(lastIcon);

        RectTransform rt = lastIcon.GetComponent<RectTransform>();
        CanvasGroup cg = lastIcon.GetComponent<CanvasGroup>();
        if (cg == null) cg = lastIcon.AddComponent<CanvasGroup>();

        float t = 0f;
        Vector3 startScale = rt.localScale;
        Vector3 endScale = Vector3.zero;

        while (t < 1f)
        {
            t += Time.deltaTime * 2f; // 动画时长 0.5s
            rt.localScale = Vector3.Lerp(startScale, endScale, t);
            cg.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        Destroy(lastIcon);
    }

    // ✅ 暂停 / 继续
    public void PauseGame(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
    }

    // ✅ 游戏结束
    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("💀 GAME OVER");
        // TODO: 可显示 Game Over UI 或切场景
        // SceneManager.LoadScene("GameOverScene");
    }

    // ✅ 返回主菜单按钮
    public void GoToMainMenu()
    {
        Debug.Log("🔙 Returning to Main Menu...");
        Time.timeScale = 1f; // 确保游戏不暂停
        SceneManager.LoadScene("MainMenu");
    }
}
