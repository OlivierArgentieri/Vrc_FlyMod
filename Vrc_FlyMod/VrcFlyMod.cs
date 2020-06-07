using MelonLoader;
using UnityEngine;

namespace Vrc_FlyMod
{
    /// <summary>
    /// F1 : Increase UP
    /// F2 : Decrease Up
    /// F3 : Not used
    /// F4 : Enable/Disable
    /// F5 : Increase Speed
    /// F6 : Decrease Speed
    /// </summary>
    public class VrcFlyMod : MelonMod
    {
        
        #region f/p

        private const float minSpeed = 0.01f;
        private const float maxSpeed = 1f;

        private Vector3 editedPosition = Vector3.zero;
        private bool isFlyActive = false;
        private float speed = 0.01f;


        public float Speed
        {
            get => speed;
            set
            {
                speed = value;

                if (speed < minSpeed)
                    speed = minSpeed;
                if (speed > maxSpeed)
                    speed = maxSpeed;
            }
        }

        private VRCPlayer localPlayer
        {
            get => VRCPlayer.field_Internal_Static_VRCPlayer_0;
            set => VRCPlayer.field_Internal_Static_VRCPlayer_0 = value;
        }

        public bool IsValid => localPlayer;

        #endregion


        #region unity methods

        public override void OnUpdate()
        {
            if (!IsValid) return;

            if (Input.GetKey(KeyCode.F1))
                IncreaseUp();

            if (Input.GetKey(KeyCode.F2))
                DecreaseUp();

            if (Input.GetKeyDown(KeyCode.F5))
                IncreaseSpeed();

            if (Input.GetKeyDown(KeyCode.F6))
                DecreaseSpeed();

            if (Input.GetKeyDown(KeyCode.F4))
                ToggleFly();

            Move();

            UpdatePosition();
        }

        #endregion



        #region custom methods

        void IncreaseUp()
        {
            editedPosition += Vector3.up * Speed;
        }

        void DecreaseUp()
        {
            editedPosition -= Vector3.up * Speed;
        }

        void IncreaseSpeed()
        {
            Speed += 0.01f;
        }

        void DecreaseSpeed()
        {
            Speed -= 0.01f;
        }


        void ToggleFly()
        {
            isFlyActive = !isFlyActive;
        }

        void Move()
        {
            if (!isFlyActive) return;

            float _horizontal = Input.GetAxis("Horizontal");
            float _vertical = Input.GetAxis("Vertical");

            editedPosition += localPlayer.transform.right * Speed * _horizontal;
            editedPosition += localPlayer.transform.forward * Speed * _vertical;
        }

        void UpdatePosition()
        {
            if (!localPlayer) return;

            if (isFlyActive)
                localPlayer.transform.position = editedPosition;
            else
                editedPosition = localPlayer.transform.position;
        }

        #endregion
    }
}
