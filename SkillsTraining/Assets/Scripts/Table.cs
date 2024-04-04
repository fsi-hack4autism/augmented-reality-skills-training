using UnityEngine;

public class Table : PlaceableProp
{
    [SerializeField]
    private GameObject[] PlaceableObjects;

    [SerializeField]
    private Transform[] PlaceablesSpawnPoints;

    protected override void OnPlacementStopped()
    {
        SpawnPlaceables();
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
