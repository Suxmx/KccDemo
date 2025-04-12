using System;
using CharacterController;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CCCD
{
    public class MyCharacterController : MonoBehaviour, ICharacterController
    {
        [BoxGroup("引用")] public Unit Unit;
        [BoxGroup("引用")] public KinematicCharacterMotor Motor;
        [BoxGroup("数据设置")] public float StableGroundMoveSpeed = 10;
        [BoxGroup("数据设置")] public float StableGroundAccelerationSpeed = 10;
        [BoxGroup("数据设置")] public float StableGroundDecelerationSpeed = 15;
        [BoxGroup("数据设置")] public float JumpUpSpeed = 10;
        [BoxGroup("数据设置")] public float AirMoveSpeed = 12;
        [BoxGroup("数据设置")] public Vector3 Gravity = new Vector3(0, -9.8f, 0);

        private void Awake()
        {
            Motor = GetComponent<KinematicCharacterMotor>();
            Motor.CharacterController = this;
        }

        public void SetInput(PlayerCcInput input)
        {
            Unit.CcData.Forward = input.Forward;
            Unit.CcData.UpDirection = input.UpDirection;
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            Unit.CcData.StableGroundMoveSpeed = StableGroundMoveSpeed;
            Unit.CcData.StableGroundAccelerationSpeed = StableGroundAccelerationSpeed;
            Unit.CcData.StableGroundDecelerationSpeed = StableGroundDecelerationSpeed;
            Unit.CcData.JumpUpSpeed = JumpUpSpeed;
            Unit.CcData.AirMoveSpeed = AirMoveSpeed;
            Unit.CcData.Gravity = Gravity;

            if (Unit.TryGetModule<MoveModule>(out var moveModule))
            {
                moveModule.UpdateVelocity(Motor, ref currentVelocity, deltaTime);
            }

            AddGravity(ref currentVelocity, deltaTime);
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }

        public void AddGravity(ref Vector3 currentVelocity, float deltaTime)
        {
            currentVelocity += Gravity * deltaTime;
        }
    }
}