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

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        // Pick Random colour
        colour = colourList[Random.Range(0, colourList.Count)];
        Debug.Log("Player is colour: " + colour);
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
        if (collisionGameObject.CompareTag(colour))
        {
            Physics2D.IgnoreCollision(other.collider, circleCollider2D);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
