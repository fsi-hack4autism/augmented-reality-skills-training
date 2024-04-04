using TMPro;
using UnityEngine;

public class Shelves : PlaceableProp
{
    [SerializeField]
    private PlacementLocation[] PlacementLocations;

    [SerializeField]
    private TMP_Text PlacementDescriptionText;

    private void Update()
    {
        string placementDescription = "";

        foreach (PlacementLocation location in PlacementLocations)
        {
            placementDescription += location.GetPlacementDescription() + " ";
        }

        PlacementDescriptionText.text = placementDescription;
    }
}
