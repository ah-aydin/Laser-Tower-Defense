using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0, 5)] float moveSpeed = 0.5f;

    private void Start()
    {
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        foreach(Waypoint waypoint in path)
        {
            Vector3 currentPos = transform.position;
            Vector3 nextPosition = waypoint.transform.position;
            float travelPercent = 0f;

            transform.LookAt(nextPosition);

            while (travelPercent < 1f)
            {
                travelPercent += moveSpeed * Time.deltaTime;
                transform.position = Vector3.Lerp(currentPos, nextPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
