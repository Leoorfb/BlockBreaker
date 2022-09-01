using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StartLevelEvent : UnityEvent<int, int, int> { }

public class LevelManager : MonoBehaviour
{
    // Eventos ativados quando começa o jogador passa de fase ou perde o jogo
    public StartLevelEvent StartLevelEvent;
    public UnityEvent GameOverEvent;

    // Atributos sobre o level
    public int level = 0;
    public int levelMultiplier = 5;
    private int _levelSpeed;
    private int levelSpeed {
        get { return _levelSpeed; }  
        set {
            if (value < 10)
                _levelSpeed = value;
            else
                _levelSpeed = 10 + (value / 10);
        } }
    
    // Caso o jogo acabou
    public bool isGameOver = false;
    
    void Start()
    {
        ScoresManager.Instance.score = 0;
        NextLevel();
        
        // Scores Manager
        GameOverEvent.AddListener(ScoresManager.Instance.SaveNewGameData);
    }

    public void GameOverCheck (int livesRemaining)
    {
        if (livesRemaining < 0)
        {
            GameOver();
        }
    }

    // Função que acaba o jogo
    public void GameOver()
    {
        isGameOver = true;
        GameOverEvent.Invoke();
    }

    // Função que define a fase
    public void SetLevel(int _level)
    {
        level = _level;
        levelSpeed = level + levelMultiplier;
        StartLevelEvent.Invoke(level,levelMultiplier,levelSpeed);
    }

    // Função que avança para a próxima fase
    public void NextLevel()
    {
        SetLevel(level++);
    }
}
