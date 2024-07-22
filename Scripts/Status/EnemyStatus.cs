using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatus", menuName = "Scriptable Object/EnemyStatus", order = int.MaxValue)]
public class EnemyStatus : ScriptableObject
{
    [SerializeField]
    float EnemyId = 0;
    public float GetEnemyId { get { return EnemyId; } }

    [SerializeField]
    float EnemyHP = 0;
    public float GetEnemyHP { get { return EnemyHP; } }

    [SerializeField]
    float EnemySpeed = 0;
    public float GetEnemySpeed { get { return EnemySpeed; } }

    [SerializeField]
    float EnemyAttackRange = 0;
    public float GetEnemyAttackRange { get { return EnemyAttackRange; } }

    [SerializeField]
    float EnemyAttackPower = 0;
    public float GetEnemyAttackPower { get { return EnemyAttackPower; } }

    [SerializeField]
    float EnemyAttackDelay = 0;
    public float GetEnemyAttackDelay { get { return EnemyAttackDelay; } }

}
