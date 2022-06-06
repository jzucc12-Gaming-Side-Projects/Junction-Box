using GMTK2021.BALL;
using GMTK2021.SOUND;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK2021.EVENT
{
    public class Attractor : MonoBehaviour, IEvent
    {
        #region//Attractor variables
        [SerializeField] int set = 0;
        [SerializeField] float ballSpace = 6;
        float maxTravel = .008f;
        static bool[,] amReady = new bool[10, 2];
        #endregion


        #region/Monobehaviour
        private void Awake()
        {
            amReady[set, 0] = false;
            amReady[set, 1] = false;
        }

        private void Update()
        {
            if (amReady[set, 0] && amReady[set, 1])
            {
                FindObjectOfType<SFXManager>().PlaySFX(SFXType.attract);
                Ball.Reconnect();
                amReady[set, 0] = false;
                amReady[set, 1] = false;
            }
        }
        #endregion

        #region//Event
        public void ActivateEvent(Ball _ball)
        {
            if (Ball.GetIsConnected()) return;
            if (_ball.IsFirst()) StartCoroutine(PullIn(_ball, true));
            else if (!_ball.IsFirst()) StartCoroutine(PullIn(_ball, false));
        }

        public void DeactivateEvent(Ball _ball)
        {
            if (Ball.GetIsConnected()) return;
            if (_ball.IsFirst()) amReady[set, 0] = false;
            else if (!_ball.IsFirst()) amReady[set, 1] = false;
        }

        IEnumerator PullIn(Ball _ball, bool _isFirst)
        {
            _ball.freeze = true;

            do
            {
                if(Time.timeScale != 0) _ball.transform.position = Vector2.MoveTowards(_ball.transform.position, transform.position, maxTravel);
                yield return null;
            }
            while (transform.position != _ball.transform.position);

            if (_isFirst) amReady[set, 0] = true;
            else amReady[set, 1] = true;
            _ball.freeze = false;
        }
        #endregion
    }
}
