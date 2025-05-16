using System;
using System.Collections;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    [Header("Settings")]
    public float speed = 10f;
    public Transform tip;

    
    private PullInteractable _bow;
    private Rigidbody _rigidbody;
    private bool _isAirborne = false;
    private Vector3 _lastPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Stop();
    }

    private void FixedUpdate()
    {
        if (!_isAirborne) return;
        
        CheckCollision();
        _lastPosition = tip.localPosition;
    }

    private void CheckCollision()
    {
        if (!Physics.Linecast(_lastPosition, tip.position, out RaycastHit hit)) return;
        print("Hit: " + hit.transform.gameObject.name);
        if (hit.transform.gameObject.layer != 10)
        {
            print("Wrong layer: " + hit.transform.gameObject.layer + ", searching for: " + 10);
            return;
        }
        
        if (hit.transform.TryGetComponent(out Rigidbody hitRigidbody))
        {
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            transform.parent = hit.transform;
            hitRigidbody.AddForce(_rigidbody.linearVelocity, ForceMode.Impulse);
        }
        else
        {
            print("No rigidbody");
        }
        Stop();
    }

    public void RegisterBow(PullInteractable bow)
    {
        _bow = bow;
        _bow.OnRelease += Release;
    }

    private void Release(float pullAmount)
    {
        _bow.OnRelease -= Release;
        gameObject.transform.parent = null;
        _isAirborne = true;
        SetPhysics(true);

        var force = transform.forward * speed * pullAmount;
        _rigidbody.AddForce(force, ForceMode.Impulse);
        StartCoroutine(RotateWithVelocity());
    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (_isAirborne)
        {
            var newRotation = Quaternion.LookRotation(_rigidbody.linearVelocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }

    private void Stop()
    {
        _isAirborne = false;
        SetPhysics(false);
    }

    private void SetPhysics(bool usePhysics)
    {
        _rigidbody.useGravity = usePhysics;
        _rigidbody.isKinematic = !usePhysics;
    }
}
