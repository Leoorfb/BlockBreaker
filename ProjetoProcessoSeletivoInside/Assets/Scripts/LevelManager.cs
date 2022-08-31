using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public UnityEvent NextLevelEvent;
    public UnityEvent GameOverEvent;

    public static LevelManager Instance { get; private set; }

    public int lives = 3;
    public int level = 1;

    public int levelSpeed;
    public int levelMultiplier = 5;

    [SerializeField]
    private PowerUpEffect[] powerUpsAvaible;
    [SerializeField]
    private float powerUpMaxSpawnValue;

    public bool isBuildingLevel = true;
    
    public bool isGameOver = false;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        level = 0;
        NextLevel();
    }

    public void GameOver()
    {
        isGameOver = true;
        GameOverEvent.Invoke();
        GameMenuUI.Instance.SetGameOverScreen(true);
        ScoresManager.Instance.SaveNewGameData();
    }

    public void SetLevel(int _level)
    {
        level = _level;
        GameMenuUI.Instance.UpdateLevel();
        levelSpeed = level + levelMultiplier;

        PlayerController.Instance.speed = levelSpeed;
        Ball.speed = levelSpeed;
    }

    public void NextLevel()
    {
        Debug.Log(level++);
        SetLevel(level++);
        NextLevelEvent.Invoke();
    }
}
