using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer sprite;
    public bool canMove = true;

    Vector2 movement;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        canMove = true;
    }

    void Update()
    {
        if (!canMove)
        {
            movement = Vector2.zero;
            if (anim != null)
            {
                anim.SetFloat("MoveX", 0);
                anim.SetFloat("MoveY", 0);
                anim.SetFloat("Speed", 0);
            }
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.sqrMagnitude > 1f) movement = movement.normalized;

        if (anim != null)
        {
            anim.SetFloat("MoveX", movement.x);
            anim.SetFloat("MoveY", movement.y);
            anim.SetFloat("Speed", movement.sqrMagnitude);
        }

        if (sprite != null)
        {
            if (movement.x > 0.01f) sprite.flipX = false;
            else if (movement.x < -0.01f) sprite.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
