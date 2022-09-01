using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Evento ativado quando a quantidade de vidas � alterada
    public IntEvent LivesChangeEvent;
    
    // Quantidade de vidas (talvez mover para outro script)
    public int lives = 3;

    // Velocidade do jogador
    public float speed = 5f;
    // Posi��o inicial do jogador
    private Vector3 startPosition;

    // Os blocos est�o sendo criados?
    private bool areBlocksSpawning = true;
    // O jogo acabou?
    private bool isGameOver = false;

    private void Awake()
    {
        ScoresManager.Instance.player = this;
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (areBlocksSpawning || isGameOver)
            return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * speed * moveInput * Time.fixedDeltaTime);
    }

    // Fun��es para definir o valor de areBlocksSpawning e isGameOver (Adionadas a eventos)
    public void SetBlocksAreSpawning()
    {
        areBlocksSpawning = true;
        StartSetup();
    }
    public void SetBlocksAreNotSpawning() => areBlocksSpawning = false;
    public void SetGameIsOver() => isGameOver = true;

    // Fun��o que causa dano ao player
    public void damagePlayer()
    {
        damagePlayer(1);
    }
    public void damagePlayer(int damage)
    {
        lives -= damage;
        LivesChangeEvent.Invoke(lives);
    }

    // Fun��o que define velocidade
    public void SetSpeed(int level, int levelMultiplier, int levelSpeed)
    {
        speed = levelSpeed;
    }
    public void SetSpeed(int _speed)
    {
        speed = _speed;
    }
    // Fun��o que reconfigura o player para a configura��o inicial
    public void StartSetup()
    {
        transform.position = startPosition;
    }
}
