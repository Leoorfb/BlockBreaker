using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    // Evento chamado quando o ultimo bloco � quebrado
    public UnityEvent BrokeLastBlockEvent;
    // Evento chamado quando a bola sai da tela
    public UnityEvent BallLeftTheScreenEvent;

    // Efeitos son�rios
    [Header("SFX Settings")]
    public AudioClip[] bounceSounds;
    public AudioClip[] breakSounds;
    private AudioSource ballAudio_;
    
    // Part�cula de quando quebra um bloco
    public ParticleSystem breakParticle;

    // Dire��o de Movimento
    private Vector3 moveDirection = Vector3.up;
    // Velocidade de Movimento
    public static float speed = 5f;

    // Angulo de Inicio
    private float startAngleRange = 35f;

    // Limite da tela vertical
    private float screenLimitY = -4f;
    
    // Posi��o inicial
    private Vector3 startPosition;

    // Quantos Power Ups tem
    public int explosivePowerUps = 0;
    public int speedPowerUps = 0;
    public TrailRenderer ballTrail;
    public TrailRenderer explosiveTrail;
    public TrailRenderer speedTrail;

    // Est� esperando input do jogador?
    private bool isWaitingPlayerInput = true;
    // Os blocos est�o sendo criados?
    private bool areBlocksSpawning = true;
    // O jogo acabou?
    private bool isGameOver = false;

    void Start()
    {
        ballAudio_ = GetComponent<AudioSource>();
        startPosition = transform.position;
        StartSetup();
    }

    void FixedUpdate()
    {
        if (areBlocksSpawning || isGameOver)
        {
            return;
        }
        if (isWaitingPlayerInput)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
                isWaitingPlayerInput = false;
            return;
        }

        transform.Translate(moveDirection * speed * Time.deltaTime);

        if (transform.position.y < screenLimitY)
        {
            outOfScreen();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Bounce(collision.contacts[0].normal);

        if (collision.gameObject.CompareTag("Block"))
        {
            var main = breakParticle.main;
            main.startColor = collision.gameObject.GetComponent<Block>().color;
            breakParticle.transform.position = collision.contacts[0].point;
            breakParticle.Play();

            int blockCount;
            collision.gameObject.GetComponent<Block>().Break(this, (explosivePowerUps > 0), out blockCount);

            if (blockCount <= 0)
            {
                BrokeLastBlockEvent.Invoke();
            }

            PlayRandomSound(breakSounds);
        }
        else
        {
            PlayRandomSound(bounceSounds);
        }
    }

    // Fun��es para definir o valor de areBlocksSpawning e isGameOver (Adionadas a eventos)
    public void SetBlocksAreSpawning()
    {
        areBlocksSpawning = true;
        StartSetup();
    }
    public void SetBlocksAreNotSpawning() => areBlocksSpawning = false;
    public void SetGameIsOver() => isGameOver = true;

    // Fun��o que define velocidade
    public void SetSpeed(int level, int levelMultiplier, int levelSpeed)
    {
        speed = levelSpeed;
    }
    public void SetSpeed(int _speed)
    {
        speed = _speed;
    }

    // Fun��o que reconfigura a bola para a configura��o inicial
    public void StartSetup()
    {
        moveDirection = Vector3.up;
        moveDirection = AddRandomDirection(startAngleRange);
        transform.position = startPosition;
        isWaitingPlayerInput = true;
    }

    // Fun��o que vira a dire��o de movimento
    void Bounce(Vector3 flipAxis)
    {
        moveDirection = Vector3.Reflect(moveDirection, flipAxis);
    }

    // Toca um som de uma array de sons
    void PlayRandomSound(AudioClip[] sounds)
    {
        int randomIndex = Random.Range(0, sounds.Length);
        ballAudio_.PlayOneShot(sounds[randomIndex]);
    }

    // Fun��o que adiciona um angulo aleat�ria a dire��o de movimento
    Vector3 AddRandomDirection(float angleRange)
    {
        Quaternion randomAngle = Quaternion.AngleAxis(Random.Range(-angleRange, angleRange), Vector3.forward);
        return randomAngle * moveDirection;
    }
    
    // Fun��o para quando a Bola sai da tela
    void outOfScreen()
    {
        BallLeftTheScreenEvent.Invoke();
    }
}
