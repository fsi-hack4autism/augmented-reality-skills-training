using System.Collections.Generic;
using UnityEngine;

public class PlacementLocation : MonoBehaviour
{
    [SerializeField]
    private string LocationDescription;

    public List<PlaceableObject> PlacedObjects => placedObjects; 

    private List<PlaceableObject> placedObjects = new List<PlaceableObject>();

    public void AddPlacedObject(PlaceableObject placedObject)
    {
        placedObjects.Add(placedObject);
    }

    public void RemovePlacedObject(PlaceableObject removedObject)
    {
        placedObjects.Remove(removedObject);
    }

    public string GetPlacementDescription()
    {
        if (placedObjects.Count == 0)
            return string.Empty;

        //The first item on the second shelf is a blue car
        //The second item on the second shelf is a blue car
        //The third item on the second shelf is an orange car
        //The fourth item on the second shelf is an orange car
        //The fifth item on the second shelf is a yellow car
        //The first item on the first shelf is a stuffed toy
        //The second item on the first shelf is a black book
        //The third item on the first shelf is a red book
        //The fourth item on the first shelf is a blue book
        //The First item on the third shelf is a blue dice

        string[] ordinals = new string[16]
        {
            "first",
            "second",
            "third",
            "fourth",
            "fifth",
            "sixth",
            "seventh",
            "eighth",
            "ninth",
            "tenth",
            "elventh",
            "twelveth",
            "thirteenth",
            "fourteenth",
            "fifteenth",
            "sixteenth"
        };

        string description = "On the " + LocationDescription + " there";

        if (placedObjects.Count <= 2)
        {
            description += " is a " + placedObjects[0].Description;

            if (placedObjects.Count == 2)
            {
                description += " and a " + placedObjects[1].Description;
            }

            description += ".";
        }
        else
        {
            description += " is ";

            for (int i = 0; i < placedObjects.Count; i++)
            {
                description += "a " + placedObjects[i].Description;

                if (i == placedObjects.Count - 1)
                {
                    description += ".";
                }
                else if (i == placedObjects.Count - 2)
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

    public List<string> GetFormattedDescriptions()
    {
        List<string> formattedDescriptions = new List<string>();

        //['userA puts Red Elephant on First Row', 'userA puts Red Elephant on Second Row' ,'userA puts Blue Car on Second Row']

        foreach (PlaceableObject placedObject in placedObjects)
        {
            string placedObjectDescription = "'userA puts " + placedObject.Description + " on " + LocationDescription + "'";

            formattedDescriptions.Add(placedObjectDescription);
        }

        return formattedDescriptions;
    }
}
