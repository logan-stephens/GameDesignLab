using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
    private Rigidbody2D b;
    private Animator anim;
    private Vector2 startPos;
    private bool hit = false;
    private float hitTime = 0f; // DEATH TIMER COUNTDOWN THING

    [Header("Classification")]
    public bool isQuestionBox = true;
    public GameObject coinPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        b = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startPos = transform.position;

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (hit || !col.CompareTag("Player")) return;

        TriggerHit();
    }



    public void TriggerHit()
    {
        Debug.Log("Bottom hit");
        if (hit) return; hit = true;
        hitTime = Time.time; // DEATH TIMER USE CASE

        if (coinPrefab != null) SpawnCoin(); // if block has coin spawn it

        if (isQuestionBox) anim.SetTrigger("hit"); // only for qblock
    }





    void SpawnCoin()
    {
        if (coinPrefab != null) {
            // create coin
            GameObject coin = Instantiate(coinPrefab, transform);
            coin.transform.localPosition = Vector3.up; 
            // animate coin
            Animator coinAnim = coin.GetComponent<Animator>(); coinAnim.enabled = true;
            
            // ADD SOUND HERE TOO
        }
    }

    public void ResetGame()
    {
        hit = false;
        b.bodyType = RigidbodyType2D.Dynamic; // change body type to stop movement
        transform.position = startPos;
        b.linearVelocity = Vector2.zero;
        if (isQuestionBox) anim.SetTrigger("ResetBlock");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isQuestionBox && hit && // a question box is hit
            Time.time - hitTime > 0.5f && // and enough time passed
            Mathf.Abs(b.linearVelocity.y) < 0.05f && Mathf.Abs(transform.position.y - startPos.y) < 0.01f) // and it's barely moving
        {
            b.bodyType = RigidbodyType2D.Static; // then stop it
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}