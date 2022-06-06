using GMTK2021.MENU;
using UnityEngine;

namespace GMTK2021.ENDGAME
{
    public class EndGameUI : MonoBehaviour
    {
        //[SerializeField] LevelButton nextLevel = null;
        Color victoryColor = Color.blue;
        Color defeatColor = Color.red;
        bool didWin = false;
        [SerializeField] Canvas winCanvas = null;
        [SerializeField] Canvas loseCanvas = null;

        void Start()
        {
            if (didWin)
            {
                winCanvas.gameObject.SetActive(true);
                DisplayVictory(victoryColor, "Victory!");
            }
            else
            {
                loseCanvas.gameObject.SetActive(true);
                DisplayVictory(defeatColor, "Defeat...");
            }
        }

        private void DisplayVictory(Color _color, string _text)
        {
            GetComponentInChildren<VictoryText>().SetVictoryText(_text);
            GetComponentInChildren<VictoryText>().SetTextColor(_color);
            GetComponentInChildren<SceneChangeButton>().SetColors(_color);
            GetComponentInChildren<ReloadSceneButton>().SetColors(_color);
        } 

        public void SetDidWin(bool _didWin) { didWin = _didWin; }
    }
}
