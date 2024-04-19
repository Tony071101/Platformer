using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    [HideInInspector]
    [SerializeField] private ContactFilter2D contactFilter;

    [HideInInspector]
    [SerializeField] private Transform wallCheck;

    [HideInInspector]
    [SerializeField] private LayerMask whatIsGround;
    public Animator _anim { get; private set; }
    public Rigidbody2D _rigidbody { get; private set; }
    private CapsuleCollider2D touchingCol;
    private float groundDistance = 0.05f;
    private float wallDistance = 0.15f;
    private float ceilingDistance = 0.05f;
    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    private bool _isGrounded;
    public bool IsGrounded { get{
        return _isGrounded; 
    } private set{
        _isGrounded = value;

        if(_anim == null) {
            return;
        }

        _anim.SetBool(AnimationStrings.isGrounded, value);
    } }

    private bool _isOnWall;
    public bool IsOnWall { get{
        return _isOnWall; 
    } private set{
        _isOnWall = value;

        if(_anim == null) {
            return;
        }

        _anim.SetBool(AnimationStrings.isOnWall, value);
    } }

    private bool _isOnCeiling;
    public bool IsOnCeiling { get{
        return _isOnCeiling; 
    } private set{
        _isOnCeiling = value;

        if(_anim == null) {
            return;
        }

        _anim.SetBool(AnimationStrings.isOnCeiling, value);
    } }

    private void Awake() 
    {
        _anim = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        touchingCol = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate() 
    {
        IsGrounded = touchingCol.Cast(Vector2.down, contactFilter, groundHits, groundDistance) > 0;  
        IsOnCeiling = touchingCol.Cast(Vector2.up, contactFilter, ceilingHits, ceilingDistance) > 0;
        IsOnWall = Physics2D.Raycast(wallCheck.position, wallCheckDirection, wallDistance, whatIsGround);
    }
}
