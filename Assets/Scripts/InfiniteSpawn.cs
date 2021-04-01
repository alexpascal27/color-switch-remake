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
    [Range(0f, 3f)] [SerializeField] private float screenScaleFactor = 1.5f;
    public Vector3 vectorToAddToSpawnPoint = Vector3.zero;
    private bool firstTime = true;
    private List<VerticalObject> _verticalObjects = new List<VerticalObject>();
    
    void Awake()
    {
        // Get ScreenHeight
        availableScreenHeight = Mathf.Abs(Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y) + Mathf.Abs(Camera.main.ScreenToWorldPoint(Vector3.zero).y);
        availableScreenHeight *= screenScaleFactor;
        Debug.Log("AvailableScreenHeight: " + availableScreenHeight);
        
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
        bool firstObjectOutOfBounds = IsFirstObjectOutOfBounds();

        if (firstObjectOutOfBounds)
        {
            // Remove
            RemoveFirstObject();
            Debug.Log("Removed");
            
            // Add
            Debug.Log("Add: " + AddObject(shape));;
        }
        
    }

    private bool IsFirstObjectOutOfBounds()
    {
        if (_verticalObjects.Count < 1) return false;
        VerticalObject firstVerticalObject = _verticalObjects[0];
        float topOfObjectY = firstVerticalObject.GetObjectPrefab().transform.position.y +
                     firstVerticalObject.GetObjectVerticalSize() / 2 + firstVerticalObject.GetTopGapSize();
        
        // Get Y position of bottom of screen
        float bottomY = GetPositionAtBottomOfScreen().y;

        // If below screen
        if (topOfObjectY < bottomY)
        {
            return true;
        }

        return false;
    }

    private bool AddObject(GameObject objectPrefab)
    {
        float changedBottomGap = bottomGap;
        if (firstTime)
        {
            changedBottomGap *= 1;
            firstTime = false;
            Debug.Log("First time");
        }
        VerticalObject verticalObject = new VerticalObject(objectPrefab, changedBottomGap, topGap);
        float objectVerticalSize = verticalObject.GetVerticalSize();

        if (availableScreenHeight - objectVerticalSize > 0)
        {
            availableScreenHeight -= objectVerticalSize;
            SpawnVerticalObject(verticalObject);
            _verticalObjects.Add(verticalObject);
            return true;
        }

        return false;
    }

    private void SpawnVerticalObject(VerticalObject verticalObject)
    {
        // update spawn point to simulate bottom gap
        vectorToAddToSpawnPoint.y += verticalObject.GetBottomGapSize();

        // spawn object
        GameObject objectToSpawn = verticalObject.GetObjectPrefab();
        float objectCenterY = verticalObject.GetObjectVerticalSize() / 2;
        Vector3 objectSpawnPosition = GetPositionAtBottomOfScreen() + vectorToAddToSpawnPoint + new Vector3(0, objectCenterY, 0);
        objectToSpawn.transform.position = objectSpawnPosition;
        verticalObject.SetObjectPrefab(Instantiate(objectToSpawn));

        // update spawn point
        vectorToAddToSpawnPoint.y += objectCenterY * 2;
        Debug.Log("Height: " +(objectCenterY * 2));

        // update spawn point to simulate top gap
        vectorToAddToSpawnPoint.y += verticalObject.GetTopGapSize();
    }

    private void RemoveFirstObject()
    {
        VerticalObject objectToRemove = _verticalObjects[0];
        
        // update vertical spawn point by minusing object
        // add to area
        float verticalSize = objectToRemove.GetVerticalSize();
        availableScreenHeight += verticalSize;
        // Move spawn point down
        vectorToAddToSpawnPoint.y -= verticalSize;
        
        DestroyImmediate(objectToRemove.GetObjectPrefab(), true);

        _verticalObjects.RemoveAt(0);
    }

    private Vector3 GetPositionAtBottomOfScreen()
    {
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        return new Vector3(0, bottomLeft.y);
    }

}
