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

        player.StartCoroutine(DisableBuff(player));
    }

    // Corrotina que desativa o Power Up depois de um tempo
    IEnumerator DisableBuff(PlayerController player)
    {
        yield return new WaitForSeconds(duration);
        player.speed /= speedMultiplier;
        Ball.speed /= speedMultiplier;
    }
}
