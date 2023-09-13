using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    static int cherries = 0;
    static bool keyCollected = false;

    [SerializeField] private TextMeshProUGUI cherriesText;


    private void Start()
    {
        keyCollected = false;
    }
    private void Update()
    {
        cherriesText.text = ":" + cherries;
    }


    // in combination with isTrigger, makes sure that collection does not cause collision (does not interfere with physics)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if we collide with an object with the Cherry tag
        if (collision.gameObject.CompareTag("Cherry"))
        {
            //remove the object from the screen and update text
            Destroy(collision.gameObject);
            setCherries(cherries + 1);
            cherriesText.text = "Cherries: " + cherries;
            Debug.Log("Cherries: " + cherries, this);
        }
        
        else if (collision.gameObject.CompareTag("Key"))
        {
            //remove the object from the screen
            Destroy(collision.gameObject);
            keyCollected = true;
            Debug.Log("Key Collected");
        }
    }

    public static int getCherries()
    {
        return cherries;
    }

    public static void setCherries(int newCherries)
    {
        cherries = (int) (Mathf.Min(newCherries, 20f));
        // herriesText.text = "Cherries: " + cherries;
    }

    public static bool IsKeyCollected()
    {
        return keyCollected;
    }


}
