using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static  PlayerController instance;

    [Header("Moviemiento")]
    public float moveSpeed;

    [Header("Componentes")]
    public Rigidbody2D theRB;

    [Header("Salto")]
    private bool canDoubleJump;
    public float jumpForce;

    [Header("Grounded")]
    public Transform groundCheckPoint;
    
    public LayerMask whatIsGround;

    private bool isGrounded; 

    [Header("Animator")]
    public Animator anim;
    private SpriteRenderer theSR;


    public float knockBackLength, knockBackForce;
    private float knockBackCounter;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (knockBackCounter <= 0 ) {
            theRB.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, theRB.velocity.y);
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);
            if (isGrounded) {
                canDoubleJump = true;
            }
            if (Input.GetButtonDown("Jump")) {
                if (isGrounded) {
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                }
                else {
                    if (canDoubleJump) {
                        theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                        canDoubleJump = false;
                    }
                }
                
            }

            if (theRB.velocity.x < 0 ) {
                theSR.flipX = true;
            } else if ( theRB.velocity.x > 0 ) {
                theSR.flipX = false;
            }
        } else {
            knockBackCounter -= Time.deltaTime;
            if (!theSR.flipX) 
            {
                theRB.velocity = new Vector2(-knockBackForce, theRB.velocity.y);
            } else 
            {
                theRB.velocity = new Vector2(knockBackForce, theRB.velocity.y);
            }
        }
        anim.SetFloat("moveSpeed", Mathf.Abs(theRB.velocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }

    public void KnockBack() 
    {
        knockBackCounter = knockBackLength;
        theRB.velocity = new Vector2(0f, knockBackForce);
    }
}
