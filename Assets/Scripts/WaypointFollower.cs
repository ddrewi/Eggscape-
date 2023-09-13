using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    // use an array so that we can generalize this script for 2, 3, 4, etc. waypoints
    [SerializeField] private GameObject[] waypoints;
    // waypoint we are currently heading toward
    private int currentWaypointIndex = 0;

    // the speed at which the platform moves
    [SerializeField] private float speed = 2f;

    private void Update()
    {
        // update the position of the platform each frame
        // if the distance between the current waypoint destination and the moving platform is very small,
        // then we've reached the waypoint
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }
}
