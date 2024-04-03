using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MP.GameElements.Interactions
{
    [RequireComponent(typeof(PhotonView))]
    public abstract class Interactable : MonoBehaviour
    {
        public const int NOBODY = -1;
        public int interactingClientID { get; protected set; } = NOBODY;
        protected PhotonView view;

        protected virtual void Awake()
        {
            view = GetComponent<PhotonView>();
        }

        public virtual bool BeginInteraction(int clientID)
        {
            return interactingClientID == NOBODY;
        }

        public virtual void DuringInteraction()
        {

        }

        public virtual void EndInteraction()
        {

        }
    }

}
