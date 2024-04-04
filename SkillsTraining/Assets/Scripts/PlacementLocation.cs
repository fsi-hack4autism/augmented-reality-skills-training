using System.Collections.Generic;
using UnityEngine;

public class PlacementLocation : MonoBehaviour
{
    [SerializeField]
    private string LocationDescription;

    private List<PlaceableObject> PlacedObjects = new List<PlaceableObject>();

    public void AddPlacedObject(PlaceableObject placedObject)
    {
        PlacedObjects.Add(placedObject);
    }

    public void RemovePlacedObject(PlaceableObject removedObject)
    {
        PlacedObjects.Remove(removedObject);
    }

    public string GetPlacementDescription()
    {
        if (PlacedObjects.Count == 0)
            return string.Empty;

        string description = "On the " + LocationDescription + " there ";

        if (PlacedObjects.Count <= 2)
        {
            description += " is a " + PlacedObjects[0].Description;

            if (PlacedObjects.Count == 2)
            {
                description += " and a " + PlacedObjects[1].Description;
            }
        }
        else
        {
            description += " is ";

            for (int i = 0; i < PlacedObjects.Count; i++)
            {
                description += "a " + PlacedObjects[i].Description;

                if (i == PlacedObjects.Count - 1)
                {
                    description += ".";
                }
                else if (i == PlacedObjects.Count - 2)
                {
                    description += ", and ";
                }
                else
                {
                    description += ", ";
                }
            }
        }

        return description;
    }
}
