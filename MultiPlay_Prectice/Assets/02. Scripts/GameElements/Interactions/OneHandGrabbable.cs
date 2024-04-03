using Photon.Pun;
using UnityEngine;

namespace MP.GameElements.Interactions
{
    public class OneHandGrabbable : Interactable, IOneHandGrabbable
    {
        protected Rigidbody rigid;
        protected Collider collider;

        protected override void Awake()
        {
            base.Awake();
            rigid = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        }
        public override bool BeginInteraction(int clientID)
        {
            if(base.BeginInteraction(clientID) == false)
                return false;
            

            if (ClientCharacterController._spawned.ContainsKey(clientID) == false)
                throw new System.Exception($"[OneHandGrabbable] : Failed to BeginIbteratcion. wrong clientID {clientID}");

            view.RPC("GrabClientRpc", RpcTarget.All, new object[] { clientID });
            return true;
        }

        public override void EndInteraction()
        {
            base.EndInteraction();

            view.RPC("UngrabClientRpc", RpcTarget.All, null);
        }


        [PunRPC]
        public void GrabClientRpc(int clientID)
        {
            interactingClientID = clientID;

            if (ClientCharacterController._spawned.TryGetValue(clientID, out ClientCharacterController controller))
            {
                if (controller.TryGetComponent(out IOneHandGrabber oneHandGrabber))
                {
                    Grab(oneHandGrabber);
                }
            }
        }

        [PunRPC]
        public void UngrabClientRpc()
        {
            Ungrab();
            interactingClientID = NOBODY;
        }

        public void Grab(IOneHandGrabber grabber)
        {
            transform.SetParent(grabber.hand);
            rigid.isKinematic = true;
            collider.isTrigger = true;
            transform.localPosition = Vector3.zero;
        }

        public void Ungrab()
        {
            rigid.isKinematic = false;
            collider.isTrigger = false;
            transform.SetParent(null);
        }
    }

}
