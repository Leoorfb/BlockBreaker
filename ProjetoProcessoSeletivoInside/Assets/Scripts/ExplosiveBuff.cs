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
        ball.explosivePowerUps++;
        ball.explosiveTrail.emitting = true;
        ball.ballTrail.emitting = (false);

        if (coroutine != null)
            ball.StopCoroutine(coroutine);

        coroutine = DisableBuff(ball);
        ball.StartCoroutine(coroutine);

        ScoresManager.Instance.player.PlayerUsedPowerUp.Invoke(powerUpIcon.texture, duration);
    }

    // Corrotina que desativa o Power Up depois de um tempo
    IEnumerator DisableBuff(Ball ball)
    {
        yield return new WaitForSeconds(duration);
        ball.explosivePowerUps--;
        if (ball.explosivePowerUps <= 0)
        {
            ball.explosiveTrail.emitting = (false);
            if (ball.speedPowerUps <= 0)
            {
                ball.ballTrail.emitting = (true);
            }
        }
    }
}
