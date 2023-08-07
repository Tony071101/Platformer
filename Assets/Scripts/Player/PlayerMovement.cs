using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private float jumpForce = 14f;
    private float speedForce = 7f;
    private float directionX;
    private float angle = 0f;
    private float distance = .1f;
    private enum MovementState { idle, running, jumping, falling }
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        UpdateAnimation();
    }

    private void Move()
    {
        directionX = Input.GetAxisRaw("Horizontal");
        _rigidbody2D.velocity = new Vector2(directionX * speedForce, _rigidbody2D.velocity.y);

        if (directionX != 0f)
        {
            sprite.transform.rotation = Quaternion.Euler(0f, directionX < 0f ? 180f : 0f, 0f);
        }

        if (Input.GetButtonDown("Jump") && GroundCheck())
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
        }

    }

    private void UpdateAnimation()
    {
        MovementState state;
        if (directionX != 0)
        {
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }

        if (_rigidbody2D.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (_rigidbody2D.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
        // sprite.transform.rotation = Quaternion.Euler(0f, directionX < 0f ? 180f : 0f, 0f);
    }

    private bool GroundCheck()
    {
        //reminder to recheck what Boxcast is in Google
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, angle, Vector2.down, distance, jumpableGround);
    }

    private bool WallCheck()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, angle, new Vector2(transform.localScale.x, 0), distance, wallLayer);
    }
}
