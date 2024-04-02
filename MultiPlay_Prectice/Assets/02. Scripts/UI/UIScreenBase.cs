using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.UI
{
    public class UIScreenBase : UIBase
    {
        protected override void Awake()
        {
            base.Awake();

            UIManager.instance.RegisterPopup(this);
        }

        public override void Show()
        {
            base.Show();

            UIManager.instance.SetScreen(this);
        }
    }
}
