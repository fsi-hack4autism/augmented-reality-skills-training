using UnityEngine;

public class AppFlowManager : MonoBehaviour
{
    private enum AppFlowState
    {
        Setup_PlaceShelves,
        Setup_PlaceTable,
        TaskExplanation,
        DoingTask,
        Evaluation
    }

    [SerializeField]
    private Shelves Shelves;

    [SerializeField]
    private Table Table;

    private AppFlowState _currentState = AppFlowState.Setup_PlaceShelves;

    // Start is called before the first frame update
    private void Start()
    {
        SwitchState(AppFlowState.Setup_PlaceShelves);
    }

    private void SwitchState(AppFlowState state)
    {
        _currentState = state;

        switch (_currentState)
        {
            case AppFlowState.Setup_PlaceShelves:
                Shelves.StartPlacement(OnShelfPlacementDone);
                break;
            case AppFlowState.Setup_PlaceTable:
                Table.StartPlacement(OnTablePlacementDone);
                break;
        }
    }

    private void OnShelfPlacementDone()
    {
        SwitchState(AppFlowState.Setup_PlaceTable);
    }

    private void OnTablePlacementDone()
    {
        SwitchState(AppFlowState.TaskExplanation);
    }
}
