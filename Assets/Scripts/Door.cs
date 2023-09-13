using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

    [SerializeField] string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if we collide with an object with the Player tag
        if (collision.gameObject.CompareTag("Player") && ItemCollector.IsKeyCollected())
        {
            // Load new scene
            SceneManager.LoadScene(sceneName: nextScene);
        }
    }


    /*private bool PlayerInRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);


        RaycastHit2D hit = Physics2D.BoxCast(transform.position,transform, new Vector2());

        return hit.collider != null;
    }*/


    /*private void EnterDoor()
      {
        if (PlayerInRange())
        {
            // Change Level
        }
    }*/

}
