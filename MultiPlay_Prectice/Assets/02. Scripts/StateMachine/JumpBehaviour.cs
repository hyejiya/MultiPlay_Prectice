using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace MP.StateMachine
{
    public class JumpBehaviour : StateMachineBehaviourBase
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            animator.GetComponent<NavMeshAgent>().enabled = false;
            animator.GetComponent<Rigidbody>().AddForce(Vector3.up * 3.0f);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent <NavMeshAgent>().enabled = true;
        }
    }
}
