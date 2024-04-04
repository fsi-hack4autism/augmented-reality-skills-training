using System;
using UnityEngine;

public class PlaceableProp : MonoBehaviour
{
    [SerializeField]
    private SpatialMeshManager SpatialMeshManager;

    [SerializeField]
    private PlacementSolver PlacementSolver;

    [SerializeField]
    private GameObject VisualRoot;

    private bool isPlacing = false;
    private Action placementDoneCallback = null;

    private void Awake()
    {
        VisualRoot.SetActive(false);
    }

    public void StartPlacement(Action placementDoneCallback)
    {
        OnPlacementStarted();

        if (isPlacing)
            return;

        this.placementDoneCallback = placementDoneCallback;

        isPlacing = true;

        VisualRoot.SetActive(true);

        SpatialMeshManager.EnableScanning();

        PlacementSolver.OnPlacingStopped.AddListener(OnSolverPlacementStopped);
        PlacementSolver.StartPlacement();
    }

    protected virtual void OnPlacementStarted() { }

    private void OnSolverPlacementStopped()
    {
        isPlacing = false;

        SpatialMeshManager.DisableScanning();

        PlacementSolver.OnPlacingStopped.RemoveListener(OnSolverPlacementStopped);

        OnPlacementStopped();

        placementDoneCallback?.Invoke();
        placementDoneCallback = null;
    }

    protected virtual void OnPlacementStopped() { }
}
