using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] float moveSpeed = 0.5f;

    List<Waypoint> path = new List<Waypoint>();

    Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowRoad());
    }

    void FindPath()
    {
        path.Clear();

        GameObject road = GameObject.FindGameObjectWithTag("Road");
        foreach (Transform tile in road.transform)
        {
            Waypoint waypoint = tile.GetComponent<Waypoint>();
            if (!waypoint) continue;
            path.Add(tile.GetComponent<Waypoint>());
        }
    }

    // Places the gameObject at the start of the path
    void ReturnToStart()
    {
        transform.position = path[0].transform.position;
    }

    private void FinishRoad()
    {
        // TODO add more "specticle"
        gameObject.SetActive(false);
        enemy.BankPenalty();
    }

    IEnumerator FollowRoad()
    {
        // Follow waypoint
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

        FinishRoad();
    }
}
