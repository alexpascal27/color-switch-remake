using System;
using UnityEngine;

public class VerticalObject
{
    private GameObject objectPrefab;
    private float bottomGapSize;
    private float topGapSize;
    
    public VerticalObject(GameObject objectPrefab, float bottomGapSize, float topGapSize)
    {
        this.objectPrefab = objectPrefab;
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
        float objectSize = GetObjectVerticalSize();
        return bottomGapSize + objectSize + topGapSize;
    }

    public float GetObjectVerticalSize()
    {
        return objectPrefab.GetComponent<Renderer>().bounds.size.y;
    }
    
}
