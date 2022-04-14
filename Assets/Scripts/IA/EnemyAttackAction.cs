using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/Acciones/Ataques)")]

public class EnemyAttackAction : EnemyActions
{
    public int attackScore = 3;
    public float recoveryTime = 2;

    public float maxAttackAngle = 35;
    public float minAttackAngle = -35;

    public float minDistanceToAttack = 0;
    public float maxDistanceToAttack = 3;
}
