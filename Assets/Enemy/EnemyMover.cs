using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] float moveSpeed = 0.5f;

    List<Node> path = new List<Node>();

    Enemy enemy;
    GridManager gridManager;
    Pathfinder pathfinder;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    private void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    private void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        if (resetPath)
        {
            coordinates = pathfinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();
        path.Clear();
        path = pathfinder.GetNewPath(coordinates);
        StartCoroutine(FollowRoad());
    }

    // Places the gameObject at the start of the path
    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordiantes(pathfinder.StartCoordinates);
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
        for(int i = 2; i < path.Count; i++)
        {
            Vector3 currentPos = transform.position;
            Vector3 nextPosition = gridManager.GetPositionFromCoordiantes(path[i].coordinates);
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
