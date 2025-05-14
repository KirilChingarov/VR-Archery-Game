using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PullInteractable : XRBaseInteractable
{
    [Space(10)]
    public Transform start;
    public Transform end;
    public GameObject notch;

    public Animator bowAnimator;

    private float CalculatePull(Vector3 pullPosition)
    {
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - pullPosition;
        float maxLength = targetDirection.magnitude;

        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullPosition, targetDirection) / maxLength;
        return Mathf.Clamp(pullValue, 0, 1);
    }

    private void UpdateStringPullAnimation(float pullAmount)
    {
        bowAnimator.Play("pull", 0, pullAmount);
    }
}
