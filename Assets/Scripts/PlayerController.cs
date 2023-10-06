using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float CollisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public bool isInDialog;
    public bool isDead;

    private Vector2 input;
    private Animator animator;
    private Rigidbody2D rb;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isInDialog || isDead) return;
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (input.x != 0) input.y = 0;

        if (input != Vector2.zero)
        {
            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void FixedUpdate()
    {
        if (isInDialog || isDead) return;
        bool success = MovePlayer(input);

        if (!success)
        {
            success = MovePlayer(new Vector2(input.x, 0));

            if (!success)
            {
                success = MovePlayer(new Vector2(0, input.y));
            }
        }
    }

    public bool MovePlayer(Vector2 direction)
    {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + CollisionOffset);

        if (count == 0)
        {
            Vector2 moveVector = direction * moveSpeed * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + moveVector);
            return true;
        }
        else
        {
            return false;
        }
    }
}
