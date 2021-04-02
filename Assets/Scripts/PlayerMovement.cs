using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2D;
    [SerializeField] private float jumpForce;
    private Vector3 currentPosition;
    private Vector3 previousPosition;
    private List<String> colourList = new List<string>(new []{"Red", "Green", "Blue"});
    private String colour;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private Transform raycastFiringPoint;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        // Pick Random colour
        colour = colourList[Random.Range(0, colourList.Count)];
        Debug.Log("Player is colour: " + colour);
    }

    void Update()
    {
        // Send a raycast and if we collide with a collider that has hase tag as us then ignore that collision
        RaycastHit2D raycastHit2D = Physics2D.Raycast(raycastFiringPoint.position, Vector2.up, raycastDistance);
        Collider2D raycastCollider = raycastHit2D.collider;
        // If hit something
        if (raycastCollider != null)
        {
            // If we hit    
            GameObject colliderGameObject = raycastCollider.gameObject;
            if (colliderGameObject.CompareTag(colour))
            {
                Physics2D.IgnoreCollision(colliderGameObject.GetComponent<BoxCollider2D>(), circleCollider2D);
            }
        }
    }
    
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            MoveUp();
        }
    }

    private void MoveUp()
    {
        // Turn off gravity
        //float initialGravityScale = rb.gravityScale;
        //rb.gravityScale = 0f;

        // Jump
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(0f, jumpForce));    
        
        // Turn gravity on
        //rb.gravityScale = initialGravityScale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject collisionGameObject = other.contacts[0].collider.gameObject;
        if (!collisionGameObject.CompareTag(colour))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
