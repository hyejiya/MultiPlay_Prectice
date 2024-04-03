using Photon.Pun;
using UnityEngine;

namespace MP.GameElements.Interactions
{
    [RequireComponent(typeof(PhotonView))]
    public class Interactor : MonoBehaviour
    {
        protected LayerMask interactableMask;
        protected PhotonView view;
        protected Interactable current;

        protected virtual void Awake()
        {
            view = GetComponent<PhotonView>();
        }

        public bool TryInteraction()
        {
            if(TryCastInteractable(out Interactable interactable))
            {
                //현재 상호작용을 아무도 안하고 있다면 상호작용 시작
                if (interactable.interactingClientID == Interactable.NOBODY)
                {
                    if (interactable.BeginInteraction(view.OwnerActorNr))
                    {
                        current = interactable;
                    }
                }                 
                else
                {
                    current.EndInteraction();
                }
                interactable.BeginInteraction(view.OwnerActorNr);
            }
            return false;
        }

        protected virtual bool TryCastInteractable(out Interactable interactable)
        {
            if(Physics.SphereCast(origin: transform.position,
                               radius: 0.5f,
                               direction: transform.forward,
                               hitInfo: out RaycastHit hit,
                               maxDistance: 1f,
                               layerMask: interactableMask))
            {
                interactable= hit.collider.GetComponent<Interactable>();
                return true;
            }

            interactable = null;
            return false;
            
        }
    }
}
