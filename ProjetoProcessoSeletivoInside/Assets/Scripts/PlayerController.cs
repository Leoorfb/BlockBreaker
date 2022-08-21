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

    private void FixedUpdate()
    {
        if (LevelManager.Instance.isBuildingLevel || LevelManager.Instance.isGameOver)
            return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * speed * moveInput * Time.deltaTime);
    }
}
