using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private Vector2 startPos;
    private bool wasHit = false;
    public GameObject coin;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startPos = transform.position; // go back after wasHit
    }

    // after colliding with bounding box on the bottom
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!wasHit && col.CompareTag("Player")) Hit();
    }

    void Hit() {
        wasHit = true;
        body.AddForce(Vector2.up * 400f);

        // SHOW A COIN EITHER THROUGH A FUNCTION OR SOMETHING

        anim.SetTrigger("hit");
        // MAKE IT FREEZE IN PLACE SOMEHOW BUT ONLY AFTER A CERTAIN AMOUNT OF TIME
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // If gravity or Mario's weight tries to push the block below its starting height...
        if (body.position.y < startPos.y)
        {
            // Force it to stay exactly at the start position
            body.position = new Vector2(body.position.x, startPos.y);
            
            // Kill any downward velocity so the spring doesn't jitter
            body.linearVelocity = new Vector2(body.linearVelocity.x, Mathf.Max(0, body.linearVelocity.y));
        }
    }

    public void ResetGame()
    {
        wasHit = false; 
        transform.position = startPos; // may be unnecessary? idk can try deleting
        
        body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        anim.SetTrigger("ResetBlock");
    }
}
