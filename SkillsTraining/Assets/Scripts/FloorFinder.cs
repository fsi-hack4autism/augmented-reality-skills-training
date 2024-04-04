using MixedReality.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;
using System.Collections.Generic;
using UnityEngine;

public class FloorFinder : MonoBehaviour
{
    [SerializeField]
    private SolverHandler SolverHandler;

    [SerializeField]
    private LayerMask[] FloorLayers = { UnityEngine.Physics.DefaultRaycastLayers };

    public bool HasValidWorldHeight => hasValidFloorHeight;
    public float FloorWorldHeight => floorHeight;

    private bool hasValidFloorHeight = false;
    private float floorHeight = 0;
    private int floorHeightCount = 0;

    private Vector3[] testPattern = new Vector3[25];

    private RayStep testRayStep;

    private bool findingFloor = false;

    private void Start()
    {
        int testPointIndex = 0;

        for (int y = -2; y <= 2; y++)
        {
            for (int x = -2; x <= 2; x++)
            {
                float gridStep = 0.25f;

                testPattern[testPointIndex] = new Vector3(x * gridStep, 0.0f, y * gridStep);

                testPointIndex++;
            }
        }
    }

    public void StartFindingFloor()
    {
        findingFloor = true;
    }

    public void StopFindingFloor()
    {
        findingFloor = false;
    }

    private void Update()
    {
        if (findingFloor == false)
            return;

        Transform transform = SolverHandler.TransformTarget;

        List<float> potentialFloorHeights = new List<float>();

        foreach (Vector3 testPoint in testPattern)
        {
            Vector3 origin = transform.position + testPoint;
            Vector3 endpoint = origin + Vector3.down;

            testRayStep.UpdateRayStep(origin, endpoint);

#if UNITY_EDITOR
            Debug.DrawRay(origin, Vector3.down * 3.0f, Color.blue);
#endif // UNITY_EDITOR

            if (MixedRealityRaycaster.RaycastSimplePhysicsStep(testRayStep, 3.0f, FloorLayers, false, out RaycastHit hit))
            {
                if (Vector3.Angle(Vector3.up, hit.normal) < 3.0f)
                {
                    potentialFloorHeights.Add(hit.point.y);
                }
            }
        }

        EvaluatePotentialFloorHeights(potentialFloorHeights);
    }

    private class HeightRange
    {
        public float MinHeight;

        public int Count => count;

        public float AverageHeight
        {
            get
            {
                if (Count == 0)
                    return 0f;

                return heightSum / count;
            }
        }

        private float heightSum = 0f;
        private int count = 0;

        public void AddHeight(float height)
        {
            heightSum += height;

            count++;
        }
    }

    private void EvaluatePotentialFloorHeights(List<float> potentialFloorHeights)
    {
        // Easiest case
        if (hasValidFloorHeight == false && potentialFloorHeights.Count == 1)
        {
            hasValidFloorHeight = true;

            floorHeight = potentialFloorHeights[0];

            floorHeightCount = 1;

            return;
        }

        float lowestHeight = float.MaxValue;
        float highestHeight = float.MinValue;

        foreach (float potentialFloorHeight in potentialFloorHeights)
        {
            if (potentialFloorHeight < lowestHeight)
            {
                lowestHeight = potentialFloorHeight;
            }
            if (potentialFloorHeight > highestHeight)
            {
                highestHeight = potentialFloorHeight;
            }
        }

        List<HeightRange> heightRanges = new List<HeightRange>();

        float rangeStep = 0.05f;

        for (float minHeight = lowestHeight; minHeight <= highestHeight; minHeight += rangeStep)
        {
            HeightRange heightRange = new HeightRange();

            heightRange.MinHeight = minHeight;

            foreach (float potentialFloorHeight in potentialFloorHeights)
            {
                if (potentialFloorHeight >= minHeight && potentialFloorHeight < minHeight + rangeStep)
                {
                    heightRange.AddHeight(potentialFloorHeight);
                }
            }

            if (heightRange.Count > 0)
            {
                heightRanges.Add(heightRange);
            }
        }

        foreach (HeightRange heightRange in heightRanges)
        {
            if (hasValidFloorHeight == false)
            {
                floorHeight = heightRange.AverageHeight;
                floorHeightCount = heightRange.Count;

                hasValidFloorHeight = true;

                break;
            }

            if (heightRange.AverageHeight < floorHeight &&
                heightRange.Count >= floorHeightCount - 2)
            {
                floorHeight = heightRange.AverageHeight;
                floorHeightCount = heightRange.Count;
            }
        }
    }
}
