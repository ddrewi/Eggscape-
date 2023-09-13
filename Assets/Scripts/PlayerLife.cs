using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rb;
    private int numCherries;
    private ItemCollector ic;
    private PlayerMovement pm;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Debug.Log("Trap Hit", this);
            spikeHit();
        }
        else if (collision.gameObject.CompareTag("MovingTrap"))
        {
            Debug.Log("Moving Trap Hit", this);
            SawHit();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Hit", this);
            EnemyHit();
                
        }
    }

    

    private void Die()
    {
        // make sure that player cannot move anymore
        rb.bodyType = RigidbodyType2D.Static;
        // trigger death animation
        anim.SetTrigger("death");
        
    }
   
    // this method is called through our death animation (check Animation window)
    private void RestartLevel()
    {
        // this method reloads the ACTIVE scene...
        // do we want to change this for Eggscape?
        // where does the player respawn?
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ItemCollector.setCherries(0);
    }

    private void loseCherries()
    {
        if(ItemCollector.getCherries() == 1)
        {
            ItemCollector.setCherries(0);
        }
        else
        {
            ItemCollector.setCherries(ItemCollector.getCherries() / 2);
        }
    }

    public void spikeHit()
    {
        Hit();
        rb.velocity = new Vector2(rb.velocity.x, 10f);
    }
    public void EnemyHit()
    {
        BroadcastMessage("BounceBack");
        Hit();
        rb.velocity = new Vector2(-1 * rb.velocity.x, 5f);
    }
    private void SawHit()
    {
        BroadcastMessage("BounceBack");
        Hit();
        rb.velocity = new Vector2(-1 * rb.velocity.x, 5f);
    }
    public void Hit()
    {
        loseCherries();
        if (ItemCollector.getCherries() == 0)
        {
            Die();
        }
    }
}
