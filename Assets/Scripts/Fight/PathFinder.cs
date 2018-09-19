using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinder
{

    NavMeshPath m_NavMeshPath = new NavMeshPath();
    public Vector3[] corners { get { return m_NavMeshPath.corners; } }
    int cornerIndex = 0;
    State state = State.Idle;

    Transform transform { get { return owner.transform; } }
    ActorBase owner;

    public PathFinder(ActorBase actor)
    {

        var list = new List<int>();
        owner = actor;
    }

    public bool CalculatePath(Vector3 startPosition, Vector3 endPosition, int mask = NavMesh.AllAreas)
    {
        return NavMesh.CalculatePath(startPosition, endPosition, mask, m_NavMeshPath);
    }

    public bool IsReach(Vector3 currentPosition, float measure)
    {
        if (corners != null && corners.Length > 0)
        {
            return Vector3.Distance(corners[corners.Length - 1], currentPosition) <= measure;
        }
        else
        {
            return false;
        }
    }

    public void MoveTo(Vector3 position)
    {
        if (CalculatePath(this.transform.position, position, NavMesh.AllAreas))
        {
            cornerIndex = 0;
            state = State.Move;
        }
        else
        {
            state = State.Idle;
        }
    }

    public void Update()
    {
        if (state == State.Move)
        {
            var direction = Vector3.Normalize(corners[cornerIndex] - this.transform.position);
            transform.forward = direction;
            transform.position += direction * owner.speed;

            if (Vector3.Distance(corners[cornerIndex], transform.position) <= Mathf.Pow(owner.speed * Time.deltaTime, 2))
            {
                cornerIndex++;
            }

            if (cornerIndex >= corners.Length)
            {
                state = State.Idle;
                cornerIndex = 0;
            }
        }
    }

    enum State
    {
        Idle,
        Move,
    }

}
