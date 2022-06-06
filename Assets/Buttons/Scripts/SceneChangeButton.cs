using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMTK2021.MENU
{
    public class SceneChangeButton : ButtonManager
    {
        [SerializeField] string targetScene = "";
        public override void OnClick() { SceneManager.LoadScene(targetScene); }
    }
}
