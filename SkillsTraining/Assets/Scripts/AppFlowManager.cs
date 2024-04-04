using TMPro;
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

    [SerializeField]
    private PlacementEvaluator PlacementEvaluator;

    [SerializeField]
    private GameObject EvaluationTextGameObject;

    [SerializeField]
    private TMP_Text EvaluationText;

    private AppFlowState _currentState = AppFlowState.Setup_PlaceShelves;

    // Start is called before the first frame update
    private void Start()
    {
        PlacementEvaluator.OnEvaluationResponse += OnPlacementEvaluationResponse;

        SwitchState(AppFlowState.Setup_PlaceShelves);
    }

    private void OnDestroy()
    {
        PlacementEvaluator.OnEvaluationResponse -= OnPlacementEvaluationResponse;
    }

    private void SwitchState(AppFlowState state)
    {
        _currentState = state;

        switch (_currentState)
        {
            case AppFlowState.Setup_PlaceShelves:
                EvaluationTextGameObject.SetActive(false);

                Shelves.StartPlacement(OnShelfPlacementDone);
                break;
            case AppFlowState.Setup_PlaceTable:
                Table.StartPlacement(OnTablePlacementDone);
                break;
            case AppFlowState.Evaluation:
                EvaluationText.text = "Waiting for response...";

                EvaluationTextGameObject.SetActive(true);

                string placementDescription = Shelves.GetPlacementDescription();

                PlacementEvaluator.StartEvaluation(placementDescription);
                break;
        }
    }

    private void OnShelfPlacementDone()
    {
        SwitchState(AppFlowState.Setup_PlaceTable);
    }

    private void OnTablePlacementDone()
    {
        //SwitchState(AppFlowState.TaskExplanation);
        SwitchState(AppFlowState.DoingTask);
    }

    public void OnPlacementTaskDone()
    {
        SwitchState(AppFlowState.Evaluation);
    }

    private void OnPlacementEvaluationResponse(string evaluationResponse)
    {
        EvaluationText.text = evaluationResponse;
    }
}
