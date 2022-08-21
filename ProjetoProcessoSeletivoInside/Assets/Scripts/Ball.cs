using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

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
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (LevelManager.Instance.isBuildingLevel  || LevelManager.Instance.isGameOver)
        {
            transform.position = startPosition;
            PlayerController.Instance.transform.position = PlayerController.Instance.startPosition;
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

    void OnCollisionEnter(Collision collision)
    {
        Bounce(collision.contacts[0].normal);
    }

    void Bounce(Vector3 flipAxis)
    {
        moveDirection = FlipDirection(flipAxis);
        moveDirection = AddRandomDirection(-bounceAngleRange, bounceAngleRange);
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
        if (LevelManager.Instance.GetLives() > 0)
        {
            LevelManager.Instance.AddLives(-1);
            transform.position = startPosition;
            PlayerController.Instance.transform.position = PlayerController.Instance.startPosition;
            waitingPlayerInput = true;
        }
        else
        {
            LevelManager.Instance.GameOver();
        }
    }
}
