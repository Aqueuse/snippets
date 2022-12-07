using System;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem.Controls;

public class KeymapRebinding : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI actionNameText;  // action name text
    [SerializeField] private TextMeshProUGUI buttonBindingText; // button binding text
    [SerializeField] private string actionName; // Rebind Settings

    [SerializeField] private GameObject rebindButton;
    [SerializeField] private GameObject listeningForInputButton;

    private PlayerInput playerInput;
    private InputAction focusedInputAction;
    private InputActionAsset focusedInputActionAsset;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    
    private void Start() {
        playerInput = BananaMan.Instance.GetComponent<PlayerInput>();
        focusedInputAction = playerInput.actions.FindAction(actionName);
    }
    
    public void ButtonPressedStartRebind() {
        StartRebindProcess();
    }

    void StartRebindProcess() {
        focusedInputAction.Disable();
        
        ToggleGameObjectState(rebindButton, false);
        ToggleGameObjectState(listeningForInputButton, true);
        
        rebindOperation = focusedInputAction.PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindCompleted(actionName));

        rebindOperation.Start();
    }
    
    void RebindCompleted(string actionNameUI) {
        rebindOperation.Dispose();
        rebindOperation = null;

        ToggleGameObjectState(rebindButton, true);
        ToggleGameObjectState(listeningForInputButton, false);

        focusedInputAction.Enable();

        UpdateBindingDisplayUI();
        UpdateActionDisplayUI(actionNameUI);
    }


    public void ButtonPressedWasdStartRebind(string actionNameComposite) {
        StartRebindWasdProcess(actionNameComposite);
    }

    void StartRebindWasdProcess(string actionNameComposite) {
        focusedInputAction.Disable();

        ToggleGameObjectState(rebindButton, false);
        ToggleGameObjectState(listeningForInputButton, true);

        var wasd = focusedInputAction.ChangeBinding("WASD");
        var part = wasd.NextPartBinding(actionNameComposite);

        rebindOperation = focusedInputAction.PerformInteractiveRebinding()
            .WithTargetBinding(part.bindingIndex)
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindCompleted(actionNameComposite));

        rebindOperation.Start();
    }
    
    void UpdateActionDisplayUI(string actionNameUI) {
        actionNameText.SetText(actionNameUI);
    }

    void UpdateBindingDisplayUI() {
        string displayName = focusedInputAction.controls[0].displayName;

        try {
            string localizedDisplayName = ((KeyControl)Keyboard.current[displayName]).keyCode.ToString();
            buttonBindingText.SetText(localizedDisplayName);
        }
        catch (Exception) { // fallback if there is no matching in the actual keyboard
            buttonBindingText.SetText(displayName);
        }
    }

    void ToggleGameObjectState(GameObject targetGameObject, bool newState) {
        targetGameObject.SetActive(newState);
    }

    public void ButtonPressedResetBinding() {
        ResetBinding();
    }

    public void ResetBinding() {
        // reset all bindings
        InputActionRebindingExtensions.RemoveAllBindingOverrides(focusedInputAction);
        UpdateBindingDisplayUI();
    }
}