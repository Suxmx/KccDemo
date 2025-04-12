using UnityEngine;

namespace CharacterController
{
    public struct PlayerCcInput
    {
        public PlayerCcInput(Vector3 forward, Vector3 upDirection)
        {
            Forward = forward;
            UpDirection = upDirection;
        }

        public Vector3 Forward;
        public Vector3 UpDirection;
    }
}