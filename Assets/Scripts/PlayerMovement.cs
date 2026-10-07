using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    // unity variables
    public float speed = 10;
    public float maxSpeed = 20;
    public float upSpeed = 10;
    
    public Animator marioAnimator;

    public AudioSource marioAudio;
    public AudioSource marioDeathAudio;
    public AudioClip marioDeath;
    public float deathImpulse = 15;

    public GameManager gameManager;
    public Transform gameCamera;

    

    // private variables
    private SpriteRenderer marioSprite;
    private Rigidbody2D marioBody;
    private bool faceRightState = true;
    private bool jumpedState = false;
    private bool onGroundState = true;
    private bool moving = false;

    // state
    [System.NonSerialized]
    public bool alive = true;

    void PlayDeathImpulse() {marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);}
    void PlayJumpSound() { marioAudio.PlayOneShot(marioAudio.clip);}


    // called by AnimationEvent in mario-die
    void GameOverScene() { gameManager.GameOver(); }


    void Start(){
        marioSprite = GetComponent<SpriteRenderer>();
        marioBody = GetComponent<Rigidbody2D>();
        marioAnimator.SetBool("onGround", onGroundState);

        Application.targetFrameRate =  30; // 30 FPS
    }
    
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state to create the Finite State Machine
            marioAnimator.SetBool("onGround", onGroundState);
        }

        if (col.gameObject.CompareTag("Enemy") && alive)
        {
            // get & compare normal vector
            Vector2 n = col.GetContact(0).normal;
            if (n.y <= 0.5f) Die(); // contact normal is pointing down
        }
    }



    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState ? 1 : -1);
        }
    }

    void Move(int value)
    {
        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.linearVelocity.magnitude < maxSpeed) marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0) moving = false;
        else {
            FlipMarioSprite(value);
            moving = true;
        }
    }

    

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false; jumpedState = true;

            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }


    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }

    public void Die()
    {
        if (!alive) return;

        alive = false;
        marioAnimator.Play("mario-die");
        marioDeathAudio.PlayOneShot(marioDeath);
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");

        GameRestart(); // reset everything

        Time.timeScale = 1.0f; // resume time
    }

    
    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);

        // reset sprite direction
        faceRightState = true; marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);
    }    
}