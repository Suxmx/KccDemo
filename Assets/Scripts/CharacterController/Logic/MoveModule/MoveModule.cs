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
                Vector3 addVelocity = Vector3.ProjectOnPlane(ccData.Forward, ccData.UpDirection) *
                                      (ccData.AirAccelerationSpeed * deltaTime);
                Vector3 curVelocityOnPlane = Vector3.ProjectOnPlane(currentVelocity, ccData.UpDirection);
                // 可继续加速
                if (curVelocityOnPlane.magnitude < ccData.AirMoveSpeed)
                {
                    Vector3 newTotalVelocity =
                        Vector3.ClampMagnitude(addVelocity + curVelocityOnPlane, ccData.AirMoveSpeed);
                    addVelocity = newTotalVelocity - curVelocityOnPlane;
                    Debug.Log($"cur magnitude:{curVelocityOnPlane.magnitude} addVelocity: {addVelocity}");
                }
                else
                {
                    // 防止继续加速
                    if (Vector3.Dot(curVelocityOnPlane, addVelocity) > 0f)
                    {
                        addVelocity = Vector3.ProjectOnPlane(addVelocity, curVelocityOnPlane.normalized);
                    }
                }

                currentVelocity += addVelocity;
            }
        }
    }
}