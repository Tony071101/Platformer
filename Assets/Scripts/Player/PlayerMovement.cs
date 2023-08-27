using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;
    private Player player;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private bool isFacingRight = true;
    private PlayerState state;

    //Moving mechanic
    private float jumpForce = 14f;
    private float speedForce = 7f;
    private float directionX;
    private float angle = 0f;
    private float distance = .1f;

    //Dashing mechanic
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 20f;
    private float dashingTime = 0.3f;
    private float dashingCd = 1f;

    //Wall slide and Wall jump mechanic
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    private float radius = 0.2f;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private bool isKnockedBack = false;
    // private enum MovementState { idle, running, jumping, falling, sliding, dashing, wallSliding }
    // Start is called before the first frame update
    private void Start()
    {
        player = GetComponent<Player>();
        player.onMove += Move;
        player.onKnockBack += IsKnockedBack;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private bool CanMove
    {
        get
        {
            return anim.GetBool("canMove");
        }
    }
    private void OnDisable()
    {
        player.onMove -= Move;
        player.onKnockBack -= IsKnockedBack;
    }
    // Update is called once per frame

    private void Move(object sender, EventArgs e)
    {
        directionX = Input.GetAxisRaw("Horizontal");
        if (isDashing)
        {
            return;
        }
        if (CanMove)
        {
            if (!isKnockedBack)
            {
                _rigidbody2D.velocity = new Vector2(directionX * speedForce, _rigidbody2D.velocity.y);
            }
            if (Input.GetButtonDown("Jump") && GroundCheck())
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && directionX != 0 && !WallCheck())
            {
                StartCoroutine(Dash());
            }

            if (!isWallJumping)
            {
                Flip();
            }

            WallSlide();

            WallJump();
        }

        AnimUpdate();

    }

    private void AnimUpdate()
    {
        if (_rigidbody2D.velocity.y > 0.1f)
        {
            state = PlayerState.jumping;
        }
        else if (_rigidbody2D.velocity.y < -0.1f)
        {
            state = PlayerState.falling;
            if (isWallSliding)
            {
                state = PlayerState.wallSliding;
            }
        }
        else if (directionX != 0)
        {
            if (isDashing)
            {
                if (!GroundCheck())
                {
                    state = PlayerState.dashing;
                }
                else
                {
                    state = PlayerState.sliding;
                }
            }
            else
            {
                state = PlayerState.running;
            }
        }
        else
        {
            state = PlayerState.idle;
        }

        anim.SetInteger("state", (int)state);
    }

    private void Flip()
    {
        if (isFacingRight && directionX < 0f || !isFacingRight && directionX > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool GroundCheck()
    {
        //reminder to recheck what Boxcast is in Google
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, angle, Vector2.down, distance, jumpableGround);
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapCircle(wallCheck.position, radius, wallLayer);
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

    private void WallSlide()
    {
        if (WallCheck() && !GroundCheck() && directionX != 0)
        {
            isWallSliding = true;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Mathf.Clamp(_rigidbody2D.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            _rigidbody2D.velocity = new Vector2(wallJumpingDirection * jumpForce, jumpForce);
            wallJumpingCounter = 0f;
            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x = wallJumpingDirection;
                transform.localScale = localScale;
            }
        }

        Invoke(nameof(StopWallJumping), wallJumpingDuration);
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    private void IsKnockedBack(Vector2 knockBack)
    {
        isKnockedBack = true;
        _rigidbody2D.velocity = new Vector2(knockBack.x, _rigidbody2D.velocity.y + knockBack.y);
        StartCoroutine(ResetKnockBack());
    }

    private IEnumerator ResetKnockBack()
    {
        yield return new WaitForSeconds(0.2f); // Đợi trong khoảng thời gian xung đột
        isKnockedBack = false;
    }
}
