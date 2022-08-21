using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance
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
    private GameObject gameOverScreen;

    private int lives = 3;
    private int level = 1;

    public int blockRowSize = 8;
    public int blockColSize = 1;
    public float blockSpacing = .1f;
    public Color[] rowsColors;
    public Color currentRowColor;
    public int currentRowScore;

    public int levelSpeed;
    public int levelMultiplier = 5;

    [SerializeField]
    private GameObject blockPrefab;
    private Vector3 topLeftCorner;

    float blockHeight;
    float blockWidth;
    float colWidth = 14.5f;

    public bool isBuildingLevel = true;
    public bool isGameOver = false;

    float blockHalfHeight 
    { get
        {
            return blockHeight / 2;
        }
    }
    float blockHalfWidth
    {
        get
        {
            return blockWidth / 2;
        }
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        blockHeight = blockPrefab.transform.localScale.y;
        blockWidth = blockPrefab.transform.localScale.x;
        topLeftCorner = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetLives(3);
        UpdatePlayerName();
        SetScore(0);
        SetLevel(1);
        StartCoroutine(SpawnBlocks());
    }

    private void Update()
    {
        UpdateTime();
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        ScoresManager.Instance.SaveNewGameData();
        isGameOver = true;
    }

    IEnumerator SpawnBlocks()
    {
        yield return new WaitForSeconds(.5f / blockColSize);
        isBuildingLevel = true;
        Vector3 position;
        Vector3 topLeftCornerOffset = new Vector3(topLeftCorner.x - blockHalfWidth, topLeftCorner.y + blockHalfHeight, 0);
        Vector3 blockSize = new Vector3(blockWidth, blockHeight, 2);

        for (int y = 1; y <= blockRowSize; y++)
        {
            currentRowColor = rowsColors[y-1];
            currentRowScore = (blockRowSize - y + 1) * levelMultiplier * level;
            for (int x = 1; x <= blockColSize; x++)
            {
                position = new Vector3((x * (blockSpacing + blockWidth)),(-y * (blockSpacing + blockHeight)), 0);
                GameObject newBlock = Instantiate(blockPrefab, topLeftCornerOffset + position, blockPrefab.transform.rotation);
                newBlock.transform.SetParent(transform);
                newBlock.transform.localScale = blockSize;
                Block.blockCount++;
                yield return new WaitForSeconds(.5f/blockColSize);
            }
        }
        isBuildingLevel = false;
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
    }

    public void AddScore(int _score)
    {
        SetScore(ScoresManager.Instance.score + _score);
    }

    public int GetLives()
    {
        return lives;
    }
    public void SetLives(int _lives)
    {
        lives = _lives;
        livesText.text = "Lives: " + lives;
    }

    public void AddLives(int _lives)
    {
        SetLives(lives + _lives);
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

    public void SetLevel(int _level)
    {
        Debug.Log("level " + level + " _level" + _level);
        level = _level;
        levelText.text = "Level: " + level;
        levelSpeed = level * levelMultiplier;
        blockColSize = level;
        blockWidth = (colWidth / level) - blockSpacing;
        Debug.Log("Block Width " + blockWidth);
    }

    public void NextLevel()
    {
        Debug.Log(level++);
        SetLevel(level++);
        StartCoroutine(SpawnBlocks());
    }
}
