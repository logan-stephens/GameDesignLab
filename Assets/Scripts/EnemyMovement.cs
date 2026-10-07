using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // private variables - position tracking
    private Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
    private float originalX;

    // private variables
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    private Animator animator;
    private Collider2D enemyCollider;
    private bool dead = false;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();

        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        startPosition = transform.localPosition;
        originalX = transform.position.x;
        ComputeVelocity();
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    // note that this is Update(), which still works but not ideal. See below.
    void FixedUpdate()
    {
        if (dead) return; // dead goombas can't walk!

        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {// move goomba
            Movegoomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (dead) return; 
        if (collision.gameObject.CompareTag("Player")) {
            // find the normal of contact
            Vector2 n = collision.GetContact(0).normal;
            if (n.y < -0.5f) Stomp(collision.gameObject);
        }
    }

    private void Stomp(GameObject player)
    {
        dead = true;

        // play death sound
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null) audio.Play();
        // disable collider - reset on gameReset?
        if (enemyCollider != null) enemyCollider.enabled = false;
        if (animator != null) animator.SetTrigger("stomp");

        StartCoroutine(DisableGoomba(0.5f));
    }

    private IEnumerator DisableGoomba(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }


    public void GameRestart()
    {
        gameObject.SetActive(true); dead = false; if (enemyCollider != null) enemyCollider.enabled = true;
        if (animator != null) animator.Play("chilling");

        // reset location
        transform.localPosition = startPosition;
        originalX = transform.position.x;
        moveRight = -1;
        ComputeVelocity();
    }

}