using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    EnemyStats enemyStats;
    public PursueState pursueState;
    public LayerMask detectionLayer;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();

    }
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyStats.isDead == true)
            return this;
        
            #region Handle Enemy Detection
            Collider[] other = Physics.OverlapSphere(enemyManager.transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < other.Length; i++)
            {
                CharacterStats characterStats = other[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    //reconocer aliados o enemigos

                    Vector3 targetDirection = characterStats.transform.position - enemyManager.transform.position;
                    float viewAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                    if (viewAngle > enemyManager.minDetectionAngle && viewAngle < enemyManager.maxDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;

                    }
                }
            }
            #endregion
            #region HandleSwitchState
            if (enemyManager.currentTarget != null)
            {
                return pursueState;
            }
            else
            {
                return this;
            }
            #endregion
        


    }
}
