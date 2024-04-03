using MP.GameElements.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace MP.StateMachine
{
    public class MoveBehaviour : StateMachineBehaviourBase
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();


        }

    }

    
} 

    

