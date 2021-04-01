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
        // If parent has render then we gucci and just return the height
        Renderer renderer = objectPrefab.GetComponent<Renderer>();
        if(renderer!=null) return renderer.bounds.size.y;
        // otherwise
        else
        {
            // Loop through children and if we haven't checked a specific position add to total
            float lowestPointY = 10000, highestPointY = -1000;
            int i = 0;
            GameObject childGameObject = objectPrefab.transform.GetChild(i).gameObject;
            while (true)
            {
                lowestPointY = CheckPoint(lowestPointY, childGameObject, true);
                highestPointY = CheckPoint(highestPointY, childGameObject, false);
                
                i++;
                try
                {
                    childGameObject = objectPrefab.transform.GetChild(i).gameObject;
                }
                catch (Exception e)
                {
                    break;
                }
            }

            return highestPointY - lowestPointY;
        }
    }

    private float CheckPoint(float pointY, GameObject childGameObject, bool lowest)
    {
        float centerPositionY = childGameObject.transform.position.y;
        Vector3 rotation = childGameObject.transform.rotation.eulerAngles;
        //Debug.Log("Center Position Y: " + centerPositionY);
        Renderer renderer = childGameObject.GetComponent<Renderer>();
        float height = rotation.z != 0 ? renderer.bounds.size.x : renderer.bounds.size.y;
        //Debug.Log("Height: " + height);
        //Debug.Log("PointY: " + pointY);
        if (lowest)
        {
            float lowestPointY = centerPositionY - height / 2;
            //Debug.Log("LowestPointY: " + lowestPointY);
            if (lowestPointY < pointY) return lowestPointY;
            else return pointY;
        }
        else
        {
            float highestPointY = centerPositionY + height / 2;
            //Debug.Log("HighestPointY: " + highestPointY);
            if (highestPointY > pointY) return highestPointY;
            else return pointY;
        }
    }
    
}
