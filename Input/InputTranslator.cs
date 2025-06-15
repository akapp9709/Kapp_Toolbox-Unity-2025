using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Toolbox.Input
{
    public class InputTranslator
    {
        public static InputTranslator Instance;

        public InputTranslator()
        {
            MainInputActions _input = new MainInputActions();

            _input.Main.Enable();

            _input.Main.Move.performed += OnMove;
            _input.Main.Move.canceled += OnMove;

            _input.Main.Action1.started += OnMainAction;
            _input.Main.Action1.canceled += OnMainAction;

            _input.Main.Action2.started += OnSecondaryAction;
            _input.Main.Action2.canceled += OnSecondaryAction;

            _input.Main.AbilitySlot1.started += OnAbility1;
            _input.Main.AbilitySlot1.canceled += OnAbility1;

            _input.Main.AbilitySlot2.started += OnAbility2;
            _input.Main.AbilitySlot2.canceled += OnAbility2;

            _input.Main.AbilitySlot3.started += OnAbility3;
            _input.Main.AbilitySlot3.canceled += OnAbility3;

            _input.Main.AbilitySlot4.started += OnAbility4;
            _input.Main.AbilitySlot4.canceled += OnAbility4;

            _input.Main.SpaceBar.started += OnSpace;

            _input.Main.Sprint.started += OnSprint;
            _input.Main.Sprint.canceled += OnSprint;

            _input.Main.Aim.performed += OnAim;
        }

        public Vector2 AimDirection { get; private set; }
        private void OnAim(InputAction.CallbackContext context)
        {
            AimDirection = context.ReadValue<Vector2>();
        }

        public Action OnSprintedStarted, OnSprintCanceled;
        private void OnSprint(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnSprintedStarted?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    OnSprintCanceled?.Invoke();
                    break;
            }
        }

        public Action OnSpaceStarted, OnSpaceCanceled;
        private void OnSpace(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnSpaceStarted?.Invoke();
                    break;
            }
        }

        public Action OnAbility2Started, OnAbility2Canceled;
        private void OnAbility2(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnAbility2Started?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    OnAbility2Canceled?.Invoke();
                    break;
            }
        }

        public Action OnAbility3Started, OnAbility3Canceled;
        private void OnAbility3(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnAbility3Started?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    OnAbility3Canceled?.Invoke();
                    break;
            }
        }

        public Action OnAbility4Started, OnAbility4Canceled;
        private void OnAbility4(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnAbility4Started?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    OnAbility4Canceled?.Invoke();
                    break;
            }
        }

        public Action OnAbility1Started, OnAbility1Canceled;
        private void OnAbility1(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnAbility1Started?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    OnAbility1Canceled?.Invoke();
                    break;
            }
        }

        public Action OnSecondaryActionStarted, OnSecondaryActionPerformed, OnSecondaryActionCanceled;
        private void OnSecondaryAction(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnSecondaryActionStarted?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    OnSecondaryActionCanceled?.Invoke();
                    break;
                case InputActionPhase.Performed:
                    OnSecondaryActionPerformed?.Invoke();
                    break;
            }
        }

        public Action OnMainActionStarted, OnMainActionPerformed, OnMainActionCanceled;
        private void OnMainAction(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnMainActionStarted?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    OnMainActionCanceled?.Invoke();
                    break;
                case InputActionPhase.Performed:
                    OnMainActionPerformed?.Invoke();
                    break;
            }
        }

        public Action OnMoveStart, OnMovePerformed, OnMoveCanceled, OnMoveMaster;
        public Vector2 MoveDirection;
        private void OnMove(InputAction.CallbackContext context)
        {
            MoveDirection = context.ReadValue<Vector2>();
        }
    }
}
