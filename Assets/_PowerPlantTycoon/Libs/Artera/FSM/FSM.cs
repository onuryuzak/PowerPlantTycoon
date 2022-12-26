
using System.Collections.Generic;
using UnityEngine;

namespace Artera.AI
{
    public interface IFSM
    {
        // Execution
        void push(int state);
        void pop();
        void switchState(int state);
        // Creational
        void addState(IState state);
        void addStates(List<IState> states);
        void removeState(IState state);
        void removeStates(int index, int count);
        void restart();
        bool isInitialized { get; }
        GameObject gameObject { get; }
        T getComponent<T>();
    }

    abstract public class FSM : MonoBehaviour, IFSM
    {
        public System.Action onInitilized;
        protected List<IState> _states;
        protected Stack<IState> _stack;
        protected IState _previousState;
        protected IState _currentState;

        private bool _isActive = true;
        private bool _initialized;
        private int _initalState = -1;

        #region BASE
        private void Awake()
        {
            _stack = new Stack<IState>();
        }

        void Start()
        {
            initialize();
        }

        private void OnEnable()
        {
            if (_initialized)
            {
                runFSM();// This method will be called after first initialization.
            }
            active = true;
        }

        private void OnDisable()
        {
            active = false;
        }

        void Update()
        {
            if (_currentState != null && _isActive)
            {
                _currentState.onUpdate();
            }
        }

        void LateUpdate()
        {
            if (_currentState != null && _isActive)
            {
                _currentState.onLateUpdate();
            }
        }
        #endregion

        #region METHODS
        void initialize()
        {
            // Must be overrided!
            onInitialize();

            active = true;
            _initialized = true;
            runFSM();
            onInitilized?.Invoke();
        }

        // Initialization Methods
        virtual protected void onInitialize()
        {
            throw new System.Exception("[FSM::onInitialize] onInitialize method MUST be overrided!");
        }

        void runFSM()
        {
            if (_currentState != null)
            {
                push(_currentState.stateID);
            }
            else if (_states.Count > 0)
            {
                Debug.LogWarningFormat("[FSM::runFSM] Initial state is not assigned. First state will be initialized automatically.");
                _currentState = _states[0];
                push(0);
            }
        }

        public void setInitialState(int state)
        {
            if (state >= _states.Count || state < 0)
            {
                throw new System.Exception(string.Format("[FSM:setInitialState] Out of index array! List count: {0} Current Index: {1}", _states.Count, state));
            }
            _initalState = state;
            _currentState = bringState(_initalState);
        }
        #endregion

        #region INTERFACE
        // Execution Methods
        public void push(int state)
        {
            if (!_isActive) { Debug.Log("FSM::push SM is not active yet!"); return; }

            if (_stack.Count > 0)
            {
                IState currentState = _stack.Peek();
                currentState.onExit();
            }
            IState targetState = bringState(state);
            _stack.Push(targetState);
            _currentState = targetState;
            targetState.onEnter();
        }

        public void pop()
        {
            if (!_isActive) { Debug.Log("FSM::pop SM is not active yet!"); return; }

            if (_stack.Count > 1)
            {
                IState currentState = _stack.Pop();
                IState targetState = _stack.Peek();

                _currentState = targetState;
                currentState.onExit();
                targetState.onEnter();
            }
            else
            {
                Debug.LogWarningFormat("[FSM::runFSM] Last state of stack cannot be removed.");
            }
        }

        public void switchState(int state)
        {
            if (!_isActive) { Debug.Log("FSM::switchState SM is not active yet!"); return; }
            if (_states == null) { Debug.Log("FSM::changeState States is not initialized yet."); return; }
            if (_currentState.stateID == state)
            {
                Debug.Log($"FSM::changeState Already same state: {_currentState.GetType()}");
                return;
            }

            IState targetState = bringState(state);
            if (_currentState != null)
            {
                _currentState.onExit();
                _stack.Pop();
            }
            _stack.Push(targetState);
            _currentState = targetState;
            targetState.onEnter();
        }

        // Creational Methods
        public void addState(IState state)
        {
            _states.Add(state);
        }

        public void addStates(List<IState> states)
        {
            _states.AddRange(states);
        }

        public void removeState(IState state)
        {
            _states.Remove(state);
        }

        public void removeStates(int index, int count)
        {
            _states.RemoveRange(index, count);
        }

        public void restart()
        {
            if (_currentState != null)
            {
                _currentState.onExit();
            }

            _initalState = (_initalState != -1) ? _initalState : 0;
            setInitialState(_initalState);
            _currentState.onEnter();
        }

        public T getComponent<T>()
        {
            return base.GetComponent<T>();
        }
        #endregion

        #region HELPER
        public bool active { set => _isActive = value; get => _isActive; }
        public bool isInitialized => _initialized;

        
        public IState activeState => _currentState;
        public IState bringState(int state) => _states.Find((obj) => obj.stateID == state);
        #endregion
    }
}