using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PowerUps/ExplosiveBuff")]
public class ExplosiveBuff : PowerUpEffect
{
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

    IEnumerator DisableBuff(Ball ball)
    {
        yield return new WaitForSeconds(duration);
        ball.hasExplosivePowerUp = false;
    }
}
