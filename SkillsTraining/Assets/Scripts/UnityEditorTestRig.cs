using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UnityEditorTestRig : MonoBehaviour
{
    [SerializeField]
    private GameObject[] EditorOnlyObjects;

    // Start is called before the first frame update
    void Start()
    {
        List<XRDisplaySubsystem> displaySubsystems = new List<XRDisplaySubsystem>();

        SubsystemManager.GetSubsystems(displaySubsystems);

        bool shouldShowEditorOnlyObjects = displaySubsystems.Count == 0;

        foreach (GameObject editorOnlyObject in EditorOnlyObjects)
        {
            editorOnlyObject.SetActive(shouldShowEditorOnlyObjects);
        }
    }
}
