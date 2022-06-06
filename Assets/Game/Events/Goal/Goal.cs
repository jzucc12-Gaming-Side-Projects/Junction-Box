using GMTK2021.BALL;
using GMTK2021.STAGE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK2021.EVENT
{
    public class Goal : MonoBehaviour, IEvent
    {
        [SerializeField] SpriteRenderer myRenderer = null;
        [SerializeField] bool connectionRequired = true;
        [SerializeField] bool isFirst = true;
        static bool[] amReady = new bool[] { false, false };
        float maxTravel = .008f;

        public void ActivateEvent(Ball _ball)
        {
            if (connectionRequired && !Ball.GetIsConnected()) return;
            if (!connectionRequired && Ball.GetIsConnected()) return;
            if (isFirst && _ball.IsFirst()) amReady[0] = true;
            else if (!isFirst && !_ball.IsFirst()) amReady[1] = true;
        }

        public void DeactivateEvent(Ball _ball)
        {
            if (isFirst && _ball.IsFirst()) amReady[0] = false;
            else if (!isFirst && !_ball.IsFirst()) amReady[1] = false;
        }

        #region//Monobehaviour
        private void Awake()
        {
            Sprite mySprite;
            if(isFirst)
            {
                if (connectionRequired)
                    mySprite = Resources.Load<Sprite>("Art/Better Assets/Junction A Connection");
                else
                    mySprite = Resources.Load<Sprite>("Art/Better Assets/Junction A");
            }
            else
            {
                if (connectionRequired)
                    mySprite = Resources.Load<Sprite>("Art/Better Assets/Junction B Connection");
                else
                    mySprite = Resources.Load<Sprite>("Art/Better Assets/Junction B");
            }

            myRenderer.sprite = mySprite;
        }

        void OnEnable()
        {
            LevelManager.onVictory += Pull;
        }

        void OnDisable()
        {
            LevelManager.onVictory -= Pull;
        }
        #endregion

        void Pull()
        {
            foreach (Ball ball in FindObjectsOfType<Ball>())
            {
                if (isFirst && ball.IsFirst())
                    StartCoroutine(PullIn(ball));
                else if (!isFirst && !ball.IsFirst())
                    StartCoroutine(PullIn(ball));
            }
        }

        IEnumerator PullIn(Ball _ball)
        {
            do
            {
                _ball.transform.position = Vector2.MoveTowards(_ball.transform.position, transform.position, maxTravel);
                yield return null;
            }
            while (transform.position != _ball.transform.position);
        }

        private void Update()
        {
            if (amReady[0] && amReady[1])
            {
                LevelManager.Victory();
                amReady[0] = false;
                amReady[1] = false;
            }
        }
    }
}
