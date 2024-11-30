using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;
    public PlayerWeaponController playerWeaponController;
    private AudioSource audioSource;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {

        SetAnimation();
    }
    public void SetAnimation()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);
        anim.SetBool("isDead", playerController.isDead);
        anim.SetBool("isAttack", playerController.isAttack);
        anim.SetInteger("weaponTag", (int)playerWeaponController.weaponAttack.weaponTag);
    }
    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }
    public void PlayAttack()
    {
        anim.SetTrigger("attack");
    }

    public void AttackSound()
    {
        audioSource.clip = Resources.Load<AudioClip>("Sound/PlayerAttack");
        audioSource.volume = 0.2f;
        audioSource.Play();
    }

    public void HurtSound()
    {
        audioSource.clip = Resources.Load<AudioClip>("Sound/PlayerOnHurt");
        audioSource.volume = 0.2f;
        audioSource.Play();
    }
}
