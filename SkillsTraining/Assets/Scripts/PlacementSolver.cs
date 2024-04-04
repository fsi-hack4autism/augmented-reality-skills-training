using MixedReality.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlacementSolver : Solver
{
    [SerializeField]
    private FloorFinder FloorFinder;

    [SerializeField]
    private float DefaultPlacementDistance = 1.5f;

    [SerializeField]
    private float MaxRaycastDistance = 20.0f;

    [SerializeField]
    private float SurfaceNormalOffset = 0.0f;

    [SerializeField]
    private float FloatingOffset = 0.0f;

    [SerializeField]
    private LayerMask[] MagneticSurfaces = { UnityEngine.Physics.DefaultRaycastLayers };

    [SerializeField]
    private UnityEvent onPlacingStopped = new UnityEvent();

    public UnityEvent OnPlacingStopped => onPlacingStopped;

    private RayStep currentRay;

    private bool didHitSurface;

    private RaycastHit currentHit;

    private XRInteractionManager interactionManager;

    private List<IXRInteractor> interactorsCache;

    private bool isPlacing = false;

    public void StartPlacement()
    {
        isPlacing = true;

        SolverHandler.UpdateSolvers = true;

        RegisterPlacementAction();

        FloorFinder.StartFindingFloor();
    }

    public void StopPlacement()
    {
        isPlacing = false;

        SolverHandler.UpdateSolvers = false;

        FloorFinder.StopFindingFloor();

        onPlacingStopped?.Invoke();
    }

    /// <inheritdoc/>
    public override void SolverUpdate()
    {
        if (isPlacing == false)
            return;

        // Make sure the Transform target is not null, added for the case where auto start is true 
        // and the tracked target type is the controller ray, if the hand is not in the frame we cannot
        // calculate the position of the object
        if (SolverHandler.TransformTarget != null)
        {
            PerformRaycast();
            SetPosition();
            SetRotation();
        }
    }

    private void PerformRaycast()
    {
        // The transform target is the transform of the TrackedTargetType, i.e. Controller Ray, Head or Hand Joint
        Transform transform = SolverHandler.TransformTarget;

        Vector3 origin = transform.position;
        Vector3 endpoint = transform.position + transform.forward;
        currentRay.UpdateRayStep(in origin, in endpoint);

        // Check if the current ray hits a magnetic surface
        didHitSurface = MixedRealityRaycaster.RaycastSimplePhysicsStep(currentRay, MaxRaycastDistance, MagneticSurfaces, false, out currentHit);
    }

    private void SetPosition()
    {
        if (FloorFinder.HasValidWorldHeight)
        {
            if (didHitSurface)
            {
                Vector3 floorPosition = currentHit.point;
                floorPosition.y = FloorFinder.FloorWorldHeight;

                if (Vector3.Angle(Vector3.up, currentHit.normal) > 85.0f)
                {
                    floorPosition += currentHit.normal * SurfaceNormalOffset;
                }

                GoalPosition = floorPosition;
            }
            else
            {
                Vector3 gazeEndPoint = SolverHandler.TransformTarget.position + (SolverHandler.TransformTarget.forward * DefaultPlacementDistance);
                gazeEndPoint.y = FloorFinder.FloorWorldHeight;

                GoalPosition = gazeEndPoint;
            }
        }
        else
        {
            if (didHitSurface)
            {
                Vector3 floatingHitPosition = currentHit.point;
                floatingHitPosition.y += FloatingOffset;

                if (Vector3.Angle(Vector3.up, currentHit.normal) > 85.0f)
                {
                    floatingHitPosition += currentHit.normal * SurfaceNormalOffset;
                }

                GoalPosition = floatingHitPosition;

                // TODO: Figure out if this is better than baking the offest in above
                //AddOffset(Vector3.up * FloatingOffset);
            }
            else
            {
                Vector3 gazeEndPoint = SolverHandler.TransformTarget.position + (SolverHandler.TransformTarget.forward * DefaultPlacementDistance);
                gazeEndPoint.y += FloatingOffset;

                GoalPosition = gazeEndPoint;
            }
        }
    }

    private void SetRotation()
    {
        if (didHitSurface &&
            Vector3.Angle(Vector3.up, currentHit.normal) > 85.0f)
        {
            Vector3 surfaceNormal = currentHit.normal;
            surfaceNormal.y = 0.0f;

            GoalRotation = Quaternion.LookRotation(-surfaceNormal, Vector3.up);
        }
        else
        {
            Vector3 direction = currentRay.Direction;
            direction.y = 0;

            GoalRotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    private void RegisterPlacementAction()
    {
        // Refresh the registeration if they already exist
        UnregisterPlacementAction();

        if (interactionManager == null)
        {
            interactionManager = ComponentCache<XRInteractionManager>.FindFirstActiveInstance();
            if (interactionManager == null)
            {
                Debug.LogError("No interaction manager found in scene. Please add an interaction manager to the scene.");
            }
        }

        if (interactorsCache == null)
        {
            interactorsCache = new List<IXRInteractor>();
        }

        // Try registering for the controller's "action" so object selection isn't required for placement.
        // If no controller, then fallback to using object selections for placement.
        interactionManager.GetRegisteredInteractors(interactorsCache);
        foreach (IXRInteractor interactor in interactorsCache)
        {
            if (interactor is XRBaseControllerInteractor controllerInteractor &&
                controllerInteractor.xrController is ActionBasedController actionController)
            {
                actionController.selectAction.action.performed += StopPlacementViaPerformedAction;
            }
            else if (interactor is IXRSelectInteractor selectInteractor)
            {
                selectInteractor.selectEntered.AddListener(StopPlacementViaSelect);
            }
        }
    }

    private void UnregisterPlacementAction()
    {
        if (interactorsCache != null)
        {
            foreach (IXRInteractor interactor in interactorsCache)
            {
                if (interactor is XRBaseControllerInteractor controllerInteractor &&
                    controllerInteractor.xrController is ActionBasedController actionController)
                {
                    actionController.selectAction.action.performed -= StopPlacementViaPerformedAction;
                }
                else if (interactor is IXRSelectInteractor selectInteractor)
                {
                    selectInteractor.selectEntered.RemoveListener(StopPlacementViaSelect);
                }
            }
            interactorsCache.Clear();
        }
    }

    private void StopPlacementViaPerformedAction(InputAction.CallbackContext context)
    {
        StopPlacement();
    }

    /// <summary>
    /// Stop the placement of a game object via an interactor's select event.
    /// </summary>
    private void StopPlacementViaSelect(SelectEnterEventArgs args)
    {
        StopPlacement();
    }
}
