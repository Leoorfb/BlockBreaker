using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Color color = Color.white;
    private MeshRenderer Renderer;
    private Material material;
    public int scoreValue = 5;

    public static int blockCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<MeshRenderer>();
        material = Renderer.material;

        color = LevelManager.Instance.currentRowColor;
        scoreValue = LevelManager.Instance.currentRowScore;

        material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Break();
    }

    void Break()
    {
        LevelManager.Instance.AddScore(scoreValue);
        blockCount--;
        if(blockCount <= 0)
        {
            LevelManager.Instance.NextLevel();
        }
        Destroy(gameObject);
    }
}
