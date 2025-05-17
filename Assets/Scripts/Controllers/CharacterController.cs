using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CharacterController : MonoBehaviour
{
    private NavMeshAgent _agent;
    public Transform target;
    
    [Header("Animation")]
    public Animator animator;
    private const string IdleAnimationStateName = "Idle";
    private const string RunAnimationStateName = "Run";
    private const string DeathAnimationStateName = "Death";
    private readonly string[] _attackAnimations = {"Attack01", "Attack02"};

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(target.position);
    }

    // MARK: Animations
    private void PlayIdleAnimation()
    {
        PlayAnimation(IdleAnimationStateName);
    }

    private void PlayRunAnimation()
    {
        PlayAnimation(RunAnimationStateName);
    }

    private void PlayDeathAnimation()
    {
        PlayAnimation(DeathAnimationStateName);
    }

    private void PlayAttackAnimation()
    {
        var attackStateName = _attackAnimations[Random.Range(0, _attackAnimations.Length)];
        PlayAnimation(attackStateName);
    }

    private void PlayAnimation(string stateName)
    {
        animator.Play(stateName);
    }
}
