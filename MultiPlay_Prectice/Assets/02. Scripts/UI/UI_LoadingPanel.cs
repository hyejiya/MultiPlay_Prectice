using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MP.UI
{
    public class UI_LoadingPanel : UIScreenBase
    {
        public override void Show()
        {
            base.Show();
            sortingOrder = 99;
        }
    }
}

