using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{


    [SerializeField] private float speed = .15f;
    private bool isCollected;
    // Start is called before the first frame update
    void Start()
    {
        isCollected = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 360 * speed * Time.deltaTime, 0);


    }


}
