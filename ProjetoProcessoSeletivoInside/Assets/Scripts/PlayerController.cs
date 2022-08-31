using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance
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

    public float speed = 5f;
    public Vector3 startPosition;

    private bool areBlocksSpawning = true;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        startPosition = transform.position;
    }

    private void Start()
    {   
        LevelManager.Instance.GameOverEvent.AddListener(SetGameIsOver);
        BlockSpawner.Instance.SpawningBlocksEvent.AddListener(SetBlocksAreSpawning);
        BlockSpawner.Instance.FinishedSpawningBlockEvent.AddListener(SetBlocksAreNotSpawning);
    }
    
    private void FixedUpdate()
    {
        if (areBlocksSpawning || isGameOver)
            return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * speed * moveInput * Time.fixedDeltaTime);
    }

    void SetBlocksAreSpawning()
    {
        areBlocksSpawning = true;
        StartSetup();
    }
    void SetBlocksAreNotSpawning()
    {
        areBlocksSpawning = false;
    }
    void SetGameIsOver()
    {
        isGameOver = true;
    }
    public void StartSetup()
    {
        transform.position = startPosition;
    }
}
