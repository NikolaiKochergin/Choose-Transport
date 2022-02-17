using System.Collections;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _player.IdleBeginig += OnIdleBeginig;
        _player.StartedRun += StartRun;
        _player.StartedUseTransport += UseTransportAnimation;
        _player.Finished += Finished;
        _player.Failed += Failed;
        _player.StartedExitFromTransport += ExitFromTransport;
        _player.StartedFinishedMove += StartedFinishedMove;
    }

    private void OnDisable()
    {
        _player.IdleBeginig -= OnIdleBeginig;
        _player.StartedRun -= StartRun;
        _player.StartedUseTransport -= UseTransportAnimation;
        _player.Finished -= Finished;
        _player.Failed -= Failed;
        _player.StartedExitFromTransport -= ExitFromTransport;
        _player.StartedFinishedMove -= StartedFinishedMove;
    }

    private void OnIdleBeginig()
    {
        _animator.Play("Idle");
    }
    
    private void StartRun()
    {
        _animator.Play("Run");
    }
    private void UseTransportAnimation(string nameAnimation)
    {
        _animator.Play(nameAnimation);
    }

    private void Finished()
    {
        _animator.Play("Win");
    }

    private void Failed()
    {
        StartCoroutine(PlayFailWithDelay());
    }

    private void ExitFromTransport()
    {
        _animator.Play("TransportEscape");
    }

    private void StartedFinishedMove()
    {
        _animator.Play("FinishWalk");
    }

    private IEnumerator PlayFailWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        _animator.Play("Fail");
    }
}
