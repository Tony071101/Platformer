using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer sprite;
    private Animator anim;
    private float jumpForce = 14f;
    private float speedForce = 7f;
    private float directionX;

    private enum MovementState { idle, running, jumping, falling }
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
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
        if (Input.GetButtonDown("Jump"))
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
        sprite.transform.rotation = Quaternion.Euler(0f, directionX < 0f ? 180f : 0f, 0f);
    }
}
