using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Actor
{
    public class PathFinder
    {
        NavMeshPath m_NavMeshPath = new NavMeshPath();
        public Vector3[] corners { get { return this.m_NavMeshPath.corners; } }
        int cornerIndex = 0;
        State state = State.Idle;

        Transform transform { get { return this.owner.transform; } }
        ActorBase owner;

        public PathFinder(ActorBase actor)
        {

            var list = new List<int>();
            this.owner = actor;
        }

        public bool CalculatePath(Vector3 startPosition, Vector3 endPosition, int mask = NavMesh.AllAreas)
        {
            return NavMesh.CalculatePath(startPosition, endPosition, mask, this.m_NavMeshPath);
        }

        public bool IsReach(Vector3 currentPosition, float measure)
        {
            if (this.corners != null && this.corners.Length > 0)
            {
                return Vector3.Distance(this.corners[this.corners.Length - 1], currentPosition) <= measure;
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
                this.cornerIndex = 0;
                this.state = State.Move;
            }
            else
            {
                this.state = State.Idle;
            }
        }

        public void Update()
        {
            if (this.state == State.Move)
            {
                var direction = Vector3.Normalize(this.corners[this.cornerIndex] - this.transform.position);
                this.transform.forward = direction;
                this.transform.position += direction * this.owner.speed;

                if (Vector3.Distance(this.corners[this.cornerIndex], this.transform.position) <= Mathf.Pow(this.owner.speed * Time.deltaTime, 2))
                {
                    this.cornerIndex++;
                }

                if (this.cornerIndex >= this.corners.Length)
                {
                    this.state = State.Idle;
                    this.cornerIndex = 0;
                }
            }
        }

        enum State
        {
            Idle,
            Move,
        }

    }
}

