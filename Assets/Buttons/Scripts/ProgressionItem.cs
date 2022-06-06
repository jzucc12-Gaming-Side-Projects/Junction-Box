using GMTK2021.UI;
using SAVING;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GMTK2021.MENU
{
    public class ProgressionItem : SceneChangeButton, ILoadable
    {
        [SerializeField] ProgressionItem preReq = null;
        [SerializeField] bool selfPreReq = false;
        bool isCompleted = false;
        bool loaded = false;

        #region//Monobehaviour
        private void OnEnable()
        {
            LevelSelectMenuManager.DataDeleted += OnDelete;
            LevelSelectMenuManager.LoadingDone += Check;
        }

        private void OnDisable()
        {
            LevelSelectMenuManager.DataDeleted -= OnDelete;
            LevelSelectMenuManager.LoadingDone -= Check;
        }

        void Update()
        {
            Check();
        }
        #endregion

        #region//Progression
        private void Check()
        {
            if(selfPreReq && !loaded)
            {
                gameObject.SetActive(false);
                return;
            }

            if (preReq == null) return;
            if (!preReq.isCompleted)
                gameObject.SetActive(false);
        }

        public void LoadState(object _state)
        {
            Dictionary<string, object> copyState = (Dictionary<string, object>)_state;
            isCompleted = (bool)copyState["finish"];
            loaded = true;
            if (selfPreReq)
                gameObject.SetActive(isCompleted);
        }

        public void OnDelete() 
        { 
            isCompleted = false;
            if (selfPreReq) gameObject.SetActive(false);
            else if (preReq != null)
                gameObject.SetActive(false);

        }
        #endregion
    }
}
