using UnityEngine;
using UnityEngine.UI;

namespace GMTK2021.MENU
{
    public class QuitButton : ButtonManager
    {
        public override void OnClick()
        {
            Application.Quit();
        }
    }

}