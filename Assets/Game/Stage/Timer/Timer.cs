using System;
using TMPro;
using UnityEngine;

namespace GMTK2021.STAGE
{
    public class Timer : MonoBehaviour
    {
        #region//Timer properties
        [SerializeField] float baseMaxTime = 15;
        [SerializeField] TextMeshProUGUI timerText = null;
        float maxTimeModifier = 1;
        float currMaxTime;
        float timeLeft;
        bool timeOut = false;
        bool runTimer = false;
        public static event Action onTimeOut;
        #endregion


        #region//Monobehaviour
        void Awake()
        {
            ResetTimer();
            StartTimer();
        }

        void FixedUpdate()
        {
            if (runTimer && timeLeft > 0)
            {
                timeLeft = Mathf.Max(0, timeLeft - Time.deltaTime);
                UpdateUI(timeLeft);
            }
            else if(timeLeft <= 0)
            {
                timeOut = true;
                StopTimer();
            }
        }
        #endregion

        void UpdateUI(float _time)
        {
            int mins = (int)(_time / 60);
            int secs = (int)(_time % 60);
            string pad = ":";
            if (secs < 10) pad += "0";
            timerText.text = (mins + pad + secs);
        }

        #region//Start up
        public void ResetTimer()
        {
            currMaxTime = baseMaxTime * maxTimeModifier;
            timeLeft = currMaxTime;
            timeOut = false;
        }

        public void StartTimer()
        {
            runTimer = true;
        }

        public void StopTimer()
        {
            runTimer = false;
            if (PlayerPrefs.GetInt("enforce") == 0) return;
            onTimeOut?.Invoke();
        }
        #endregion

        #region//Getters
        public bool GetTimeOut() { return timeOut; }
        public float GetTimeLeft() { return timeLeft; }
        #endregion
    }
}