using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;
    private float moveSpeed = -2f;
    private Animator anim;
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D boxCollider;
    private float radius = 0.2f;
    private float angle = 0f;
    private float distance = .1f;
    private enum MovementState { idle, combatIdle, running }
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {

        // if (Vector2.Distance(wayPoints[currentWaypointIndex].transform.position, transform.position) < distance)
        // {
        //     currentWaypointIndex++;
        //     if (currentWaypointIndex >= wayPoints.Length)
        //     {
        //         currentWaypointIndex = 0;
        //     }
        // }

        //Rotate Y when moving left or right
        if (WallCheck())
        {
            transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
            moveSpeed *= -1f;
        }

        _rigidbody2D.velocity = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        // transform.position = Vector2.MoveTowards(transform.position, wayPoints[currentWaypointIndex].transform.position, Time.deltaTime * moveSpeed);

        //Animation
        MovementState state;
        state = MovementState.running;
        anim.SetInteger("AnimState", (int)state);
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
}
