using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxPowerupController : MonoBehaviour, IPowerupController
{
    public Animator powerupAnimator;
    public BasePowerup powerup; // reference to this question box's powerup

    private Vector3 startPos;
    private Vector3 powerupStartPos;
    private Rigidbody2D boxBody;
    private Rigidbody2D powerupBody;
    private RigidbodyType2D boxStartType;
    private RigidbodyType2D powerupStartType;

    void Start()
    {
        startPos = transform.localPosition;
        powerupStartPos = powerup.transform.localPosition;

        boxBody = GetComponent<Rigidbody2D>();
        powerupBody = powerup.GetComponent<Rigidbody2D>();

        boxStartType = boxBody.bodyType;
        powerupStartType = powerupBody.bodyType;
    }

    public void GameRestart()
    {
        // Reset question box
        transform.localPosition = startPos;
        boxBody.bodyType = boxStartType;

        Animator boxAnimator = GetComponent<Animator>();
        boxAnimator.Rebind();
        boxAnimator.Update(0f);

        // Restore mushroom
        powerup.gameObject.SetActive(true);
        powerup.GetComponentInChildren<Collider2D>().enabled = false;
        powerup.transform.localPosition = powerupStartPos;
        powerup.spawned = false;
        

        powerupBody.bodyType = powerupStartType;
        powerupBody.bodyType = RigidbodyType2D.Static;
        powerupBody.linearVelocity = Vector2.zero;

        powerupAnimator.Rebind();
        powerupAnimator.Update(0f);
        powerup.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !powerup.hasSpawned)
        {
            // show disabled sprite
            this.GetComponent<Animator>().SetTrigger("hit");
            // spawn the powerup
            powerup.GetComponentInChildren<SpriteRenderer>().enabled = true;
            powerupAnimator.SetTrigger("spawned");
        }
    }

    // used by animator
    public void Disable()
    {
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.localPosition = new Vector3(0, 0, 0);
    }



}