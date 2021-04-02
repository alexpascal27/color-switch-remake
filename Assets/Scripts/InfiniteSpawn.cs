using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InfiniteSpawn : MonoBehaviour
{
    [SerializeField] private GameObject shape;
    [SerializeField] private float bottomGap = 10f;
    [SerializeField] private float topGap = 5f;
    [SerializeField] private float bottomGapMinSize = 1f;
    [SerializeField] private float bottomGapMaxSize = 5f;
    [SerializeField] private float topGapMinSize = 1f;
    [SerializeField] private float topGapMaxSize = 5f;
    
    private float availableScreenHeight;
    [Range(0f, 3f)] [SerializeField] private float screenScaleFactor = 1.5f;
    public Vector3 vectorToAddToSpawnPoint = Vector3.zero;
    private bool firstTime = true;
    private List<VerticalObject> _verticalObjects = new List<VerticalObject>();
    
    [SerializeField] private float[] rotationSpeedOptions = new float[]{20f, 40f, 60f};
    [SerializeField] private float[] scaleOptions = new float[]{0.75f, 1f, 1.25f};
    
    void Awake()
    {
        // Get ScreenHeight
        availableScreenHeight = Mathf.Abs(Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y) + Mathf.Abs(Camera.main.ScreenToWorldPoint(Vector3.zero).y);
        availableScreenHeight *= screenScaleFactor;
        
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
            
            // Add
            AddObject(shape);;
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
            changedBottomGap *= 4f;
            firstTime = false;
        }
        else
        {
            changedBottomGap = Random.Range(bottomGapMinSize, bottomGapMaxSize);
            topGap = Random.Range(topGapMinSize, bottomGapMaxSize);
        }
        VerticalObject verticalObject = new VerticalObject(objectPrefab, 7f, changedBottomGap, topGap);
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
        // Random rotation when starting
        objectToSpawn.transform.position = Vector3.zero;
        //objectToSpawn.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, 359f)));
        // Get position of object
        float objectCenterY = verticalObject.GetObjectVerticalSize() / 2;
        Vector3 objectSpawnPosition = GetPositionAtBottomOfScreen() + vectorToAddToSpawnPoint + new Vector3(0, objectCenterY, 0);
        objectToSpawn.transform.position = objectSpawnPosition;
        // Set scale and rotation of object
        objectToSpawn = SetScaleAndRotationOfShape(objectToSpawn);
        verticalObject.SetObjectPrefab(Instantiate(objectToSpawn));
        // Set rotation speed of shape
        

        // update spawn point
        vectorToAddToSpawnPoint.y += objectCenterY * 2;
        // update spawn point to simulate top gap
        vectorToAddToSpawnPoint.y += verticalObject.GetTopGapSize();
    }

    private GameObject SetScaleAndRotationOfShape(GameObject shapeObject)
    {
        Transform parentTransform = shapeObject.transform.parent;
        shapeObject.transform.parent = null;
        
        // Randomise the scale
        int indexOfScale = Random.Range(0, scaleOptions.Length);
        float scale = scaleOptions[indexOfScale];
        shapeObject.transform.localScale = new Vector3(scale, scale);

        shapeObject.transform.parent = parentTransform;

        return SetRotationBasedOnScale(shapeObject, indexOfScale);
    }

    private GameObject SetRotationBasedOnScale(GameObject shapeObject, int i)
    {
        // Get rotation index - reverse of scale (if scale i is 2 then rotation 1 is 0, if scale i is 1 then rotation i is 1)
        //int rotationIndex = rotationSpeedOptions.Length - 1 - i;
        
        // Set rotation speed based on the ShapeMovement Script
        shapeObject.GetComponent<ShapeMovement>().rotationSpeed = rotationSpeedOptions[i];

        return shapeObject;
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
