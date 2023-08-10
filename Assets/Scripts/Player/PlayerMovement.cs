using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask wallLayer;
    private Player player;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private float jumpForce = 14f;
    private float speedForce = 7f;
    private float directionX;
    private float angle = 0f;
    private float distance = .1f;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 20f;
    private float dashingTime = 0.2f;
    private float dashingCd = 1f;
    private enum MovementState { idle, running, jumping, falling, sliding, dashing }
    // Start is called before the first frame update
    private void Start()
    {
        player = GetComponent<Player>();
        player.onMove += Move;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnDisable()
    {
        player.onMove -= Move;
    }
    // Update is called once per frame

    private void Move(object sender, EventArgs e)
    {
        if (isDashing)
        {
            return;
        }
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        AnimUpdate();
    }

    private void AnimUpdate()
    {
        MovementState state;

        if (_rigidbody2D.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (_rigidbody2D.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }
        else if (directionX != 0)
        {
            state = isDashing ? (!GroundCheck() ? MovementState.dashing : MovementState.sliding) : MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool GroundCheck()
    {
        //reminder to recheck what Boxcast is in Google
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, angle, Vector2.down, distance, jumpableGround);
    }

    private bool WallCheck()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, angle, new Vector2(transform.localScale.x, 0f), distance, wallLayer);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0;
        _rigidbody2D.velocity = new Vector2(directionX * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        _rigidbody2D.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCd);
        canDash = true;
    }
}
