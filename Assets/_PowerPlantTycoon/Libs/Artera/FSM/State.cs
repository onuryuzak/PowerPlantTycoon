
namespace Artera.AI
{
    public interface IState
    {
        void onEnter();
        void onExit();
        void onUpdate();
        void onLateUpdate();
        int stateID { get; }
    }

    public class State : IState
    {

        protected int _stateId;
        protected IFSM _owner;
        protected IState _prevState;
        protected IState _nextState;
        protected string _name;

        public State(int stateId, IFSM owner)
        {
            _stateId = stateId;
            _owner = owner;
        }

        virtual public void onEnter()
        {
            // Override on subclass
        }

        virtual public void onExit()
        {
            // Override on subclass
        }

        virtual public void onUpdate()
        {
            // Override on subclass
        }

        virtual public void onLateUpdate()
        {
            // Override on subclass
        }

        public int stateID => _stateId;
    }
}