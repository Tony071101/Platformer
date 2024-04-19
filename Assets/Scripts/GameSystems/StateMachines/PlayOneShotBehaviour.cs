using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotBehaviour : StateMachineBehaviour
{
   [SerializeField] private AudioClip soundToPlay;
   [SerializeField] private float volume = 2f;
   [SerializeField] private bool playOnEnter = true, playOnExit = false, playAfterDelay = false;
   [SerializeField] private float playDelay = 0.25f;
   private float timeSinceEntered = 0f;
   private bool hasDelayedSoundPlayed = false;
   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      if(playOnEnter) {
         AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
         hasDelayedSoundPlayed = true;
      }

      timeSinceEntered = 0f;
   }

   // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      if (playAfterDelay && !hasDelayedSoundPlayed)
      {
         timeSinceEntered += Time.deltaTime;
         if (timeSinceEntered > playDelay)
         {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
            hasDelayedSoundPlayed = true;
         }
      }
   }

   // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      if(playOnExit) {
         AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
      }

      hasDelayedSoundPlayed = false;
   }

   // OnStateMove is called right after Animator.OnAnimatorMove()
   // override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   // {
   //    if(playOnMove) {
   //       AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
   //    }
   // }

   // OnStateIK is called right after Animator.OnAnimatorIK()
   //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   //{
   //    // Implement code that sets up animation IK (inverse kinematics)
   //}
}
