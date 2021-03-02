using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagAnimations : MonoBehaviour
{
    [SerializeField]
    private float attackDelay = 0.3f;
    Animator anim;
    private string currentState;
    private string PlayerIdle;
    private string PlayerRun;
    private string PlayerJump;
    private string PlayerAttack;
    private string PlayerFallFlat;
    private string PlayerFall;
    private string PlayerPull;
    private string PlayerPush;
    private string PlayerClimb;
    private string PlayerLedgeClimb;
    private string PlayerGotHit;
    private string PlayerGrapple;
    private string PlayerGrappleThrow;
    private string PlayerGrappleCancel;
    private bool isAttackPressed;
    private bool isAttacking;
    private bool isGrounded;
    private bool isJumpPressed;
    private int groundMask;
    //=====================================================
    // Start is called before the first frame update
    //=====================================================
    void Start()
    {
        anim = GetComponent<Animator>();
        groundMask = 1 << LayerMask.NameToLayer("Ground");
    }

    //=====================================================
    // Update is called once per frame
    //=====================================================
    void Update()
    {
        //Checking for inputs
        //space jump key pressed?
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPressed = true;
        }

        //space Atatck key pressed?
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            isAttackPressed = true;
        }
    }

    //=====================================================
    // Physics based time step loop
    //=====================================================
    private void FixedUpdate()
    {
        //check if player is on the ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundMask);

        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if (isGrounded && !isAttacking)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                ChangeAnimationState(PlayerJump);
            }
            else
            {
                ChangeAnimationState(PlayerIdle);
            }
        }

        //------------------------------------------

        //Check if trying to jump 
        if (isJumpPressed && isGrounded)
        {
            isJumpPressed = false;
            ChangeAnimationState(PlayerJump);
        }
        //attack
        if (isAttackPressed)
        {
            isAttackPressed = false;

            if (!isAttacking)
            {
                isAttacking = true;

                if (isGrounded)
                {
                    ChangeAnimationState(PlayerAttack);
                }
                Invoke("AttackComplete", attackDelay);
            }
        }
    }

    void AttackComplete()
    {
        isAttacking = false;
    }

    //=====================================================
    // mini animation manager
    //=====================================================
    void ChangeAnimationState(string newAnimation)
    {
        if (currentState == newAnimation) return;

        anim.Play(newAnimation);
        currentState = newAnimation;
    }
}
