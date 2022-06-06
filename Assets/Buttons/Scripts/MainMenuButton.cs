using UnityEngine.SceneManagement;

namespace GMTK2021.MENU
{
    public class MainMenuButton : ButtonManager
    {
        public override void OnClick() { SceneManager.LoadScene(0); }
    }

}