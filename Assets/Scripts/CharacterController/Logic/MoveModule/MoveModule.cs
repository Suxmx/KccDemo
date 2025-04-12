using CharacterController;
using KinematicCharacterController;
using UnityEngine;

namespace CCCD
{
    public class MoveModule : UnitModuleBase
    {
        public MoveModule(Unit unit) : base(unit)
        {
        }

        public void UpdateVelocity(KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
            UnitCcData ccData = Unit.CcData;
            // 如果在地面上
            if (motor.GroundingStatus.IsStableOnGround)
            {
                Debug.Log("onground");
                // 重映射当前速度
                float curVelocityMagnitude = currentVelocity.magnitude;
                Vector3 groundNormal = motor.GroundingStatus.GroundNormal;
                Vector3 reorientedVelocity = motor.GetDirectionTangentToSurface(currentVelocity, groundNormal) *
                                             curVelocityMagnitude;
                Debug.Log($"groundNormal: {groundNormal} reorientedVelocity: {reorientedVelocity}");
                // 重映射输入
                Vector3 inputRight = Vector3.Cross(ccData.Forward, ccData.UpDirection);
                Vector3 reorientedInput = Vector3.Cross(ccData.UpDirection, inputRight);
                Vector3 targetVelocity = reorientedInput * ccData.StableGroundMoveSpeed;
                Debug.Log($"reorientedInput:{reorientedInput} targetVelocity:{targetVelocity}");

                currentVelocity = Vector3.MoveTowards(reorientedVelocity, targetVelocity,
                    ccData.StableGroundAccelerationSpeed * deltaTime);
            }
            else // 在空中
            {
                
            }
        }
    }
}