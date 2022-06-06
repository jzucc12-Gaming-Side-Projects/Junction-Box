using UnityEngine.SceneManagement;

namespace GMTK2021.MENU
{
    public class ReloadSceneButton : ButtonManager
    {
        public override void OnClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
