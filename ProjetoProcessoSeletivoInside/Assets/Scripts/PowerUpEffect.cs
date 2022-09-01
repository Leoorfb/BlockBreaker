using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpEffect : ScriptableObject
{
    [Header("Power Up Settings")]
    // Chance do Power Up ser criado (entre 0 e 100)
    public float spawnChance;
    // Icone do Power Up
    public Sprite powerUpIcon;

    // Função que aplica o Power Up
    public abstract void Apply(Ball ball);
}
