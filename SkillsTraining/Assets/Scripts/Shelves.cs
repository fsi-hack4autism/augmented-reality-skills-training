using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shelves : PlaceableProp
{
    [Serializable]
    public class Shelf
    {
        public string Name;

        public List<PlacementLocation> PlacementLocations;
    }

    [SerializeField]
    private PlacementLocation[] PlacementLocations;

    [SerializeField]
    private TMP_Text PlacementDescriptionText;

    //[SerializeField]
    //private TMP_Text FormattedDescriptionText;

    [SerializeField]
    private List<Shelf> Shelfs;

    private string[] ordinals = new string[16]
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

    private void Update()
    {
        PlacementDescriptionText.text = GetPlacementDescription();

        ////['userA puts Red Elephant on First Row', 'userA puts Red Elephant on Second Row' ,'userA puts Blue Car on Second Row']
        //string formattedDescription = "[";

        //List<string> formattedDescriptions = new List<string>();

        //foreach (PlacementLocation location in PlacementLocations)
        //{
        //    List<string> formattedLocationDescriptions = location.GetFormattedDescriptions();

        //    formattedDescriptions.AddRange(formattedLocationDescriptions);
        //}

        //for (int i = 0; i < formattedDescriptions.Count; i++)
        //{
        //    formattedDescription += formattedDescriptions[i];

        //    if (i < formattedDescriptions.Count - 1)
        //    {
        //        formattedDescription += ", ";
        //    }
        //}

        //formattedDescription += "]";

        //FormattedDescriptionText.text = formattedDescription;
    }

    public string GetPlacementDescription()
    {
        string placementDescription = "";

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

        foreach (Shelf shelf in Shelfs)
        {
            int itemCount = 0;

            foreach (PlacementLocation location in shelf.PlacementLocations)
            {
                foreach (PlaceableObject placedObject in location.PlacedObjects)
                {
                    string ordinal = ordinals[itemCount++];

                    placementDescription += "The " + ordinal + " item on " + shelf.Name + " is a " + placedObject.Description + ". ";
                }
            }
        }

        return placementDescription;
    }
}
