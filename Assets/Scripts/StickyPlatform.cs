using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    // when we collide with the box collider with is trigger (only the top surface)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if this object (the platform that this script is attached to) collides with an object with the name "Player"
        if (collision.gameObject.name == "Player")
        {
            // set "Player" as the child of the transform component of this platform (parent)
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
