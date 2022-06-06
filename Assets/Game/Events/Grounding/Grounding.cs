using GMTK2021.BALL;
using GMTK2021.SOUND;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK2021.EVENT
{
    public class Grounding : MonoBehaviour, IEvent
    {
        public void ActivateEvent(Ball _ball)
        {
            FindObjectOfType<SFXManager>().PlaySFX(SFXType.ground);
            Ball.Disconnect();
        }

        public void DeactivateEvent(Ball _ball)
        {
            return;
        }
    }

}