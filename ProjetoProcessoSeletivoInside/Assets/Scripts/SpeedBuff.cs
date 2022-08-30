using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SpeedBuff")]
public class SpeedBuff : PowerUpEffect
{
    public float speedMultiplier;
    public float duration;

    public override void Apply(Ball ball)
    {
        PlayerController player = PlayerController.Instance;

        player.speed *= speedMultiplier;
        Ball.speed *= speedMultiplier;

        player.StartCoroutine(DisableBuff(player));
    }

    IEnumerator DisableBuff(PlayerController player)
    {
        yield return new WaitForSeconds(duration);
        player.speed /= speedMultiplier;
        Ball.speed /= speedMultiplier;
    }
}
