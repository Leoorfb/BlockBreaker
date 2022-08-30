using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpEffect : ScriptableObject
{
    public float spawnChance;
    public Sprite powerUpIcon;

    public abstract void Apply(Ball ball);
}
