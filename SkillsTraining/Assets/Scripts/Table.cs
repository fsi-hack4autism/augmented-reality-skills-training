using MixedReality.Toolkit.SpatialManipulation;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Table : MonoBehaviour
{
    [SerializeField]
    private ARMeshManager ARMeshManager;

    [SerializeField]
    private GameObject VisualRoot;

    [SerializeField]
    private Collider PlacementCollider;

    [SerializeField]
    private List<Collider> TableColliders;

    [SerializeField]
    private TapToPlace TapToPlace;

    [SerializeField]
    private GameObject[] PlaceableObjects;

    [SerializeField]
    private Transform[] PlaceablesSpawnPoints;

    private Action _placementDoneCallback = null;

    private void Start()
    {
        VisualRoot.SetActive(false);
    }

    public void StartPlacement(Action placementDoneCallback)
    {
        ARMeshManager.gameObject.SetActive(true);

        VisualRoot.SetActive(true);

        PlacementCollider.enabled = true;

        foreach (Collider tableColliders in TableColliders)
        {
            tableColliders.enabled = false;
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

        foreach (Collider tableColliders in TableColliders)
        {
            tableColliders.enabled = true;
        }

        SpawnPlaceables();

        _placementDoneCallback?.Invoke();
        _placementDoneCallback = null;
    }

    private void SpawnPlaceables()
    {
        int numPlacables = PlaceableObjects.Length;

        if (numPlacables == 0)
            return;

        for (int i = 0; i < PlaceablesSpawnPoints.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, numPlacables);

            GameObject placeablePrefab = PlaceableObjects[randomIndex];

            GameObject placeable = Instantiate(placeablePrefab, PlaceablesSpawnPoints[i]);
        }
    }
}
