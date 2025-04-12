using System;
using System.Collections.Generic;
using CharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CCCD
{
    public class Unit : MonoBehaviour
    {
        public MyCharacterController CharacterController;
        public UnitCcData CcData = new();
        [BoxGroup("Slot")] public Transform CameraSlot;

        private void Awake()
        {
            CharacterController = GetComponent<MyCharacterController>();
            AddModule(new MoveModule(this));
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 forward = new Vector3(horizontal, 0, vertical);
            Vector3 up = Vector3.up;
            CharacterController.SetInput(new PlayerCcInput(forward, up));
        }

        #region Module

        private Dictionary<Type, UnitModuleBase> _modules = new();

        public void AddModule(UnitModuleBase module)
        {
            if (module == null) return;
            if (!_modules.ContainsKey(module.GetType()))
            {
                _modules.Add(module.GetType(), module);
            }
        }

        public bool TryGetModule<T>(out T module) where T : UnitModuleBase
        {
            Type type = typeof(T);
            if (_modules.TryGetValue(type, out var m))
            {
                module = (T)m;
                return true;
            }
            else
            {
                module = null;
                return false;
            }
        }

        #endregion
    }
}