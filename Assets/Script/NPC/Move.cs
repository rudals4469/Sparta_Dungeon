using UnityEngine;
using UnityEngine.AI;

public class MoveWithNavMesh : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed;

    private Vector3 target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            enabled = false;
            return;
        }

        agent.speed = speed;
        target = pointB.position;
        agent.SetDestination(target);
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            target = target == pointA.position ? pointB.position : pointA.position;
            agent.SetDestination(target);
        }
    }
}