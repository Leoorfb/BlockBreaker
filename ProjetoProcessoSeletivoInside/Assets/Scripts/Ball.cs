using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private AudioSource ballAudio_;

    public ParticleSystem breakParticle;

    public AudioClip[] bounceSounds;
    public AudioClip[] breakSounds;


    Vector3 moveDirection = Vector3.up;
    public float speed = 5f;

    public float startAngleMin = 30f; 
    public float startAngleMax = 150f; 
    public float bounceAngleRange = 15f;

    private float screenLimitY = -4f;
    private Vector3 startPosition;

    private bool waitingPlayerInput = true;
    //[SerializeField]
    //private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = AddRandomDirection(startAngleMin, startAngleMax);
        startPosition = transform.position;
        ballAudio_ = GetComponent<AudioSource>();
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (LevelManager.Instance.isBuildingLevel  || LevelManager.Instance.isGameOver)
        {
            return;
        }

        if (waitingPlayerInput)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
                waitingPlayerInput = false;
            return;
        }

        transform.Translate(moveDirection * speed * Time.deltaTime);

        if (transform.position.y < screenLimitY)
        {
            outOfScreen();
        }
    }

    public void StartSetup()
    {
        moveDirection = Vector3.up;
        moveDirection = AddRandomDirection(startAngleMin, startAngleMax);
        transform.position = startPosition;
        PlayerController.Instance.transform.position = PlayerController.Instance.startPosition;
        waitingPlayerInput = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        Bounce(collision.contacts[0].normal);

        if (collision.gameObject.CompareTag("Block"))
        {
            breakParticle.startColor = collision.gameObject.GetComponent<Block>().color;
            breakParticle.transform.position = collision.contacts[0].point;
            breakParticle.Play();

            collision.gameObject.GetComponent<Block>().Break();
            PlayRandomSound(breakSounds);

            if (Block.blockCount <= 0)
            {
                StartSetup();
            }
        }
        else
        {
            PlayRandomSound(bounceSounds);
        }
    }


    void Bounce(Vector3 flipAxis)
    {
        moveDirection = FlipDirection(flipAxis);
        moveDirection = AddRandomDirection(-bounceAngleRange, bounceAngleRange);
    }

    void PlayRandomSound(AudioClip[] sounds)
    {
        int randomIndex = Random.Range(0, sounds.Length);

        ballAudio_.PlayOneShot(sounds[randomIndex]);
    }

    Vector3 FlipDirection(Vector3 flipAxis)
    {
        return  Vector3.Reflect(moveDirection, flipAxis);
    }
    Vector3 AddRandomDirection(float minAngle, float maxAngle)
    {
        Quaternion randomAngle = Quaternion.AngleAxis(Random.Range(minAngle, maxAngle), Vector3.forward);
        return randomAngle * moveDirection;
    }
        
    void outOfScreen()
    {
        if (LevelManager.Instance.lives > 0)
        {
            GameMenuUI.Instance.AddLives(-1);
            StartSetup();
        }
        else
        {
            LevelManager.Instance.GameOver();
        }
    }
}
