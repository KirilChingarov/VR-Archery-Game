using System.Collections;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [Header("References")]
    private PullInteractable bow;
    private Rigidbody rigidbody;

    [Header("Settings")]
    public float speed = 10f;

    private bool isAirborne = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        Stop();
    }

    public void RegisterBow(PullInteractable bow)
    {
        this.bow = bow;
        this.bow.OnRelease += Release;
    }

    private void Release(float pullAmount)
    {
        bow.OnRelease -= Release;
        gameObject.transform.parent = null;
        isAirborne = true;
        SetPhysics(true);

        Vector3 force = transform.forward * speed * pullAmount;
        rigidbody.AddForce(force, ForceMode.Impulse);
        StartCoroutine(RotateWithVelocity());
    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (isAirborne)
        {
            Quaternion newRotation = Quaternion.LookRotation(rigidbody.linearVelocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }

    private void Stop()
    {
        isAirborne = false;
        SetPhysics(false);
    }

    private void SetPhysics(bool usePhysics)
    {
        rigidbody.useGravity = usePhysics;
        rigidbody.isKinematic = !usePhysics;
    }
}
