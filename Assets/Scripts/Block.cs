using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
    // unity variables (classification)
    [Header("Classification")]
    public bool isQuestionBox = true;
    public GameObject coinPrefab;

    // private variables
    private Rigidbody2D b;
    private Animator anim;
    private Vector2 startPos;
    private bool hit = false;
    private float hitTime = 0f; // static countdown
    GameManager gameManager;


    void Start()
    {
        b = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startPos = transform.position;

        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
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
        hitTime = Time.time; // begin countdown for turning static

        if (coinPrefab != null) SpawnCoin(); // if block has coin spawn it

        if (isQuestionBox) anim.SetTrigger("hit"); // only for qblock
    }


    void SpawnCoin()
    {
        if (coinPrefab != null) {
            // create coin
            GameObject coin = Instantiate(coinPrefab, transform);
            coin.transform.localPosition = Vector3.up; 

            // link tool to gameManager score
            AnimationEventIntTool tool = coin.GetComponent<AnimationEventIntTool>();
            if (tool != null && gameManager != null) tool.useInt.AddListener(gameManager.IncreaseScore);

            // animate coin
            Animator coinAnim = coin.GetComponent<Animator>(); coinAnim.enabled = true;
        }
    }

    public void GameRestart()
    {
        hit = false;
        b.bodyType = RigidbodyType2D.Dynamic; // change body type to stop movement
        transform.position = startPos;
        b.linearVelocity = Vector2.zero;

        // reset question boxes
        if (isQuestionBox && anim != null) { anim.Rebind(); anim.Update(0f); }
    }

    void FixedUpdate()
    {
        if (isQuestionBox && hit && // a question box is hit
            Time.time - hitTime > 0.5f && // and enough time passed
            Mathf.Abs(b.linearVelocity.y) < 0.05f && Mathf.Abs(transform.position.y - startPos.y) < 0.01f) // and it's barely moving
        {
            b.bodyType = RigidbodyType2D.Static; // then stop it
        }
    }
}