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

    public PathFinder(ActorBase _actor)
    {
        owner = _actor;
    }

    public bool ReCalculatePath(Vector3 _startPosition, Vector3 _endPosition, int _mask = NavMesh.AllAreas)
    {
        return NavMesh.CalculatePath(_startPosition, _endPosition, _mask, m_NavMeshPath);
    }

    public bool IsReach(Vector3 _currentPosition, float _measure)
    {
        if (corners != null && corners.Length > 0)
        {
            return Vector3.Distance(corners[corners.Length - 1], _currentPosition) <= _measure;
        }
        else
        {
            return false;
        }
    }

    public void MoveTo(Vector3 _endPosition)
    {
        if (ReCalculatePath(this.transform.position, _endPosition, NavMesh.AllAreas))
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
