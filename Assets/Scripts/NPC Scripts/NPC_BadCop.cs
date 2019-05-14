using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_BadCop : MonoBehaviour
{
    private NavMeshAgent _agent;

    private float _checkDestinationTimer = 1.0f;
    private float _currentCheckTimer = 0.0f;

    //private float _journeyTime = 3.0f;

    private bool _movementActive = false;
    private bool _startTransitionFinal = false;

    private float _startTransitionTimer = 1.0f;
    private float _currentTransitionTImer = 0.0f;

    private eBadCopStates _currentState = eBadCopStates.NONE;

    private GameObject _player;

    public List<GameObject> badCopWaypoints;
    private float _checkPlayerTimer = 1.5f;
    private float _checkPlayerCurrentTimer = 0.0f;

    private Renderer _renderer;
    public LayerMask playerLayer;
    public LayerMask sideLayer;

    public float detectRadius = 35.0f;

    public enum eBadCopStates
    {
        NONE = 0,
        START = 1,
        FRONT_TOWER = 2,
        INSIDE_TOWER = 3,
        TOWER_FINAL = 4,
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        Debug.Assert(_agent != null);

        _player = GameObject.FindGameObjectWithTag("Player");

        Debug.Assert(_player != null);

        Debug.Assert((badCopWaypoints.Count >= 3));

        _renderer = GetComponentInChildren<Renderer>();

        Debug.Assert(_renderer != null);
    }

    void UpdateStates()
    {
        if (_startTransitionFinal)
        {
            _currentTransitionTImer += Time.deltaTime;

            if (_currentTransitionTImer >= _startTransitionTimer)
            {
                _movementActive = true;
                _startTransitionFinal = false;
                _agent.isStopped = true;
                _agent.enabled = false;
                transform.position = badCopWaypoints[3].transform.position;
                _agent.enabled = true;
            }
        }

        if (_movementActive)
        {
            _currentCheckTimer += Time.deltaTime;

            if (_currentCheckTimer >= _checkDestinationTimer)
            {
                _currentCheckTimer = 0.0f;

                if (Vector3.Distance(transform.position, _agent.destination) <= _agent.stoppingDistance)
                {
                    _movementActive = false;
                    _currentCheckTimer = 0.0f;

                    if(_currentState == eBadCopStates.INSIDE_TOWER)
                    {
                        _currentState = eBadCopStates.TOWER_FINAL;
                        _startTransitionFinal = true;
                    }
                    /// do all the other stuff
                }
            }
        }
    }

    private void Update()
    {
        DetectPlayer();
        UpdateStates();
    }

    void DetectPlayer()
    {
        if (_renderer.isVisible)
        {
            _checkPlayerCurrentTimer += Time.deltaTime;

            if (_currentState == eBadCopStates.INSIDE_TOWER)
                return;

            if (_movementActive)
                return;

            if (_checkPlayerCurrentTimer >= _checkPlayerTimer)
            {
                if (!CarryOutDetection())
                    return;

                _currentState++;

                if (_currentState == eBadCopStates.START)
                {
                    _movementActive = true;
                    _agent.SetDestination(badCopWaypoints[0].transform.position);
                }
                else if (_currentState == eBadCopStates.FRONT_TOWER)
                {
                    _movementActive = true;
                    _agent.SetDestination(badCopWaypoints[1].transform.position);
                }
                else if (_currentState == eBadCopStates.INSIDE_TOWER)
                {
                    _movementActive = true;
                    _agent.SetDestination(badCopWaypoints[2].transform.position);
                }
            }
        }
    }

    bool CarryOutDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius, playerLayer);

        bool isSeen = false;

        if (colliders.Length > 0)
        {
            foreach (Collider potentialCollider in colliders)
            {
                if (potentialCollider.CompareTag("Player"))
                {
                    if (CanPotentialTargetBeSeen(potentialCollider.transform))
                    {
                        if (IsTargetLookingAtThis(potentialCollider.gameObject.transform))
                        {
                            isSeen = true;
                            break;
                        }
                    }
                }
            }
        }

        return isSeen;
    }

    private bool CanPotentialTargetBeSeen(Transform potentialTarget)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, potentialTarget.position, out hit, sideLayer))
        {
            if (hit.transform == potentialTarget)
                return true;
        }

        return false;
    }

    private bool IsTargetLookingAtThis(Transform targetTransform)
    {
        float angle = 45.0f;

        if (Vector3.Angle(targetTransform.transform.forward, transform.position - targetTransform.position) <= angle)
            return true;

        return false;
    }
}
