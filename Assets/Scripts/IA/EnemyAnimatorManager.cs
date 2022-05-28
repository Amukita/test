using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyManager enemyManager;

    public AudioClip swordSwing;
    public AudioClip monsterDamaged;
    private AudioSource audioSource;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyManager = GetComponentInParent<EnemyManager>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;

    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidbody.velocity = velocity;
        
    }

    public void MonsterSound()
    {
        audioSource.clip = monsterDamaged;
        audioSource.Play();
    }

    public void AttackSound()
    {
        audioSource.clip = swordSwing;
        audioSource.Play();
    }

}
