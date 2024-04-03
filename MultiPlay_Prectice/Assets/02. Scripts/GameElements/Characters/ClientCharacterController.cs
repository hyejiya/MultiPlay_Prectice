using MP.GameElements.Characters;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MP.GameElements
{
    [RequireComponent(typeof(PhotonView), typeof(PhotonTransformView))]
    public class ClientCharacterController : MonoBehaviour
    {
        public static Dictionary<int, ClientCharacterController> _spawned = new Dictionary<int, ClientCharacterController> ();

        private Camera _cam;
        private PhotonView _view;
        private NavMeshAgent _agent;
        private Animator _animator;
        private LayerMask _groundMask;

        private void Awake()
        {
            _view = GetComponent<PhotonView>();
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _groundMask = 1 << LayerMask.NameToLayer("Ground");
            _cam = Camera.main;
            _spawned.Add(_view.OwnerActorNr, this);
        }

        private void Update()
        {
            if(_view.IsMine)
            {              

                Vector3 relVeloctiy = new Vector3(Vector3.Dot(_agent.transform.right, _agent.velocity),
                                      0f,
                                  Vector3.Dot(_agent.transform.forward, _agent.velocity));

                _animator.SetFloat("velocityZ", relVeloctiy.z / _agent.speed);
            }          
        }

        public void MoveTo(Vector2 touchPosition)
        {
                Ray ray = _cam.ScreenPointToRay(touchPosition);

                if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _groundMask) &&
                        NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, 0.5f, NavMesh.AllAreas))
                {
                    _agent.SetDestination(navMeshHit.position);
                }
            
        }

        public void ChangeState(State newState)
        {
            _animator.SetInteger("state", (int)newState);
            _animator.SetBool("isDirty", true);
        }
    }        
}




