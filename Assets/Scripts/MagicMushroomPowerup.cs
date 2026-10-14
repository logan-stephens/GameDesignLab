using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroomPowerup : BasePowerup
{
    public AudioClip spawnSound;
    public AudioClip collectSound;

    private Collider2D mushroomCollider;

    // setup this object's type
    // instantiate variables
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.MagicMushroom;

        mushroomCollider = GetComponentInChildren<Collider2D>();
        mushroomCollider.enabled = false;
        rigidBody.bodyType = RigidbodyType2D.Static;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && spawned)
        {
            PlayerMovement mario = col.gameObject.GetComponent<PlayerMovement>();

            if (mario != null)
                mario.RequestPowerupEffect(this);

            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position);

            DestroyPowerup();
        }
        else if (col.gameObject.layer == 10) // else if hitting Pipe, flip travel direction
        {
            if (spawned)
            {
                goRight = !goRight;
                rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);

            }
        }
    }

    // interface implementation
    public override void SpawnPowerup()
    {
        spawned = true;

        mushroomCollider.enabled = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;

        if (spawnSound != null)
            AudioSource.PlayClipAtPoint(spawnSound, Camera.main.transform.position);

        StartCoroutine(MoveMushroom());
    }

    private IEnumerator MoveMushroom()
    {
        yield return null; // Wait one frame for physics update
        rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse);
    }


    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        PlayerMovement mario = i as PlayerMovement;

        if (mario != null)
            mario.ActivateMushroom();
    }
}