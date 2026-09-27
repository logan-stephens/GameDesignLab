using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    private Rigidbody2D marioBody;
    public JumpOverGoomba jumpOverGoomba;
    public Animator marioAnimator;
    public AudioSource marioAudio;
    public AudioClip marioDeath;
    public float deathImpulse = 15;
    public Transform gameCamera;

    public GameObject obstacles;

    // state
    [System.NonSerialized]
    public bool alive = true;

    void PlayDeathImpulse() {marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);}
    void PlayJumpSound() { marioAudio.PlayOneShot(marioAudio.clip);}



    void GameOverScene() {
        Time.timeScale = 0.0f;

        gameOverScoreText.text = "Score: " + jumpOverGoomba.score;
        GameOver.SetActive(true);
        HUDscore.SetActive(false);
    }


    // global variables
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // Start is called before the first frame update
    void Start(){
        marioSprite = GetComponent<SpriteRenderer>();


        // other instructions
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        GameOver.SetActive(false);
        gameOverScoreText.text = "Score: 0";

        marioAnimator.SetBool("onGround", onGroundState);

        
    }

    // Update is called once per frame
    void Update(){
              // toggle state
      if (Input.GetKeyDown("a") && faceRightState){
          faceRightState = false;
          marioSprite.flipX = true;
          if (marioBody.linearVelocity.x > 0.1f) marioAnimator.SetTrigger("onSkid");
      }

      if (Input.GetKeyDown("d") && !faceRightState){
          faceRightState = true;
          marioSprite.flipX = false;
          if (marioBody.linearVelocity.x < -0.1f) marioAnimator.SetTrigger("onSkid");
      }

      marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));

      // other instructions
    }


    public float upSpeed = 10;
    private bool onGroundState = true;

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
    void OnCollisionEnter2D(Collision2D col)
    {

        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state to create the Finite State Machine
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }


    // FixedUpdate is called 50 times a second
    public float maxSpeed = 20;

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        /// IF ALIVE
        if (alive) {



        // other instructions
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // check if it doesn't go beyond maxSpeed
            if (marioBody.linearVelocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
        }

        // stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            // stop
            marioBody.linearVelocity = Vector2.zero;
        }

        if (Input.GetKeyDown("space") && onGroundState){
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;

            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }



        }
        /// IF DEAD

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            Debug.Log("Collided with goomba!");

            marioAnimator.Play("mario-die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;

        }
    }


    // just added
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public GameObject HUDscore;
    public GameObject GameOver;
    public TextMeshProUGUI gameOverScoreText;
    // other methods

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    

    public void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }
        foreach (Transform eachChild in obstacles.transform)
        {
            // HANDLE QUESTION BLOCK EXCLUSIVELY
            Block block = eachChild.GetComponentInChildren<Block>();
            if (block != null) block.ResetGame();

            // HANDLE OTHER STUFF
        }
        // reset score
        gameOverScoreText.text = "Score: 0";
        GameOver.SetActive(false);
        HUDscore.SetActive(true);
        jumpOverGoomba.score = 0;

        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(0,0,-10);
    }

    

    

    
}