using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5.0f;
    public float jumpPow = 18.0f;
    public LayerMask groundLayer;

    //public Transform rightLeg;
    //public Transform leftLeg;

    bool goJump = false;
    bool jumpStop = false;
    bool onGround = false;
    float axisH = 0f;

    Rigidbody2D rbody;
    Animator animator;

    public enum PLAYERID{
        player1,
        player2,
        com,
        none
    }

    public enum GAMESTATE
    {
        playing,
        gameclear,
        gameover,
        gameend,
        wait
    }

    string horizontal;
    string jump;
    public PLAYERID playerID = PLAYERID.player1;
    public static GAMESTATE gameState;

    AudioSource soundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        if(playerID == PLAYERID.player1)
        {
            horizontal = "Horizontal";
            jump = "Jump";
        }
        else
        {
            horizontal = "Horizontal2";
            jump = "Jump2";
        }
        soundPlayer = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        gameState = GAMESTATE.playing;
    }

    // Update is called once per frame
    void Update()
    {
        axisH = 0;
        if(gameState == GAMESTATE.playing)
        {
            axisH = Input.GetAxisRaw(horizontal);
            if(Input.GetButtonDown(jump) && onGround)
            {
                Jump();
            }
            if(Input.GetButtonUp(jump) && !onGround && rbody.velocity.y > 0)
            {
                jumpStop = true;
            }
        }

        if(axisH * transform.localScale.x > 0) transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
    }

    void FixedUpdate()
    {
        onGround = Physics2D.Linecast(transform.position, transform.position -(transform.up * 0.1f), groundLayer);

        if(onGround || axisH != 0)
        {
            rbody.velocity = new Vector2(axisH * runSpeed, rbody.velocity.y);
        }

        if(goJump && onGround)
        {
            goJump = false;
            Vector2 jump = new Vector2(0, jumpPow);
            rbody.AddForce(jump, ForceMode2D.Impulse);
        }
        if(jumpStop)
        {
            StartCoroutine("JumpStop");
            jumpStop = false;
        }

        if(Mathf.Abs(axisH) > 0f) animator.SetBool("moving", true);
        else animator.SetBool("moving", false);

        animator.SetBool("onground", onGround);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "goal")
        {
            gameState = GAMESTATE.gameclear;
        }
        if(collision.gameObject.tag == "danger")
        {
            gameState = GAMESTATE.gameover;
        }
    }

    void Jump()
    {
        goJump = true;
    }

    IEnumerator JumpStop()
    {
        while(rbody.velocity.y >= 0)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y - 100.0f * Time.deltaTime);
            yield return null;
        }
    }
}
