using System;
using UnityEngine;
using UnityEngine.Events;


#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace SurviveWar
{
	public class EntityInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool Jump { get; set; }
		public bool Sprint { get; set; }
        public bool Crouch { get; set; }
        public bool Aim { get; set; }
        public bool Shoot { get; set; }
        public bool Reload { get; set; }
        public bool SetRifle { get; set; }
        public bool SetGun { get; set; }
        public bool SetKnife { get; set; }
        public bool Interact { get; set; }
        public bool ScreenInteraction { get; set; }

        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        private PlayerInput playerInputs;

        private void Awake() => playerInputs = GetComponent<PlayerInput>();


#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

        public void OnCrouch(InputValue value)
        {
            CrouchInput(value.isPressed);
        }

        public void OnAim(InputValue value)
        {
            AimInput(value.isPressed);
        }

        public void OnShoot(InputValue value)
        {
            if (Reload) return; 
            ShootInput(value.isPressed);
        }

        public void OnReload(InputValue value)
        {
            ReloadInput(value.isPressed);
        }

        public void OnSetRifle(InputValue value)
        {
            SetRifleInput(value.isPressed);
        }

        public void OnSetGun(InputValue value)
        {
            SetGunInput(value.isPressed);
        }

        public void OnSetKnife(InputValue value)
        {
            SetKnifeInput(value.isPressed);
        }

        public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }

        public void OnScreenInteraction(InputValue value)
        {
            ScreenInteractionInput(value.isPressed);
        }

#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
            if (!Crouch)
                Jump = newJumpState;
            else return;
		}

		public void SprintInput(bool newSprintState)
		{
            if (!Crouch)
            {
                if (Sprint)
                    Sprint = !newSprintState;
                else if (!Sprint)
                    Sprint = newSprintState;
            }
            else return;
        }

        public void CrouchInput(bool newCrouchstate)
        {
            if (!Sprint)
            {
                if (Crouch)
                    Crouch = !newCrouchstate;
                else if (!Crouch)
                    Crouch = newCrouchstate;
            }
            else return;
        }

        public void AimInput(bool newAimState)
        {
            if (Aim)
                Aim = !newAimState;
            else if (!Aim)
                Aim = newAimState;
        }

        public void ShootInput(bool newShootState)
		{
            Shoot = newShootState;
        }

        public void ReloadInput(bool newReloadState)
        {
            Reload = newReloadState;
        }

        public void SetRifleInput(bool newRifleState)
        {
            SetRifle = newRifleState;
        }

        public void SetGunInput(bool newGunstate)
        {
            SetGun = newGunstate;
        }

        public void SetKnifeInput(bool newKnifestate)
        {
            SetKnife = newKnifestate;
        }

        public void InteractInput(bool newInteractState)
        {
            Interact = newInteractState;
        }

        public void ScreenInteractionInput(bool newScreenInteractionState)
        {
            ScreenInteraction = newScreenInteractionState;
        }

        private void OnApplicationFocus(bool hasFocus)
		{
            if (playerInputs.currentActionMap.name != "UI")
			    SetCursorState(cursorLocked);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

        public void ExitGame() => Application.Quit();
	}
	
}