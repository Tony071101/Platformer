using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] wayPoints;
    private int currentWaypointIndex = 0;
    private float moveSpeed = 2f;
    private float distance = .1f;
    private Animator anim;
    private enum MovementState { idle, combatIdle, running }
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        MovementState state;
        if (Vector2.Distance(wayPoints[currentWaypointIndex].transform.position, transform.position) < distance)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= wayPoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        //Rotate Y when moving left or right
        if (transform.position.x < wayPoints[currentWaypointIndex].transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        transform.position = Vector2.MoveTowards(transform.position, wayPoints[currentWaypointIndex].transform.position, Time.deltaTime * moveSpeed);

        //Animation
        state = MovementState.running;
        anim.SetInteger("AnimState", (int)state);
    }
}
