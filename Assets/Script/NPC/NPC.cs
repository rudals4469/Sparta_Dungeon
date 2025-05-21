using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum AIState
{
    Idle,
    Wandering
}
public class NPC : MonoBehaviour
{
    [Header("Stats")]
    public float walkSpeed;
    public float runSpeed;
    
    [Header("AI")]
    private NavMeshAgent _agent;
    public float detectDistance;
    private AIState _aiState;
    
    [Header("Wander")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    private float _playerDistance;
    public float fieldOfView = 120f;
    private Animator _animator;
    private SkinnedMeshRenderer[] _meshRenderers;
    
    private Vector3 _wanderTarget;
    [SerializeField]private LineRenderer _lineRenderer;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        SetState(AIState.Wandering);
    }

    private void Update()
    {
        _playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);
        
        _animator.SetBool("Moving", _aiState != AIState.Idle);
        
        if (_lineRenderer != null)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _wanderTarget);
            _lineRenderer.startWidth = 0.05f;
            _lineRenderer.endWidth = 0.05f;
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _lineRenderer.startColor = Color.red;
            _lineRenderer.endColor = Color.red;
        }

        switch (_aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
        }
    }

    private void SetState(AIState state)
    {
        _aiState = state;

        switch (_aiState)
        {
            case AIState.Idle: 
                _agent.speed = walkSpeed;
                _agent.isStopped = true;
                break;
            case AIState.Wandering :
                _agent.speed = walkSpeed;
                _agent.isStopped = false;
                break;
        }

        _animator.speed = _agent.speed / walkSpeed;
    }

    void PassiveUpdate()
    {
        if (_agent == null || !_agent.isOnNavMesh)
        {
            return;
        }
           
        if (_aiState == AIState.Wandering && _agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke(nameof(WanderToNewLocation), Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }
    }

    void WanderToNewLocation()
    {
        if (_aiState != AIState.Idle) return;

        SetState(AIState.Wandering);

        Vector3 target = GetWanderLocation();
        NavMeshPath path = new NavMeshPath();

        if (_agent.CalculatePath(target, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            _agent.SetPath(path); // 최종적으로 경로 설정
            _wanderTarget = target;
        }
        else
        {
            Debug.Log("경로를 찾을 수 없음: 장애물 또는 제한된 지역");
            SetState(AIState.Idle);
        }
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;
        
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance,maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
        
        int i = 0;
        do
        {
            NavMesh.SamplePosition(
                transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)),
                out hit,
                maxWanderDistance,
                NavMesh.AllAreas);

            i++;
        }while (Vector3.Distance(transform.position, hit.position) < detectDistance && i < 30);

        _wanderTarget = hit.position;

        return hit.position;
    }
}
