using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorKeyUI : MonoBehaviour
{




    [SerializeField] private Image image;


    private bool isCollected;

    // Start is called before the first frame update
    void Start()
    {
        isCollected = false;
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ItemCollector.IsKeyCollected())
        {
            CollectImage();
        }

    }

    private void CollectImage()
    {
        isCollected = true;
        image.enabled = true;
    }

}
