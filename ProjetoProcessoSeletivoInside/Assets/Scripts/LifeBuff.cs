using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PowerUps/LifeBuff")]
public class LifeBuff : PowerUpEffect
{
    public int lifeIncrease;
    public override void Apply(Ball ball)
    {
        GameMenuUI.Instance.AddLives(+lifeIncrease);
    }
}
