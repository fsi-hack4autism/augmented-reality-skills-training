using GLTFast.Schema;
using System.Collections;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    [SerializeField]
    private string ObjectDesciption;

    [SerializeField]
    private Rigidbody Rigidbody;

    [SerializeField]
    private float PhysicsSettlingTime = 1.0f;

    public string Description => ObjectDesciption;

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

    private void OnTriggerEnter(Collider other)
    {
        PlacementLocation placementLocation = other.gameObject.GetComponent<PlacementLocation>();

        if (placementLocation != null)
        {
            placementLocation.AddPlacedObject(this);

            this.transform.parent = placementLocation.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlacementLocation placementLocation = other.gameObject.GetComponent<PlacementLocation>();

        if (placementLocation != null)
        {
            placementLocation.RemovePlacedObject(this);

            this.transform.parent = null;
        }
    }
}
