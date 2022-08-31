using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Block : MonoBehaviour
{
    public Color color { get; private set; }
    private MeshRenderer Renderer;
    private Material material;
    public int scoreValue = 5;

    public static int blockCount = 0;

    public bool hasPowerUp = false;

    private SpriteRenderer powerUpSpriteRenderer;

    private PowerUpEffect _powerUpEffect;
    public PowerUpEffect powerUpEffect 
    { get 
        {
            return _powerUpEffect;
        } 
      set
        {
            _powerUpEffect = value;
            if (value != null)
            {
                hasPowerUp = true;
                
                powerUpSpriteRenderer.enabled = true;
                powerUpSpriteRenderer.sprite = powerUpEffect.powerUpIcon;
                Vector3 spriteScale = powerUpSpriteRenderer.transform.localScale;
                powerUpSpriteRenderer.transform.localScale = new Vector3(spriteScale.x/ transform.localScale.x, spriteScale.y/ transform.localScale.y, 1);
            }
            else
            {
                hasPowerUp = false;
                powerUpSpriteRenderer.enabled = false;
                material.color = color;
            }
        } 
    }

    IObjectPool<Block> _pool;
    public void SetPool(IObjectPool<Block> pool) => _pool = pool;

    private void Awake()
    {
        powerUpSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Renderer = GetComponent<MeshRenderer>();
        material = Renderer.material;
    }

    public void RestartBlock(Color _color, int _scoreValue)
    {
        color = _color;
        scoreValue = _scoreValue;
        Debug.Log(color);
        material.color = color;
    }

    public void Break(Ball ball)
    {
        GameMenuUI.Instance.AddScore(scoreValue);
        blockCount--;
        if(blockCount <= 0)
        {
            LevelManager.Instance.NextLevel();
        }

        if (hasPowerUp)
        {
            powerUpEffect.Apply(ball);
        }

        if(_pool != null)
        {
            _pool.Release(this);
        }
        else { 
            Destroy(gameObject);
        }
    }
}
