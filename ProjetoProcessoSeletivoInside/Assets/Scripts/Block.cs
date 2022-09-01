using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Block : MonoBehaviour
{
    // Atributos relacinados a cor do bloco
    public Color color { get; private set; }
    private MeshRenderer Renderer;
    private Material material;

    // Quantos pontos o bloco vale
    private int scoreValue = 5;

    // Quantidade de blocos na tela
    public static int blockCount = 0;

    // Atributos relacinados ao PowerUp do bloco
    private bool hasPowerUp = false;
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

    // Pool na qual o bloco pertence
    private IObjectPool<Block> _pool;
    public void SetPool(IObjectPool<Block> pool) => _pool = pool;

    // LayerMask dos blocos
    public LayerMask blockLayer;

    private void Awake()
    {
        powerUpSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Renderer = GetComponent<MeshRenderer>();
        material = Renderer.material;
    }

    // Fun��o que define um estado inicial para o bloco (Chamada quando o o bloco � ativado na pool)
    public void RestartBlock(Color _color, int _scoreValue)
    {
        color = _color;
        scoreValue = _scoreValue;
        material.color = color;
    }

    // Fun��o que quebra o bloco, ativando o power up, caso ele tenha um, e desativando ele na pool caso ele perten�a a uma ou destruindo ele caso contr�rio
    public void Break(Ball ball, bool isExplosive, out int _blockCount)
    {
        Break(ball, isExplosive);
        _blockCount = blockCount;
    }
    public void Break(Ball ball, bool isExplosive)
    {
        ScoresManager.Instance.AddScore(scoreValue);
        blockCount--;

        if (hasPowerUp)
            powerUpEffect.Apply(ball);

        if (isExplosive)
            Explode(ball);

        if (_pool != null)
            _pool.Release(this);
        else
            Destroy(gameObject);
    }

    // Fun��o que explode os blocos pr�ximos ao acertado
    void Explode(Ball ball)
    {
        Ray[] rays = {
            new Ray(transform.position, Vector3.up),
            new Ray(transform.position, Vector3.down),
            new Ray(transform.position, Vector3.left),
            new Ray(transform.position, Vector3.right)
        };
        RaycastHit hit;

        float distance = transform.lossyScale.y;
        for (int i = 0; i < rays.Length; i++)
        {
            if (i >= 2)
                distance = transform.lossyScale.x;
            if (Physics.Raycast(rays[i], out hit, distance, blockLayer))
            {
                hit.collider.GetComponent<Block>().Break(ball, false);
            }
        }
    }
}
