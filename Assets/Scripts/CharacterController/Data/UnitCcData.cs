using UnityEngine;

namespace CharacterController
{
    public class UnitCcData
    {
        public Vector3 Forward;
        public Vector3 UpDirection;
        public Vector3 Gravity;

        public float StableGroundMoveSpeed = 10;
        public float StableGroundAccelerationSpeed = 10;
        public float StableGroundDecelerationSpeed = 15;
        public float JumpUpSpeed = 10;
        public float AirMoveSpeed = 12;
    }
}