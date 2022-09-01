using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SpeedBuff")]
public class SpeedBuff : PowerUpEffect
{
    [Header("SpeedBuff Settings")]
    public float speedMultiplier;
    public float duration;

    public override void Apply(Ball ball)
    {
        PlayerController player = ScoresManager.Instance.player;

        player.speed *= speedMultiplier;
        Ball.speed *= speedMultiplier;

        ball.speedPowerUps++;
        ball.speedTrail.emitting = (true);
        ball.ballTrail.emitting = (false);

        player.StartCoroutine(DisableBuff(player, ball));
    }

    // Corrotina que desativa o Power Up depois de um tempo
    IEnumerator DisableBuff(PlayerController player, Ball ball)
    {
        yield return new WaitForSeconds(duration);

        ball.speedPowerUps--;
        if (ball.speedPowerUps <= 0)
        {
            ball.speedTrail.emitting = (false);
            if (ball.explosivePowerUps <= 0)
            {
                ball.ballTrail.emitting = (true);
            }
        }
        

        player.speed /= speedMultiplier;
        Ball.speed /= speedMultiplier;
    }
}
