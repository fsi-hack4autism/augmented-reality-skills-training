using System.Collections;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    [SerializeField]
    private Rigidbody Rigidbody;

    [SerializeField]
    private float PhysicsSettlingTime = 1.0f;

    private Coroutine _physicsSettlingCR = null;

    private void Awake()
    {
        Rigidbody.isKinematic = false;
    }

    public void OnObjectManipulationStarted()
    {
        Rigidbody.isKinematic = false;

        if (_physicsSettlingCR != null )
        {
            StopCoroutine(_physicsSettlingCR);
        }
    }

    public void OnObjectManipulationEnded()
    {
        Rigidbody.isKinematic = true;

        if (_physicsSettlingCR != null)
        {
            StopCoroutine(_physicsSettlingCR);
        }

        StartCoroutine(DisablePhysicsAfterSettling());
    }

    private IEnumerator DisablePhysicsAfterSettling()
    {
        yield return new WaitForSeconds(PhysicsSettlingTime);

        Rigidbody.isKinematic = false;

        _physicsSettlingCR = null;
    }
}
