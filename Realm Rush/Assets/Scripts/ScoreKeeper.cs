using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] int enemyLeftToKill = 25;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject winCanvas;

    public void Update() {
        scoreText.text = "Enemy Left To Kill: " + enemyLeftToKill;
        if(enemyLeftToKill <= 0) {
            Time.timeScale = 0f;
            winCanvas.SetActive(true);
        }
    }

    public void DecreaseEnemyLeft() {
        enemyLeftToKill--;
    }
}
