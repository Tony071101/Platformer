using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    private float walkAcceleration = 50f;
    private float maxSpeed = 3f;
    [SerializeField] private DetectionZone attackZone;
    [SerializeField] private DetectionZone cliffDetectionZone;
    private Animator _anim;
    private Vector2 walkDirectionVector = Vector2.right;
    private Damageable damageable;
    public Rigidbody2D _rigidbody { get; private set; }
    private TouchingDirections touchingDirections;
    public enum WalkableDirections { Right, Left }
    private WalkableDirections _walkDirections;
    public WalkableDirections WalkDirections 
    {
        get { return _walkDirections; }
        set { 
            if(_walkDirections != value)
            {
                //Direction flipped
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if(value == WalkableDirections.Right)
                {
                    walkDirectionVector = Vector2.right;
                } else if(value == WalkableDirections.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }    
            _walkDirections = value; 
        }
    }

    private bool _hasTarget = false;
    public bool Hastarget { 
        get {
        return _hasTarget;
        } private set {
        _hasTarget = value;
        
        if(_anim == null) {
            return;
        }
        _anim.SetBool(AnimationStrings.hasTarget, value);
        } 
    }

    public bool CanMove {
        get {
            
            if(_anim == null) {
                return false;
            }
            return _anim.GetBool(AnimationStrings.canMove);
        }
    }

    public float AttackCd { 
        get {
            if(_anim == null) {
                return 0;
            }
            return _anim.GetFloat(AnimationStrings.attackCd);
        } 
        private set {
            if(_anim == null) {
            return;
            }
            _anim.SetFloat(AnimationStrings.attackCd, Mathf.Max(value, 0));
        }
    }

    private void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        _anim = GetComponentInChildren<Animator>();
        damageable= GetComponent<Damageable>();
    }

    private void Update() {
        Hastarget = attackZone.detectedColliders.Count > 0;

        if(AttackCd > 0) {
            AttackCd -= Time.deltaTime; 
        }
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move()
    {
        if(touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }

        if(!damageable.LockVelocity) {
            if(CanMove) {
                _rigidbody.velocity = new Vector2(Mathf.Clamp(
                    _rigidbody.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, 
                    maxSpeed), 
                    _rigidbody.velocity.y);
            } else {
                _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            }
        }
    }

    private void FlipDirection()
    {
        if(WalkDirections == WalkableDirections.Right)
        {
            WalkDirections = WalkableDirections.Left;
        } else if(WalkDirections == WalkableDirections.Left)
        {
            WalkDirections = WalkableDirections.Right;
        }
    }

    public void OnHit(int dmg, Vector2 knockBack) {
        _rigidbody.velocity = new Vector2(knockBack.x, _rigidbody.velocity.y + knockBack.y);
    }

    public void OnCliffDetected() {
        if(touchingDirections.IsGrounded) {
            FlipDirection();
        }
    }
}
