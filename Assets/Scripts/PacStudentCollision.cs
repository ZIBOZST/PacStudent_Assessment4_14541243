using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentCollision : MonoBehaviour
{
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ghost"))
        {
            Debug.Log("Player hit by ghost!");
            // 玩家回到初始位置
            transform.position = startPosition;
        }
    }
}
