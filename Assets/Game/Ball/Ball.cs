using GMTK2021.EVENT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GMTK2021.BALL
{
    public class Ball : MonoBehaviour
    {
        #region//Cached variables
        BoxCollider2D myCollider;
        Ball otherBall;
        #endregion

        #region//Ball type
        static bool isConnected = true;
        float ballRadius = 0.25f;
        [SerializeField] bool isFirstBall = false;
        [SerializeField] bool isTitle = false;
        public static event Action<float> ReConnected;
        #endregion

        #region//Movement
        float moveSpeed = .2f;
        int xUnitDiff = 0;
        int yUnitDiff = 0;
        static int xJointDiff = 0;
        static int yJointDiff = 0;
        public bool freeze = false;
        static bool xCollide = false;
        static bool yCollide = false;
        #endregion

        #region//Collision
        [SerializeField] LayerMask wallCollision = -1;
        [SerializeField] LayerMask eventCollision = -1;
        [SerializeField] LayerMask firstCollision = -1;
        [SerializeField] LayerMask secondCollision = -1;
        [SerializeField] LayerMask connectionCollision = -1;
        #endregion


        #region//Monobehaviour
        private void Awake()
        {
            isConnected = true;
            myCollider = GetComponent<BoxCollider2D>();
            Ball[] balls = FindObjectsOfType<Ball>();
            otherBall = (balls[0].gameObject == gameObject ? balls[1] : balls[0]);
        }

        private void OnEnable()
        {
            if(!isFirstBall) ReConnected += FixPosition;
        }

        private void OnDisable()
        {
            if(!isFirstBall) ReConnected -= FixPosition;
        }

        void FixedUpdate()
        {
            if (freeze) return;

            if (isConnected)
            {
                if (!isFirstBall) return;

                //Determine collisions
                Vector2 newMe = new Vector2(transform.position.x + xJointDiff * moveSpeed, transform.position.y + yJointDiff * moveSpeed);
                Vector2 newThem = new Vector2(otherBall.transform.position.x + xJointDiff * moveSpeed, otherBall.transform.position.y + yJointDiff * moveSpeed);
                Collider2D[] iColliders = Physics2D.OverlapBoxAll(newMe, myCollider.bounds.extents, 0, wallCollision);
                Collider2D[] iCollidersBall = Physics2D.OverlapBoxAll(newMe, myCollider.bounds.extents, 0, secondCollision);

                Collider2D[] theyColliders = Physics2D.OverlapBoxAll(newThem, otherBall.myCollider.bounds.extents, 0, wallCollision);
                Collider2D[] theyCollidersBall = Physics2D.OverlapBoxAll(newThem, otherBall.myCollider.bounds.extents, 0, firstCollision);

                List<Collider2D> collisions = new List<Collider2D>();
                List<bool> isMe = new List<bool>();

                //Sort collisions by distance and preform movement check
                foreach (Collider2D collision in iColliders)
                {
                    collisions.Add(collision);
                    isMe.Add(true);
                }

                foreach (Collider2D collision in iCollidersBall)
                {
                    collisions.Add(collision);
                    isMe.Add(true);
                }

                foreach (Collider2D collision in theyColliders)
                {
                    collisions.Add(collision);
                    isMe.Add(false);
                }

                foreach (Collider2D collision in theyCollidersBall)
                {
                    collisions.Add(collision);
                    isMe.Add(false);
                }

                while (collisions.Count > 0)
                {
                    Collider2D currCollide = collisions[0];
                    bool me = isMe[0];
                    float currDist = currCollide.Distance(me ? myCollider : otherBall.myCollider).distance;
                    int index = 0;
                    for (int ii = 1; ii < collisions.Count; ii++)
                    {
                        float thisDist = collisions[ii].Distance(isMe[ii] ? myCollider : otherBall.myCollider).distance;
                        if (thisDist < currDist)
                        {
                            currCollide = collisions[ii];
                            me = isMe[ii];
                            currDist = thisDist;
                            index = ii;
                        }
                    }

                    if (me)
                        WallCollide(currCollide, this, otherBall, ref xJointDiff, ref yJointDiff);
                    else
                        WallCollide(currCollide, otherBall, this, ref xJointDiff, ref yJointDiff);
                    collisions.RemoveAt(index);
                    isMe.RemoveAt(index);
                }

                //Do valid move
                transform.Translate(xJointDiff * moveSpeed, yJointDiff * moveSpeed, 0);
                otherBall.transform.Translate(xJointDiff * moveSpeed, yJointDiff * moveSpeed, 0);
            }
            else
            {
                //Determine collisions
                Vector2 newMe = new Vector2(transform.position.x + xUnitDiff * moveSpeed, transform.position.y + yUnitDiff * moveSpeed);
                Collider2D[] iColliders = Physics2D.OverlapBoxAll(newMe, new Vector2(ballRadius, ballRadius), 0, wallCollision);
                Collider2D[] iCollidersBall = Physics2D.OverlapBoxAll(newMe, new Vector2(ballRadius, ballRadius), 0, (isFirstBall ? secondCollision : firstCollision));
                Collider2D[] iCollidersConnection = Physics2D.OverlapBoxAll(newMe, new Vector2(ballRadius, ballRadius), 0, connectionCollision);
                List<Collider2D> collisions = new List<Collider2D>();

                //Sort collisions by distance and preform movement check
                foreach (Collider2D collision in iColliders)
                    collisions.Add(collision);

                foreach (Collider2D collision in iCollidersBall)
                    collisions.Add(collision);

                foreach (Collider2D collision in iCollidersConnection)
                    collisions.Add(collision);


                //Adjsut position
                foreach (Collider2D collision in collisions)
                    WallCollide(collision, this, null, ref xUnitDiff, ref yUnitDiff);

                //Do valid move
                transform.Translate(xUnitDiff * moveSpeed, yUnitDiff * moveSpeed, 0);
            }
        }

        void Update()
        {
            if (isTitle) return;

            xCollide = false;
            yCollide = false;

            if (isConnected)
            {
                if (isFirstBall)
                {
                    CalcMove(ref xJointDiff, ref yJointDiff);
                    xUnitDiff = 0;
                    yUnitDiff = 0;
                }
            }
            else
            {
                CalcMove(ref xUnitDiff, ref yUnitDiff);
                xJointDiff = 0;
                yJointDiff = 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.GetComponent<IEvent>() != null)
            {
                IEvent triggered = collision.GetComponent<IEvent>();
                triggered.ActivateEvent(this);
            }
        }
        
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<IEvent>() != null)
            {
                IEvent triggered = collision.GetComponent<IEvent>();
                triggered.DeactivateEvent(this);
            }
        }
        #endregion

        #region//Connection
        public static void Disconnect() { isConnected = false; }
        public static void Reconnect() { isConnected = true; }
        public void FixPosition(float _space)
        {
            transform.position = new Vector2(otherBall.transform.position.x + _space, otherBall.transform.position.y);
        }
        #endregion

        #region//Getters
        public static bool GetIsConnected() { return isConnected; }
        public bool IsFirst() { return isFirstBall; }
        #endregion

        #region//Movement methods
        int XMovementCalc()
        {
            if (isFirstBall)
                return (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
            else
                return (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        }

        int YMovementCalc()
        {
            if (isFirstBall)
                return (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
            else
                return (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
        }

        void CalcMove(ref int _xMove, ref int _yMove)
        {
            //Move
            _xMove = XMovementCalc();
            _yMove = YMovementCalc();

            if(isConnected)
            {
                _xMove += otherBall.XMovementCalc();
                _yMove += otherBall.YMovementCalc();
            }

            //Bind values
            _xMove = Mathf.Clamp(_xMove, -1, 1);
            _yMove = Mathf.Clamp(_yMove, -1, 1);
        }
        #endregion

        #region//Collison methods
        void WallCollide(Collider2D _collision, Ball _colliding, Ball _other, ref int _xMove, ref int _yMove)
        {
            ColliderDistance2D dist = _collision.Distance(_colliding.myCollider);

            //X collision
            if ((!isConnected || !xCollide) && Mathf.Abs(dist.normal[0]) > 0.1)
            {
                xCollide = true;
                float xDist = 0;
                if(_other) xDist = _colliding.transform.position.x - _other.transform.position.x;
                _colliding.transform.position = new Vector2(dist.pointA.x + Mathf.Sign(dist.normal[0]) * _colliding.ballRadius * 1.01f * (dist.isOverlapped ? 1 : -1), _colliding.transform.position.y);
                if(_other) _other.transform.position = new Vector2(_colliding.transform.position.x - xDist, _colliding.transform.position.y);
                _xMove = 0;
            }

            //Y collision
            if ((!isConnected || !xCollide) && Mathf.Abs(dist.normal[1]) > 0.1)
            {
                yCollide = true;
                _colliding.transform.position = new Vector2(_colliding.transform.position.x, dist.pointA.y + Mathf.Sign(dist.normal[1]) * _colliding.ballRadius * 1.01f * (dist.isOverlapped ? 1 : -1));
                if(_other) _other.transform.position = new Vector2(_other.transform.position.x, _colliding.transform.position.y);
                _yMove = 0;
            }
        }
        #endregion
    }
}