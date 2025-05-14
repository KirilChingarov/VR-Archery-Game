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
    public float pull = 0f;

    private IXRSelectInteractor pullInteractor = null;

    public void SetPullInteractor(SelectEnterEventArgs args)
    {
        Debug.Log(args.interactorObject.ToString());
        pullInteractor = args.interactorObject;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if(isSelected)
            {
                Vector3 pullPosition = pullInteractor.transform.position;
                float pullAmount = CalculatePullAmount(pullPosition);
                pull = pullAmount;
                UpdateStringPullAnimation(pullAmount);
            }
        }
    }

    public void Release()
    {
        Debug.Log("Released");
        pullInteractor = null;
        pull = 0f;
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
