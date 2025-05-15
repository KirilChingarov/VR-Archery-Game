using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PullInteractable : XRBaseInteractable
{
    [Space(10)]
    public Transform start;
    public Transform end;
    public GameObject notch;

    public Animator bowAnimator;

    public event Action<float> OnRelease;

    private IXRSelectInteractor pullInteractor = null;

    public void SetPullInteractor(SelectEnterEventArgs args)
    {
        pullInteractor = args.interactorObject;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic && isSelected)
        {
            float pullAmount = CalculatePullAmount(pullInteractor.transform.position);
            UpdateStringPullAnimation(pullAmount);
        }
    }

    public void Release()
    {
        // Broadcast release
        float onReleasePullAmount = CalculatePullAmount(pullInteractor.transform.position);
        OnRelease.Invoke(onReleasePullAmount);

        pullInteractor = null;
        UpdateStringPullAnimation(0);
    }

    private float CalculatePullAmount(Vector3 pullPosition)
    {
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;
        float maxLength = targetDirection.magnitude;

        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        return Mathf.Clamp(pullValue, 0, 1);
    }

    private void UpdateStringPullAnimation(float pullAmount)
    {
        bowAnimator.Play("pull", 0, pullAmount);
    }
}
