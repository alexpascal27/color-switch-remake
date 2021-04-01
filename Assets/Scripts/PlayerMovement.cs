using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float jumpForce;
    [SerializeField] private InfiniteSpawn _infiniteSpawn;
    private Vector3 currentPosition;
    private Vector3 previousPosition;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //currentPosition = rb.transform.position;
        //previousPosition = currentPosition;
    }
    
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveUp();
        }
    }

    private void MoveUp()
    {
        // Turn off gravity
        float initialGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;

        // Jump
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(0f, jumpForce));    
        
        // Turn gravity on
        rb.gravityScale = initialGravityScale;
    }
}
