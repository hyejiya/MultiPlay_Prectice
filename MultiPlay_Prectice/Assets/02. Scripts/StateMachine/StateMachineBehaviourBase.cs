using MP.GameElements.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MP.StateMachine
{
    public abstract class StateMachineBehaviourBase : StateMachineBehaviour
    {
        private readonly int HASHID_IS_DIRTY = Animator.StringToHash("isDirty");
        protected Dictionary<State, Func<bool>> transitions;
        protected bool isOnTransition = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            animator.SetBool(HASHID_IS_DIRTY, false);
            isOnTransition = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (isOnTransition)
                return;

            foreach (var transition in transitions)
            {
                if (transition.Value.Invoke())
                {
                    ChangeState(animator, transition.Key);
                    break;
                }
            }
        }

        protected void ChangeState(Animator animator, State newState)
        {
            animator.SetInteger("state", (int)newState);
            animator.SetBool(HASHID_IS_DIRTY, false);
        }
    }
}

