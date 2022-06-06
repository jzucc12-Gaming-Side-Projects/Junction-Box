using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK2021.BALL
{
    public class ConnectionBar : MonoBehaviour
    {
        Ball firstBall = null;
        Ball secondBall = null;

        [SerializeField] SpriteRenderer barRenderer = null;

        private void Awake()
        {
            foreach (Ball ball in FindObjectsOfType<Ball>())
            {
                if (ball.IsFirst()) firstBall = ball;
                else secondBall = ball;
            }

            SyncWithBall();
        }

        private void Update()
        {
            SyncWithBall();
        }

        void SyncWithBall()
        {
            //Set visibility
            barRenderer.enabled = Ball.GetIsConnected();

            //Set position
            float averageX = (firstBall.transform.position.x + secondBall.transform.position.x) / 2;
            float averageY = (firstBall.transform.position.y + secondBall.transform.position.y) / 2;
            transform.position = new Vector2(averageX, averageY);

            //Set size
            transform.localScale = new Vector2(Mathf.Abs(firstBall.transform.position.x - secondBall.transform.position.x), transform.localScale.y);
        }
    }
}
