using GMTK2021.BALL;
using GMTK2021.SOUND;
using SAVING;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMTK2021.STAGE
{
    public class LevelManager : MonoBehaviour, ISaveable, ILoadable
    {
        #region//Cached variables
        SaveInitializer saveInitializer;
        [SerializeField] GameObject victoryScreen = null;
        [SerializeField] GameObject defeatScreen = null;
        [SerializeField] GameObject pauseScreen = null;
        #endregion

        #region//Other
        [SerializeField] bool startConnected = true;
        Dictionary<string, object> prevBest = new Dictionary<string, object>();
        #endregion

        #region//End Game Variables
        public static event Action onVictory;
        [SerializeField] string nextLevel = "Level 2";
        bool levelEnded = false;
        #endregion


        #region//Monobehaviour
        private void Awake()
        {
            saveInitializer = GetComponent<SaveInitializer>();
            BootScreens();
            Time.timeScale = 1;
        }

        void Start()
        {
            if (startConnected)
                Ball.Reconnect();
            else
                Ball.Disconnect();
        }

        private void OnEnable()
        {
            onVictory += InitiateVictory;
            Timer.onTimeOut += InitiateDefeat;
        }

        private void OnDisable()
        {
            onVictory -= InitiateVictory;
            Timer.onTimeOut += InitiateDefeat;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Pause();
        }
        #endregion

        public void Pause()
        {
            if (levelEnded) return;
            if (Time.timeScale == 0)
            {
                pauseScreen.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0;
            }
        }

        #region//Victory
        public static void Victory()
        {
            onVictory?.Invoke();
        }

        public void InitiateVictory()
        {
            FindObjectOfType<SFXManager>().PlaySFX(SFXType.done);
            saveInitializer.SaveGame();
            levelEnded = true;
            Time.timeScale = 0;
            victoryScreen.SetActive(true);
        }

        public void GoToNextLevel()
        {
           SceneManager.LoadScene(nextLevel);
        }
    #endregion

        #region//Defeat
        public void InitiateDefeat()
        {
            levelEnded = true;
            defeatScreen.SetActive(true);
        }
        #endregion

        #region//Other
        private void BootScreens()
        {
            pauseScreen.SetActive(true);
            victoryScreen.SetActive(true);
            defeatScreen.SetActive(true);
            pauseScreen.SetActive(false);
            victoryScreen.SetActive(false);
            defeatScreen.SetActive(false);
        }
        #endregion

        #region//Saving
        private void InitializePreviousBest()
        {
            prevBest.Add("time", 0f);
            saveInitializer.LoadGame();
        }

        public object SaveState()
        {
            InitializePreviousBest();
            Dictionary<string, object> state = new Dictionary<string, object>();

            state["time"] = Mathf.Max(FindObjectOfType<Timer>().GetTimeLeft(), (float)prevBest["time"]);
            state["finish"] = true;
            return state;
        }

        public void LoadState(object _state)
        {
            Dictionary<string, object> copyState = (Dictionary<string, object>)_state;
            prevBest["time"] = copyState["time"];
        }
        #endregion
    }
}
