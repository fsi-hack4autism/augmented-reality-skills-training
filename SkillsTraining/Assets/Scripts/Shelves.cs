using MixedReality.Toolkit.SpatialManipulation;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Shelves : MonoBehaviour
{
    [SerializeField]
    private ARMeshManager ARMeshManager;

    [SerializeField]
    private Collider PlacementCollider;

    [SerializeField]
    private List<Collider> ShelvesColliders;

    [SerializeField]
    private TapToPlace TapToPlace;

    private Action _placementDoneCallback = null;

    public void StartPlacement(Action placementDoneCallback)
    {
        ARMeshManager.gameObject.SetActive(true);

        PlacementCollider.enabled = true;

        foreach (Collider shelfColliders in ShelvesColliders)
        {
            shelfColliders.enabled = false;
        }

        _placementDoneCallback += placementDoneCallback;

        TapToPlace.OnPlacingStopped.AddListener(OnPlacementStopped);
        TapToPlace.StartPlacement();
    }

    private void OnPlacementStopped()
    {
        TapToPlace.OnPlacingStopped.RemoveListener(OnPlacementStopped);

        ARMeshManager.DestroyAllMeshes();

        ARMeshManager.gameObject.SetActive(false);

        PlacementCollider.enabled = false;

        foreach (Collider shelfColliders in ShelvesColliders)
        {
            shelfColliders.enabled = true;
        }

        _placementDoneCallback?.Invoke();
    }
}
