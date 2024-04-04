using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpatialMeshManager : MonoBehaviour
{
    [SerializeField]
    private ARMeshManager ARMeshManager;

    [SerializeField]
    private bool AutoStart = true;

    [SerializeField]
    private Material ScanningMaterial;

    [SerializeField]
    private Material OcclusionMaterial;

    private void Awake()
    {
        ARMeshManager.gameObject.SetActive(AutoStart);
    }

    public void EnableScanning()
    {
        ARMeshManager.gameObject.SetActive(true);

        SetSpatialMeshMaterial(ScanningMaterial);
    }

    public void DisableScanning()
    {
        SetSpatialMeshMaterial(OcclusionMaterial);

        ARMeshManager.gameObject.SetActive(false);
    }

    private void SetSpatialMeshMaterial(Material material)
    {
        IList<MeshFilter> filters = ARMeshManager.meshes;

        foreach (MeshFilter filter in filters)
        {
            MeshRenderer meshRenderer = filter.gameObject.GetComponent<MeshRenderer>();

            meshRenderer.sharedMaterial = material;
        }
    }
}
