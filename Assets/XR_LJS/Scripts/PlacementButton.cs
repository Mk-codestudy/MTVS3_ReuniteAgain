using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementButton : MonoBehaviour
{
    public int objectId;
    private Button button;
    private PlacementSystem placementSystem;

    private void Start()
    {
        button = GetComponent<Button>();
        placementSystem = FindObjectOfType<PlacementSystem>();

        if (button != null && placementSystem != null)
        {
            button.onClick.AddListener(() => placementSystem.ChangeSelectedObject(objectId));
        }
        else
        {
            Debug.LogError("Button or PlacementSystem not found!");
        }
    }
}

