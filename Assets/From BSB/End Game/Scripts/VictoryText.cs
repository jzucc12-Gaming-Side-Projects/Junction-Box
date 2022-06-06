using UnityEngine;
using UnityEngine.UI;

namespace GMTK2021.ENDGAME
{
    public class VictoryText : MonoBehaviour
    {
        string victoryText;
        Color displayColor;
        Text myText;

        private void Awake()
        {
            myText = GetComponent<Text>();
        }

        private void Update()
        {
            myText.text = victoryText;
            myText.color = displayColor;
        }

        public void SetVictoryText(string _text) { victoryText = _text; }
        public void SetTextColor(Color _color) { displayColor = _color; }
    }
}
