using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PowerUps/LifeBuff")]
public class LifeBuff : PowerUpEffect
{
    [Header("LifeBuff Settings")]
    public int lifeIncrease;
    public override void Apply(Ball ball)
    {
        ScoresManager.Instance.player.damagePlayer(-lifeIncrease);
    }
}
