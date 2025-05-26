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

        private void OnAbility4(InputAction.CallbackContext context)
        {
            Debug.Log("Ability 4 Action");
        }

        private void OnAbility1(InputAction.CallbackContext context)
        {
            Debug.Log("Ability 1 Action");
        }

        private void OnSecondaryAction(InputAction.CallbackContext context)
        {
            Debug.Log("Secondary action");
        }

        private void OnMainAction(InputAction.CallbackContext context)
        {
            Debug.Log("Main Action");
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Debug.Log("Tryna Move");
        }
    }
}
