using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Evento ativado quando a quantidade de vidas é alterada
    public IntEvent LivesChangeEvent;
    // Evento ativado quando certos powerUps são usados
    public PowerUpEvent PlayerUsedPowerUp;

    // Quantidade de vidas (talvez mover para outro script)
    public int lives = 3;

    // Velocidade do jogador
    public float speed = 5f;
    // Posição inicial do jogador
    private Vector3 startPosition;

    // Os blocos estão sendo criados?
    private bool areBlocksSpawning = true;
    // O jogo acabou?
    private bool isGameOver = false;

    private float currentHalfWidth { get { return transform.localScale.x / 2; } }

    [SerializeField]
    LayerMask borderMask;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Start()
    {
        ScoresManager.Instance.player = this;
    }

    private void FixedUpdate()
    {
        if (areBlocksSpawning || isGameOver)
            return;

        float moveInput = Input.GetAxisRaw("Horizontal");

        Vector3 direction = Vector3.right * moveInput;
        float distance = speed  * Time.fixedDeltaTime;
        Vector3 movement = direction * distance;

        Ray ray = new Ray(transform.position, direction);

        if (!Physics.Raycast(ray, distance + currentHalfWidth, borderMask))
        {
            transform.Translate(movement);
        }
    }

    // Funções para definir o valor de areBlocksSpawning e isGameOver (Adionadas a eventos)
    public void SetBlocksAreSpawning()
    {
        areBlocksSpawning = true;
        StartSetup();
    }
    public void SetBlocksAreNotSpawning() => areBlocksSpawning = false;
    public void SetGameIsOver() => isGameOver = true;

    // Função que causa dano ao player
    public void damagePlayer()
    {
        damagePlayer(1);
    }
    public void damagePlayer(int damage)
    {
        lives -= damage;
        LivesChangeEvent.Invoke(lives);
    }

    // Função que define velocidade
    public void SetSpeed(int level, int levelMultiplier, int levelSpeed)
    {
        speed = levelSpeed;
    }
    public void SetSpeed(int _speed)
    {
        speed = _speed;
    }
    // Função que reconfigura o player para a configuração inicial
    public void StartSetup()
    {
        transform.position = startPosition;
    }
}
