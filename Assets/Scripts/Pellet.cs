using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int points = 10; // 每颗豆子的分数

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 找到分数管理器并加分
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.AddScore(points);
            }

            // 吃掉豆子
            Destroy(gameObject);
        }
    }
}
