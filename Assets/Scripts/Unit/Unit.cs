using System;
using System.Collections.Generic;
using CharacterController;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CCCD
{
    public class Unit : MonoBehaviour
    {
        public CinemachineVirtualCameraBase VirtualCamera;
        public MyCharacterController CharacterController;
        public UnitCcData CcData = new();
        [BoxGroup("Slot")] public Transform CameraSlot;

        private Vector3 debugForward;

        private void Awake()
        {
            //Init Global
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //Init CC
            CharacterController = GetComponent<MyCharacterController>();
            AddModule(new MoveModule(this));
        }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
            Vector3 input = new Vector3(horizontal, 0, vertical);
            if (input != Vector3.zero)
            {
                forward = Quaternion.LookRotation(input) * forward * input.magnitude;
            }
            else
            {
                forward = Vector3.zero;
            }

            debugForward = forward;
            Vector3 up = Vector3.up;
            CharacterController.SetInput(new PlayerCcInput(forward, up));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, debugForward.normalized * 5);
            Gizmos.color = Color.white;
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