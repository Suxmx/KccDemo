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
        [BoxGroup("数据设置")] public float StableGroundAccelerationSpeed = 70;
        [BoxGroup("数据设置")] public float StableGroundDecelerationSpeed = 15;
        [BoxGroup("数据设置")] public float JumpUpSpeed = 10;
        [BoxGroup("数据设置")] public float AirMoveSpeed = 12;
        [BoxGroup("数据设置")] public float AirAccelerationSpeed = 50;
        [BoxGroup("数据设置")] public float MaxRotateSpeed = 150;
        [BoxGroup("数据设置")] public Vector3 Gravity = new Vector3(0, -9.8f, 0);

        [BoxGroup("调试信息"), Sirenix.OdinInspector.ReadOnly]
        public Vector3 DebugForward;

        [BoxGroup("调试信息"), Sirenix.OdinInspector.ReadOnly]
        public Vector3 FinalVelocity;

        [BoxGroup("调试信息"), Sirenix.OdinInspector.ReadOnly]
        public float SpeedMagnitude;

        [BoxGroup("调试信息"), Sirenix.OdinInspector.ReadOnly]
        public bool IsStableOnGround;

        private void Awake()
        {
            Motor = GetComponent<KinematicCharacterMotor>();
            Motor.CharacterController = this;
        }

        public void SetInput(PlayerCcInput input)
        {
            Unit.CcData.Forward = input.Forward.normalized;
            Unit.CcData.UpDirection = input.UpDirection.normalized;
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (Unit.CcData.Forward.magnitude != 0)
            {
                currentRotation = Quaternion.RotateTowards(currentRotation,
                    Quaternion.LookRotation(Unit.CcData.Forward),
                    deltaTime * MaxRotateSpeed);
            }
        }


        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            Unit.CcData.StableGroundMoveSpeed = StableGroundMoveSpeed;
            Unit.CcData.StableGroundAccelerationSpeed = StableGroundAccelerationSpeed;
            Unit.CcData.StableGroundDecelerationSpeed = StableGroundDecelerationSpeed;
            Unit.CcData.JumpUpSpeed = JumpUpSpeed;
            Unit.CcData.AirMoveSpeed = AirMoveSpeed;
            Unit.CcData.Gravity = Gravity;
            Unit.CcData.AirAccelerationSpeed = AirAccelerationSpeed;

            if (Unit.TryGetModule<MoveModule>(out var moveModule))
            {
                moveModule.UpdateVelocity(Motor, ref currentVelocity, deltaTime);
            }

            if (!Motor.GroundingStatus.IsStableOnGround)
            {
                AddGravity(ref currentVelocity, deltaTime);
            }

            FinalVelocity = currentVelocity;
            DebugForward = Unit.CcData.Forward;
            IsStableOnGround = Motor.GroundingStatus.IsStableOnGround;
            SpeedMagnitude = Motor.Velocity.magnitude;
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            GatherStateContext();
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

        #region 状态上下文

        private void GatherStateContext()
        {
        }

        #endregion
    }
}