using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PowerUps/ExplosiveBuff")]
public class ExplosiveBuff : PowerUpEffect
{
    [Header("ExplosiveBuff Settings")]
    public float duration;
    IEnumerator coroutine;

    public override void Apply(Ball ball)
    {
        ball.hasExplosivePowerUp = true;

        if (coroutine != null)
            ball.StopCoroutine(coroutine);

        coroutine = DisableBuff(ball);
        ball.StartCoroutine(coroutine);
    }

    // Corrotina que desativa o Power Up depois de um tempo
    IEnumerator DisableBuff(Ball ball)
    {
        yield return new WaitForSeconds(duration);
        ball.hasExplosivePowerUp = false;
    }
}
