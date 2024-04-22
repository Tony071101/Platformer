using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
public class Player : MonoBehaviour
{
    #region Variables
    private TouchingDirections touchingDirections;
    private Damageable damageable;
    public Rigidbody2D _rigidbody { get; private set; }
    public Animator _anim { get; private set; }

    [Header("Move")]
    private Vector2 horizontalMovement;
    private float moveSpeed = 10f;
    private float dashSpeed = 25f;
    private float resetTimerForDash = 0.2f;

    [Header("Jump")]
    private float jumpImpulse = 12f;
    private float onAirSpeed = 10f;
    private int maxJump = 2;
    private int jumpsRemaining;

    [Header("Gravity")]
    private float baseGravity = 2f;
    private float maxFallSpeed = 25f;
    private float fallSpeedMultiplier = 2f; 

    [Header("OnWall")]
    private float wallSlideSpeed = 2f;
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.5f;
    private float wallJumpTimer;
    private float skillCoolDown = 2f; //Can be modified
    private bool isDashCooldown = false;

    private bool _isFacingRight = true;
    public bool IsFacingRight { get { return _isFacingRight; } private set {
        if(_isFacingRight != value)
        {
            //FLip the localscale to face opposite direction
            transform.localScale *= new Vector2(-1, 1);
        }
        
        _isFacingRight = value;
    } }
    public bool CanMove
    {
        get
        {
            return _anim.GetBool(AnimationStrings.canMove);
        }
    }
    public bool IsAlive {
        get {
            return _anim.GetBool(AnimationStrings.isAlive);
        }
    }

    private bool _isMoving = false;
    public bool IsMoving 
    { 
        get
        {
            return _isMoving;
        } 
        private set
        {
            _isMoving = value;
            _anim.SetBool(AnimationStrings.isMoving, value);
        }
    }
    
    private bool _isDashing = false;
    public bool IsDashing
    {
        get
        {
            return _isDashing;
        }
        private set
        {
            _isDashing = value;
            _anim.SetBool(AnimationStrings.isDashing, value);
        }
    }
    
    public float CurrentSpeed 
    {
        get
        {
            if(CanMove)
            {
                if(IsMoving && !touchingDirections.IsOnWall)
                {
                    if(touchingDirections.IsGrounded)
                    {
                        if(IsDashing)
                        {
                            return dashSpeed;
                        }
                        else
                        {
                            return moveSpeed;
                        }
                    } else 
                    {
                        if(IsDashing) {
                            return dashSpeed;
                        } else {
                            return onAirSpeed;
                        }
                    }
                } else
                {
                    //Idle
                    return 0;
                }
            } else
            {
                //Move locked
                return 0;
            }
        }
    }
    #endregion

    private void Start() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            UIManager.Instance.ActivatePlayerUI();
        }
        MusicManager.Instance.PlayGameplayAudio();
    }

    private void Update() {
        HandleCheckDoubleJump();

        HandleGravity();

        HandleWallSlide();

        HandleCheckWallJump();
    }

    private void FixedUpdate() 
    {
        if(!isWallJumping) {
            Move();
        }
    }

    #region Handle Functions
    private void Move()
    {
        if(!damageable.LockVelocity) {
            if(touchingDirections.IsOnWall) {
                _rigidbody.velocity = new Vector2(0,0);
            } else {
                _rigidbody.velocity = new Vector2(horizontalMovement.x * CurrentSpeed, _rigidbody.velocity.y);      
            }
        }

        _anim.SetFloat(AnimationStrings.yVelocity, _rigidbody.velocity.y);
    }

    private void Dash() {
        if(IsMoving && IsAlive) {
            IsDashing = true;

            float dashDirection = IsFacingRight ? 1 : -1;

            _rigidbody.velocity = new Vector2(dashDirection * dashSpeed, _rigidbody.velocity.y);

            Invoke("ResetBoolDashing", resetTimerForDash);
            isDashCooldown = true;
            Invoke(nameof(ResetDashCooldown), skillCoolDown);
        }
    }

    private void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpImpulse);
        _anim.SetTrigger(AnimationStrings.jumpTrigger);
        jumpsRemaining--;
    }

    private void WallJump() {
        isWallJumping = true;
        _rigidbody.velocity = new Vector2(wallJumpDirection * moveSpeed, jumpImpulse);
        wallJumpTimer = 0f;

        Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
    }

    private void HandleCheckDoubleJump() {
        if(touchingDirections.IsGrounded) {
            jumpsRemaining = maxJump;
        }
    }

    private void HandleGravity() {
        if(_rigidbody.velocity.y < 0) {
            _rigidbody.gravityScale = baseGravity * fallSpeedMultiplier; //Fall faster
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Max(_rigidbody.velocity.y, - maxFallSpeed)); 
        } else {
            _rigidbody.gravityScale = baseGravity;
        }
    }

    private void HandleWallSlide() {
        if(!touchingDirections.IsGrounded && CanMove && touchingDirections.IsOnWall) {
            isWallSliding = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Max(_rigidbody.velocity.y, -wallSlideSpeed));
        } else {
            isWallSliding = false;
        }
    }

    private void HandleCheckWallJump() {
        if(isWallSliding && !isWallJumping) {
            wallJumpDirection = transform.localScale.x > 0 ? -1 : 1;
            wallJumpTimer = wallJumpTime;
        } else if (wallJumpTimer > 0f) {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump() {
        isWallJumping = false;
    }

    private void ResetBoolDashing()
    {
        IsDashing = false;
    }
    
    private void ResetDashCooldown()
    {
        isDashCooldown = false;
    }

    private void SetFacingDirection(Vector2 horizontalMovement)
    {
        if(horizontalMovement.x > 0 && !IsFacingRight)
        {
            //Face right
            IsFacingRight = true;
        } else if(horizontalMovement.x < 0 && IsFacingRight)
        {
            //Face left
            IsFacingRight = false;
        }
    }
    #endregion

    #region Event Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>();

        if(IsAlive) {
            IsMoving = horizontalMovement != Vector2.zero;  

            SetFacingDirection(horizontalMovement);
        } else {
            IsMoving = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.started && !isDashCooldown)
        {
            Dash();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    { 
        if(context.started && CanMove && IsAlive)
        {
            if(jumpsRemaining > 0)
            {
                Jump();
            }

            if(wallJumpTimer > 0f && isWallSliding){
                WallJump();
            }
        } 
        else if(context.canceled) {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
            jumpsRemaining--;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _anim.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnHit(int dmg, Vector2 knockBack) {
        if(IsAlive) {
            _rigidbody.velocity = new Vector2(knockBack.x, _rigidbody.velocity.y + knockBack.y);
        } else {
            _rigidbody.velocity = new Vector2(0,0);
            UIManager.Instance.ActivateGameOverPanel();
        }
    }
    #endregion
}
