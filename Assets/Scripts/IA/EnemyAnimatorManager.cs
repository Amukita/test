using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyLocomotionManager enemyLocomotionManager;

    public AudioClip monsterDamaged;
    private AudioSource audioSource;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
        audioSource = GetComponent<AudioSource>();

    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyLocomotionManager.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyLocomotionManager.enemyRigidbody.velocity = velocity;
    }

    private void MonsterSound()
    {
        audioSource.clip = monsterDamaged;
        audioSource.Play();
    }
}
