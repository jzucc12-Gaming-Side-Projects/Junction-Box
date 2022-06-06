using GMTK2021.SOUND;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK2021.MENU
{
    public abstract class ButtonManager : MonoBehaviour
    {
        protected Button button;
        Color mod = new Color(0.5f, 0.5f, 0.5f, 0f);

        protected virtual void Awake()
        {
            button = GetComponent<Button>();
        }

        void OnEnable()
        {
            button.onClick.AddListener(Press);
        }

        void OnDisable()
        {
            button.onClick.RemoveListener(Press);
        }

        void Press()
        {
            FindObjectOfType<SFXManager>().PlaySFX(SFXType.button);
        }

        public abstract void OnClick();

        public void SetColors(Color _color)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = _color;
            colors.highlightedColor = _color + mod;
            colors.pressedColor = _color - mod;
            colors.selectedColor = _color;
            button.colors = colors;
        }
    }

}