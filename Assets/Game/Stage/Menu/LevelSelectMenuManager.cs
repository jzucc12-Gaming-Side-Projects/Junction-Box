using System;
using UnityEngine;

namespace SAVING
{
    public class LevelSelectMenuManager : MonoBehaviour
    {
        public static event Action DataDeleted;
        public static event Action LoadingDone;
        [SerializeField] GameObject firstSet;

        private void Start()
        {
            Time.timeScale = 1;
            GetComponent<SaveInitializer>().LoadGame();
            LoadingDone?.Invoke();
        }

        public void OnDelete()
        {
            firstSet.SetActive(true);
            FindObjectOfType<SaveInitializer>().DeleteGame();
            DataDeleted?.Invoke();
        }
    }
}
