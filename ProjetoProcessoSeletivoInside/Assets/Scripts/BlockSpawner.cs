using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class BlockSpawner : MonoBehaviour
{
    public UnityEvent SpawningBlocksEvent;
    public UnityEvent FinishedSpawningBlockEvent;

    public static BlockSpawner Instance { get; private set; }

    ObjectPool<Block> _pool;

    [SerializeField]
    private GameObject _blockPrefab;

    public int blockRowSize = 8;
    public int blockColSize = 1;
    public float blockSpacing = .1f;
    public Color[] rowsColors;
    public Color currentRowColor;
    public int currentRowScore;

    private Vector3 topLeftCorner;

    float blockHeight;
    float blockWidth;
    float colWidth = 14.5f;

    float blockHalfHeight
    {
        get
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

    public bool isSpawningBlocks;

    [SerializeField]
    private PowerUpEffect[] powerUpsAvaible;
    [SerializeField]
    private float powerUpMaxSpawnValue;

    private void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SetPowerUpMaxSpawnValue();

        blockHeight = _blockPrefab.transform.localScale.y;
        blockWidth = _blockPrefab.transform.localScale.x;
        topLeftCorner = transform.position;

        _pool = new ObjectPool<Block>(CreateBlock, OnTakeBlockFromPool, OnReturnBlockToPool);
    }

    void Start()
    {
        LevelManager.Instance.NextLevelEvent.AddListener(StartSpawnBlocks);
    }

    Block CreateBlock()
    {
        var block = Instantiate(_blockPrefab);
        block.GetComponent<Block>().SetPool(_pool);
        return block.GetComponent<Block>();
    }

    void OnTakeBlockFromPool(Block block)
    {
        block.gameObject.SetActive(true);
        block.RestartBlock(currentRowColor,currentRowScore);
    }

    void OnReturnBlockToPool(Block block)
    {
        block.gameObject.SetActive(false);
    }

    void StartSpawnBlocks()
    {
        StartCoroutine(SpawnBlocks());
    }

    IEnumerator SpawnBlocks()
    {
        SpawningBlocksEvent.Invoke();

        int level = LevelManager.Instance.level;
        int levelMultiplier = LevelManager.Instance.levelMultiplier;

        blockColSize = level;
        blockWidth = (colWidth / level) - blockSpacing;

        yield return new WaitForSeconds(.5f / blockColSize);

        isSpawningBlocks = true;
        Vector3 position;
        Vector3 topLeftCornerOffset = new Vector3(topLeftCorner.x - blockHalfWidth, topLeftCorner.y + blockHalfHeight, 0);
        Vector3 blockSize = new Vector3(blockWidth, blockHeight, 2);

        for (int y = 1; y <= blockRowSize; y++)
        {
            currentRowColor = rowsColors[y - 1];
            currentRowScore = (blockRowSize - y + 1) * levelMultiplier * level;
            for (int x = 1; x <= blockColSize; x++)
            {
                var block = _pool.Get();

                position = new Vector3((x * (blockSpacing + blockWidth)), (-y * (blockSpacing + blockHeight)), 0);
                
                block.transform.position = topLeftCornerOffset + position;
                block.transform.SetParent(transform);
                block.transform.localScale = blockSize;

                SpawnPowerUp(block.GetComponent<Block>(), Random.Range(0f, 100f));

                Block.blockCount++;
                yield return new WaitForSeconds(.5f / blockColSize);
            }
        }

        isSpawningBlocks = false;
        FinishedSpawningBlockEvent.Invoke();
    }

    void SpawnPowerUp(Block block, float spawnValue)
    {
        if (spawnValue > powerUpMaxSpawnValue)
        {
            block.powerUpEffect = null;
            return;
        }

        float powerUpValue = 0;

        foreach (PowerUpEffect PUEffect in powerUpsAvaible)
        {
            powerUpValue += PUEffect.spawnChance;
            if (spawnValue < powerUpValue)
            {
                block.powerUpEffect = PUEffect;
                return;
            }
        }
    }

    void SetPowerUpMaxSpawnValue()
    {
        float powerUpValue = 0;
        foreach (PowerUpEffect PUEffect in powerUpsAvaible)
        {
            powerUpValue += PUEffect.spawnChance;
        }
        powerUpMaxSpawnValue = powerUpValue;
    }
}
