using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameMenuUI : MonoBehaviour
{
    private static GameMenuUI _instance;
    public static GameMenuUI Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    [SerializeField]
    private TextMeshProUGUI playerNameText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI livesText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private TextMeshProUGUI bestPlayerNameText;
    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    [SerializeField]
    private TextMeshProUGUI bestTimeText;

    [SerializeField]
    private GameObject gameOverScreen;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetLives(3);
        UpdatePlayerName();
        SetScore(0);

        UpdateBestScoreScreen();
    }

    private void Update()
    {
        UpdateTime();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetGameOverScreen(bool active)
    {
        gameOverScreen.SetActive(active);
    }

    public void UpdateTime()
    {
        ScoresManager.Instance.time += Time.deltaTime;

        TimeSpan time = TimeSpan.FromSeconds(ScoresManager.Instance.time);

        timeText.text = "Time: " + time.ToString("hh':'mm':'ss");
    }

    public void SetScore(int _score)
    {
        ScoresManager.Instance.score = _score;
        scoreText.text = "Score: " + ScoresManager.Instance.score;
        scoreText.fontSize = ScoreFontSize(_score);
    }

    public void AddScore(int _score)
    {
        SetScore(ScoresManager.Instance.score + _score);
    }

    public void SetLives(int _lives)
    {
        LevelManager.Instance.lives = _lives;
        livesText.text = "Lives: " + LevelManager.Instance.lives;
    }

    public void AddLives(int _lives)
    {
        SetLives(LevelManager.Instance.lives + _lives);
    }

    public void SetPlayerName(string _playerName)
    {
        ScoresManager.Instance.playerName = _playerName;
        UpdatePlayerName();
    }

    public void UpdatePlayerName()
    {
        playerNameText.text = ScoresManager.Instance.playerName;
    }

    public void UpdateLevel()
    {
        levelText.text = "Level: " + LevelManager.Instance.level;
    }

    public void UpdateBestScoreScreen()
    {
        UpdateBestPlayerName();
        UpdateBestScore();
        UpdateBestTime();
    }

    public void UpdateBestPlayerName()
    {
        bestPlayerNameText.text = ScoresManager.Instance.scoreDataList[0].playerName;
    }

    public void UpdateBestScore()
    {
        int _score = ScoresManager.Instance.scoreDataList[0].score;
        bestScoreText.text = "Score: " + _score;

        bestScoreText.fontSize = ScoreFontSize(_score);
    }

    public int ScoreFontSize(int _score)
    {
        int fontSize = 48;
        int scoreResto = _score / 10000;
        if (scoreResto > 0)
        {
            fontSize -= 2;
            scoreResto /= 10;

            while (scoreResto > 0)
            {
                fontSize -= 4;
                scoreResto /= 10;
            }
        }
        return fontSize;
    }

        public void UpdateBestTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(ScoresManager.Instance.scoreDataList[0].time);

        bestTimeText.text = "Time: " + time.ToString("hh':'mm':'ss");
    }
}
