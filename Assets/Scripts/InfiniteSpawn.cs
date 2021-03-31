using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteSpawn : MonoBehaviour
{
    [SerializeField] private GameObject shape;
    [SerializeField] private float bottomGap = 10f;
    [SerializeField] private float topGap = 5f;
    private float availableScreenHeight;
    [Range(0f, 3f)] [SerializeField] private float screenScaleToAddOn = 2f;
    private Vector3 startingSpawnPoint;
    
    private List<VerticalObject> _verticalObjects;
    
    void Awake()
    {
        // Get ScreenHeight
        availableScreenHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        availableScreenHeight *= (2 + screenScaleToAddOn);
        
        Debug.Log("AvailableScreenHeight: "+ availableScreenHeight);

        // Determine the starting spawn point
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        startingSpawnPoint = new Vector3(0, bottomLeft.y);
        Debug.Log("StartingSpawnPoint: " + startingSpawnPoint);

        // Spawn
        while (availableScreenHeight > 0)
        {
            if (!AddObject(shape)) break;
        }
    }

    void Update()
    {
        // Check first object
        // If out of screen bounds
        
        // Remove
        
        // Add
    }

    private bool AddObject(GameObject objectPrefab)
    {
        VerticalObject verticalObject = new VerticalObject(objectPrefab, bottomGap, topGap);
        float objectVerticalSize = verticalObject.GetVerticalSize();

        if (availableScreenHeight - objectVerticalSize > 0)
        {
            availableScreenHeight -= objectVerticalSize;
            SpawnVerticalObject(verticalObject);
            return true;
        }

        return false;
    }

    private void SpawnVerticalObject(VerticalObject verticalObject)
    {
        // update spawn point to simulate bottom gap
        startingSpawnPoint.y += verticalObject.GetBottomGapSize();

        // spawn object
        GameObject objectToSpawn = verticalObject.GetObjectPrefab();
        float objectCenterY = verticalObject.GetObjectVerticalSize() / 2;
        Vector3 objectSpawnPosition = startingSpawnPoint + new Vector3(0, objectCenterY, 0);
        objectToSpawn.transform.position = objectSpawnPosition;
        Instantiate(objectToSpawn);

        // update spawn point
        startingSpawnPoint.y += objectCenterY * 2;

        // update spawn point to simulate top gap
        startingSpawnPoint.y += verticalObject.GetTopGapSize();
    }

    private void RemoveFirstObject()
    {
        VerticalObject objectToRemove = _verticalObjects[0];
        
        // update vertical spawn point by minusing object
        // add to area
        float verticalSize = objectToRemove.GetVerticalSize();
        availableScreenHeight += verticalSize;
        // Move spawn point down
        startingSpawnPoint.y -= verticalSize;
        
        Destroy(objectToRemove.GetObjectPrefab());

        _verticalObjects.RemoveAt(0);
    }

}
