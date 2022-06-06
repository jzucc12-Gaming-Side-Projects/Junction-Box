using GMTK2021.BALL;

namespace GMTK2021.EVENT
{
    public interface IEvent
    {
        void ActivateEvent(Ball _ball);
        void DeactivateEvent(Ball _ball);
    }

}