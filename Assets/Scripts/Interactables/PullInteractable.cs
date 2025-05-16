using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PullInteractable : XRBaseInteractable
{
    [Space(10)]
    [Header("References")]
    public Transform start;
    public Transform end;
    public Transform notch;
    
    [HideInInspector]
    public event Action<float> OnRelease;
    
    private LineRenderer _bowString;
    private IXRSelectInteractor _pullInteractor = null;

    protected override void Awake()
    {
        base.Awake();
        _bowString = gameObject.transform.parent.GetComponentInChildren<LineRenderer>();
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {
        _pullInteractor = args.interactorObject;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic && isSelected)
        {
            float pullAmount = CalculatePullAmount(_pullInteractor.transform.position);
            UpdateStringPull(pullAmount);
        }
    }

    public void Release()
    {
        // Broadcast release
        float onReleasePullAmount = CalculatePullAmount(_pullInteractor.transform.position);
        OnRelease.Invoke(onReleasePullAmount);

        _pullInteractor = null;
        UpdateStringPull(0);
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

    private void UpdateStringPull(float pullAmount)
    {
        Vector3 pullPosition = Vector3.forward * Mathf.Lerp(start.localPosition.z, end.localPosition.z, pullAmount);
        notch.localPosition = new Vector3(notch.localPosition.x, notch.localPosition.y, pullPosition.z);
        _bowString.SetPosition(1, pullPosition);
    }
}
