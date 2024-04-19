using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRemoveBehaviour : StateMachineBehaviour
{
    [SerializeField] private float fadeTime = 0.5f;
    private float timeElapsed = 0f;
    private SpriteRenderer spriteRenderer;
    private GameObject objectToRemove; 
    private Color startColor;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       timeElapsed = 0f;
       spriteRenderer = animator.GetComponent<SpriteRenderer>();
       startColor = spriteRenderer.color;
       objectToRemove = animator.gameObject;
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       timeElapsed += Time.deltaTime;

       float newAlpha = startColor.a * (1 - timeElapsed / fadeTime);

       spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

       if(timeElapsed > fadeTime) {
        Destroy(objectToRemove);
       }
    }
}
