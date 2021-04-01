using System;
using UnityEngine;

public class VerticalObject
{
    private GameObject objectPrefab;
    private float objectVerticalSize;
    private float bottomGapSize;
    private float topGapSize;

    public VerticalObject(GameObject objectPrefab, float objectVerticalSize, float bottomGapSize, float topGapSize)
    {
        this.objectPrefab = objectPrefab;
        this.objectVerticalSize = objectVerticalSize;
        this.bottomGapSize = bottomGapSize;
        this.topGapSize = topGapSize;
    }

    public GameObject GetObjectPrefab()
    {
        return objectPrefab;
    }

    public void SetObjectPrefab(GameObject newObjectPrefab)
    {
        objectPrefab = newObjectPrefab;
    }

    public float GetBottomGapSize()
    {
        return bottomGapSize;
    }
    
    public float GetTopGapSize()
    {
        return topGapSize;
    }

    public float GetVerticalSize()
    {
        return bottomGapSize + objectVerticalSize + topGapSize;
    }

    public float GetObjectVerticalSize()
    {
        return objectVerticalSize;
    }
    
}
