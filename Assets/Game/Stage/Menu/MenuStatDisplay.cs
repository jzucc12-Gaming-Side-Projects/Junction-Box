using SAVING;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK2021.UI
{
    public class MenuStatDisplay : MonoBehaviour, ILoadable
    {
        #region//Cached variables
        TextMeshProUGUI myText;
        #endregion

        #region//Display type
        enum displayStat
        {
            time = 0,
        }
        [SerializeField] displayStat statDisplayed = displayStat.time;
        string displayText = null;
        string baseText;
        #endregion

        #region//Monobehaviour
        private void OnEnable()
        {
            LevelSelectMenuManager.DataDeleted += OnDelete;
        }

        private void OnDisable()
        {
            LevelSelectMenuManager.DataDeleted -= OnDelete;
        }

        private void Awake()
        {
            myText = GetComponent<TextMeshProUGUI>();
            baseText = myText.text + " ";
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(displayText))
                myText.text = baseText + "N/A";
            else
                myText.text = baseText + displayText;
        }
        #endregion

        #region//Progression
        void OnDelete()
        {
            displayText = "N/A";
            myText.text = baseText + displayText;
        }

        public void LoadState(object _state)
        {
            Dictionary<string, object> copyState = (Dictionary<string, object>)_state;

            switch (statDisplayed)
            {
                case displayStat.time:
                    var timeVal = (float)copyState["time"];
                    displayText = timeVal.ToString("F2");
                    break;
            }
        }
        #endregion
    }

}