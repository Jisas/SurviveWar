using UnityEngine;

namespace SurviveWar
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        [Header("Output")]
        public EntityInputs starterAssetsInputs;
        public FixedTouchField touchField;
        public UIManager manager;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            starterAssetsInputs.SprintInput(virtualSprintState);
        }

        public void VirtualCrouchInput(bool virtualCrouchState)
        {
            starterAssetsInputs.CrouchInput(virtualCrouchState);
        }

        public void VirtualAimInput(bool virtualAimState)
        {
            starterAssetsInputs.AimInput(virtualAimState);
        }
        
        public void VirtualShootInput(bool virtualShootState)
        {
            starterAssetsInputs.ShootInput(virtualShootState);
        }

        public void VirtualReloadtInput(bool virtualReloadState)
        {
            starterAssetsInputs.ReloadInput(virtualReloadState);
        }

        public void VirtualInteractInput(bool virtualInteractState)
        {
            starterAssetsInputs.InteractInput(virtualInteractState);
        }

        public void VirtualScreenInteractorInput(bool virtualInteractorState)
        {
            starterAssetsInputs.ScreenInteractionInput(virtualInteractorState);
        }
    }

}
