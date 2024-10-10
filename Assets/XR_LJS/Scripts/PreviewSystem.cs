using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    float previewYoffset = 0.06f;

    [SerializeField]
    GameObject cellIndicator;
    GameObject previewObject;

    [SerializeField]
    Material previewMaterialsPrefab;
    Material previewMasterialInstance;

    Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMasterialInstance = new Material(previewMaterialsPrefab);
        cellIndicator.SetActive(false);

    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int Size)
    {
        previewObject = Instantiate(prefab);
        preparePreview(previewObject);
        prepareCursor(Size);
        cellIndicator.SetActive(true);
    }

    private void prepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.GetComponentInChildren<Renderer>().material.mainTextureScale = size;
        }
    }

    private void preparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMasterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        Destroy(previewObject);
    }

    public void updatePostion(Vector3 position, bool validity)
    {
        MovePreview(position);
        MoveCursor(position);
        ApplyFeedback(validity);
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;

        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
        previewMasterialInstance.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(
            position.x, 
            position.y + previewYoffset, 
            position.z);
    }
}
