using System;
using UnityEngine;

public class ShapeMovement : MonoBehaviour
{
    public float rotationSpeed;
    [SerializeField] private bool rotateLeft = true;
    private Rigidbody2D rb;
    private Vector3 shapeCenter;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        shapeCenter = FindCenter();
    }

    private Vector3 FindCenter()
    {
        int i = 0;
        Vector3 center = Vector3.zero;
        Transform childTransform = gameObject.transform.GetChild(i);
        while (true)
        {
            center += childTransform.position;
            i++;
            try
            {
                childTransform = gameObject.transform.GetChild(i);
            }
            catch (Exception e)
            {
                break;
            }
        }

        return center / i;
    }

    void FixedUpdate()
    {
        Vector3 axisOfRotation = rotateLeft ? new Vector3(0, 0, 1) : new Vector3(0, 0, -1);
        float angle = rotationSpeed * Time.deltaTime;
        rb.transform.RotateAround(shapeCenter, axisOfRotation, angle);
    }
}
