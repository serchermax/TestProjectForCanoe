using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Input
{
    public class PlayerMoveInput : MonoBehaviour
    {          
        public enum MoveInputTypes
        {
            Up = 0,
            Down = 1,
            Right = 2,
            Left = 3,
            Move = 4
        }
        public delegate void actionDelegate();

        public static PlayerMoveInput Instance;
        public bool PlayerIsStand { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }

        private Dictionary<MoveInputTypes, actionDelegate> _inputEventMap;
        private Dictionary<MoveInputTypes, actionDelegate> _inputStartEventMap;
        private Dictionary<MoveInputTypes, bool> _inputStartCheck;
        private Dictionary<MoveInputTypes, actionDelegate> _inputStopEventMap;
        private Dictionary<MoveInputTypes, bool> _inputStopCheck;


        #region ListenersControl

        // Input //
        public void AddListenerOnInput(actionDelegate call) => AddListenerOnInput(call, MoveInputTypes.Move);
        public void AddListenerOnInput(actionDelegate call, MoveInputTypes type) => _inputEventMap[type] += call;

        public void RemoveListenerOnInput(actionDelegate call) => RemoveListenerOnInput(call, MoveInputTypes.Move);
        public void RemoveListenerOnInput(actionDelegate call, MoveInputTypes type)
        {
            if (_inputEventMap[type] != null) _inputEventMap[type] -= call;
        }

        // StartInput //
        public void AddListenerOnStartInput(actionDelegate call) => AddListenerOnStartInput(call, MoveInputTypes.Move);
        public void AddListenerOnStartInput(actionDelegate call, MoveInputTypes type) => _inputStartEventMap[type] += call;

        public void RemoveListenerOnStartInput(actionDelegate call) => RemoveListenerOnStartInput(call, MoveInputTypes.Move);
        public void RemoveListenerOnStartInput(actionDelegate call, MoveInputTypes type)
        {
            if (_inputStartEventMap[type] != null) _inputStartEventMap[type] -= call;
        }

        // StopInput //
        public void AddListenerOnStopInput(actionDelegate call) => AddListenerOnStopInput(call, MoveInputTypes.Move);
        public void AddListenerOnStopInput(actionDelegate call, MoveInputTypes type) => _inputStopEventMap[type] += call;

        public void RemoveListenerOnStopInput(actionDelegate call) => RemoveListenerOnStopInput(call, MoveInputTypes.Move);
        public void RemoveListenerOnStopInput(actionDelegate call, MoveInputTypes type)
        {
            if (_inputStopEventMap[type] != null) _inputStopEventMap[type] -= call;
        }

        #endregion

        private void Awake() => Initialize();
        private void Initialize()
        {
            Instance = this;
            _inputEventMap = new Dictionary<MoveInputTypes, actionDelegate>();
            _inputStartEventMap = new Dictionary<MoveInputTypes, actionDelegate>();
            _inputStartCheck = new Dictionary<MoveInputTypes, bool>();
            _inputStopEventMap = new Dictionary<MoveInputTypes, actionDelegate>();
            _inputStopCheck = new Dictionary<MoveInputTypes, bool>();

            for (int i = 0; i < Enum.GetValues(typeof(MoveInputTypes)).Length; i++)
            {
                _inputEventMap[(MoveInputTypes)i] = null;
                _inputStartEventMap[(MoveInputTypes)i] = null;
                _inputStartCheck[(MoveInputTypes)i] = false;
                _inputStopEventMap[(MoveInputTypes)i] = null;
                _inputStopCheck[(MoveInputTypes)i] = true;
            }
        }

        private void Update() => InputCheck();
        private void InputCheck()
        {
            X = UnityEngine.Input.GetAxis(Constans.X_AXIS_NAME);
            Y = UnityEngine.Input.GetAxis(Constans.Y_AXIS_NAME);
            PlayerIsStand = X == 0 && Y == 0;

            Call(Y > 0, MoveInputTypes.Up);
            Call(Y < 0, MoveInputTypes.Down);
            Call(X > 0, MoveInputTypes.Right);
            Call(X < 0, MoveInputTypes.Left);
            Call(!PlayerIsStand, MoveInputTypes.Move);
        }

        private void Call(bool condition, MoveInputTypes type)
        {
            if (condition)
            {
                if (!_inputStartCheck[type])
                {
                    _inputStartEventMap[type]?.Invoke();
                    _inputStartCheck[type] = true;
                    _inputStopCheck[type] = false;
                }
                _inputEventMap[type]?.Invoke();                
            }
            else if (!_inputStopCheck[type])
            {
                _inputStopEventMap[type]?.Invoke();
                _inputStopCheck[type] = true;
                _inputStartCheck[type] = false;
            }
        }        
    }
}
