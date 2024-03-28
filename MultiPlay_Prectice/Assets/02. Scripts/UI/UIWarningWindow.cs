using UnityEngine.UI;
using TMPro;

namespace MP.UI
{
    public class UIWarningWindow : UIPopupBase
    {
        private TMP_Text _message;
        private Button _confirm;

        protected override void Awake()
        {
            base.Awake();

            _message = transform.Find("Panel/Text (TMP) - Message").GetComponent<TMP_Text>();
            _confirm = transform.Find("Panel/Button - Confirm").GetComponent<Button>();
            _confirm.onClick.AddListener(Hide);
        }

        public void Show(string message)
        {
            base.Show();

            _message.text = message;    
        }
    }
}
