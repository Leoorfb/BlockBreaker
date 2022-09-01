using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class BlockSpawner : MonoBehaviour
{
    // Eventos ativados quando os blocos começam e terminam de ser criados
    public UnityEvent SpawningBlocksEvent;
    public UnityEvent FinishedSpawningBlockEvent;

    // Pool de blocos
    ObjectPool<Block> _pool;

    // Prefab dos blocos
    [SerializeField]
    private GameObject _blockPrefab;

    // Atributos relaciondos ao grid de blocos
    [Header("Block Grid Settings")]
    [SerializeField]
    private int blockRowSize = 8;
    private int blockColSize = 1;
    [SerializeField]
    private float blockSpacing = .1f;
    [SerializeField]
    private Color[] rowsColors;
    private Color currentRowColor;
    private int currentRowScore;
    private float colWidth = 14.5f;
    private Vector3 topLeftCorner;

    // Atributos relaciondos aos blocos
    private float blockHeight;
    private float blockWidth;
    
    private float blockHalfHeight
    {
        get
        {
            return blockHeight / 2;
        }
    }
    private float blockHalfWidth
    {
        get
        {
            return blockWidth / 2;
        }
    }

    public bool isSpawningBlocks { get; private set; }
    
    // Power Ups disponíves
    [Header("Power Ups Settings")]
    [SerializeField]
    private PowerUpEffect[] powerUpsAvaible;
    private float powerUpMaxSpawnValue;

    private void Awake() {
        SetPowerUpMaxSpawnValue();

        blockHeight = _blockPrefab.transform.localScale.y;
        blockWidth = _blockPrefab.transform.localScale.x;
        topLeftCorner = transform.position;

        _pool = new ObjectPool<Block>(CreateBlock, OnTakeBlockFromPool, OnReturnBlockToPool);
    }

    // Função que cria o bloco na pool
    Block CreateBlock()
    {
        var block = Instantiate(_blockPrefab);
        block.GetComponent<Block>().SetPool(_pool);
        return block.GetComponent<Block>();
    }

    // Função que ativa o bloco na pool
    void OnTakeBlockFromPool(Block block)
    {
        block.gameObject.SetActive(true);
        block.RestartBlock(currentRowColor,currentRowScore);
    }

    // Função que desativa o bloco na pool
    void OnReturnBlockToPool(Block block)
    {
        block.gameObject.SetActive(false);
    }

    // Função para chamar corrotina SpawnBlocks (Chamada no NextLevelEvent do LevelManager)
    public void StartSpawnBlocks(int level, int levelMultiplier, int levelSpeed)
    {
        StartCoroutine(SpawnBlocks(level,levelMultiplier));
    }

    // Corrotina que cria a grid de blocos
    IEnumerator SpawnBlocks(int level, int levelMultiplier)
    {
        SpawningBlocksEvent.Invoke();

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
                position = topLeftCornerOffset + new Vector3((x * (blockSpacing + blockWidth)), (-y * (blockSpacing + blockHeight)), 0);

                SpawnBlock(position, blockSize);

                yield return new WaitForSeconds(.5f / blockColSize);
            }
        }

        isSpawningBlocks = false;
        FinishedSpawningBlockEvent.Invoke();
    }

    // Função que cria o bloco
    void SpawnBlock(Vector3 position, Vector3 blockSize)
    {
        var block = _pool.Get();

        block.transform.position = position;
        block.transform.SetParent(transform);
        block.transform.localScale = blockSize;

        SpawnPowerUp(block, Random.Range(0f, 100f));

        Block.blockCount++;
    }

    // Função que define o Power Up do bloco
    void SpawnPowerUp(Block block, float spawnValue)
    {
        if (spawnValue > powerUpMaxSpawnValue)
        {
            block.powerUpEffect = null;
            //block.name = "Block";
            return;
        }

        float powerUpValue = 0;
        foreach (PowerUpEffect PUEffect in powerUpsAvaible)
        {
            powerUpValue += PUEffect.spawnChance;
            if (spawnValue < powerUpValue)
            {
                block.powerUpEffect = PUEffect;
                //block.name = "Block " + PUEffect.name;
                return;
            }
        }
    }

    // Função que calcula a chance de um bloco ter um Power Up
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
