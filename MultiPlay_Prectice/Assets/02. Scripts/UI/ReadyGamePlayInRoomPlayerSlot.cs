using UnityEngine;
using TMPro;

namespace MP.UI
{
    public class ReadyGamePlayInRoomPlayerSlot : MonoBehaviour
    {
        public bool isReady
        {
            set
            {
                _isReady.enabled = value;
            }
        }

        private TMP_Text _isReady;
        private TMP_Text _nickName;

        public string nickname 
        { 
            get => _nickName.text;
            set
            {
                _nickName.text = value;
            } 
        }

        private void Awake()
        {
            _isReady = transform.Find("Text (TMP) - IsReady").GetComponent<TMP_Text>();
            _nickName = transform.Find("Text (TMP) - Nickname").GetComponent<TMP_Text>();
        }
    }
}

