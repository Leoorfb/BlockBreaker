using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameMenuUI : MonoBehaviour
{
    [Header("Player UI Settings")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Best Player UI Settings")]
    [SerializeField] private TextMeshProUGUI bestPlayerNameText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI bestTimeText;

    [Header("Screens Settings")]
    [SerializeField] private GameObject gameOverScreen;

    // O jogo acabou?
    private bool isGameOver = false;

    private void Start()
    {
        UpdateLives(ScoresManager.Instance.player.lives);
        UpdatePlayerName();
        UpdateScore(0);

        ScoresManager.Instance.time = 0;

        UpdateBestScoreScreen();
        ScoresManager.Instance.ScoreChangeEvent.AddListener(UpdateScore);
    }

    private void Update()
    {
        UpdateTime();
    }

    // Função para definir o valor de isGameOver (Adionada a evento)
    public void SetGameIsOver()
    {
        isGameOver = true;
        SetGameOverScreen();
    }
    // Reinicia o jogo
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Volta para o Menu Inicial
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    // Ativa a tela de game over
    public void SetGameOverScreen()
    {
        gameOverScreen.SetActive(isGameOver);
    }
    
    // Atualiza o tempo de partida
    public void UpdateTime()
    {
        if (!isGameOver)
        { 
            ScoresManager.Instance.time += Time.deltaTime;

            TimeSpan time = TimeSpan.FromSeconds(ScoresManager.Instance.time);

            timeText.text = "Time: " + time.ToString("hh':'mm':'ss");
        }
    }

    // Define quantos pontos o player atual tem
    public void UpdateScore(int _score)
    {
        scoreText.text = "Score: " + _score;
        scoreText.fontSize = ScoreFontSize(_score);
    }

    // Atualiza o texto do campo vidas do player atual
    public void UpdateLives(int _lives)
    {
        livesText.text = "Lives: " + _lives;
    }
    // Atualiza o texto do campo nome do player atual
    public void UpdatePlayerName()
    {
        playerNameText.text = ScoresManager.Instance.playerName;
    }
    // Atualiza o texto do campo fase atual
    public void UpdateLevel(int level, int levelMultiplier, int levelSpeed)
    {
        levelText.text = "Level: " + level;
    }

    // Atualiza os campos do melhor player
    public void UpdateBestScoreScreen()
    {

        
        if (ScoresManager.Instance.scoreDataList.Count > 0)
        {
            ScoreData bestScore = ScoresManager.Instance.scoreDataList[0];

            UpdateBestPlayerName(bestScore.playerName);
            UpdateBestScore(bestScore.score);
            UpdateBestTime(bestScore.time);
        }
        else 
        {
            UpdateBestPlayerName("None");
            UpdateBestScore(0);
            UpdateBestTime(0);
        }
    }
    // Atualiza o texto do campo nome do melhor player
    public void UpdateBestPlayerName(string name)
    {
        bestPlayerNameText.text = name;
    }
    // Atualiza o texto do campo pontuação do melhor player
    public void UpdateBestScore(int score)
    {
        bestScoreText.text = "Score: " + score;

        bestScoreText.fontSize = ScoreFontSize(score);
    }
    // Define o tamanho da fonte do campo de pontuação
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
    // Atualiza o texto do campo tempo do melhor player
    public void UpdateBestTime(float _time)
    {
        TimeSpan time = TimeSpan.FromSeconds(_time);

        bestTimeText.text = "Time: " + time.ToString("hh':'mm':'ss");
    }
}
