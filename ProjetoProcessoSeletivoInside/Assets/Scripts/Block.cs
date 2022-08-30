using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //private IEnumerator tPUAnim;

    private void Awake()
    {
        powerUpSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Renderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        material = Renderer.material;

        color = LevelManager.Instance.currentRowColor;
        scoreValue = LevelManager.Instance.currentRowScore;

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
        Destroy(gameObject);
    }

    /*
    IEnumerator temporaryPowerUpAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(.75f);
            material.color = Color.white;
            yield return new WaitForSeconds(.1f);
            material.color = color;
        }
    }
    */
}
