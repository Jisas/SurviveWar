using UnityEngine.InputSystem;
using SurviveWar;

public static class InputsManager
{
    private static readonly StarterAssets input;

    static InputsManager()
    {
        input = new StarterAssets();
        input.Player.Enable();
    }

    public static StarterAssets.PlayerActions Player
    {
        get { return input.Player; }
    }

    public static StarterAssets.UIActions UI
    {
        get { return input.UI; }
    }

    public static void EnablePlayerMap(bool enable)
    {
        if (enable) input.Player.Enable();
        else input.Player.Disable();
    }

    public static void EnableUIMap(bool enable)
    {
        if (enable) input.UI.Enable();
        else input.UI.Disable();
    }

    public static bool GetIsCurrentDiviceMouse(PlayerInput playerInput)
    {
        return playerInput.currentControlScheme == "Keyboard&Mouse";
    }

    public static void SwitchToUI(EntityInputs entityInputs, bool isCurrentDiviceMouse)
    {
        if (isCurrentDiviceMouse)
        {
            entityInputs.cursorLocked = false;
            entityInputs.cursorInputForLook = false;
        }

        EnablePlayerMap(false);
        EnableUIMap(true);
    }

    public static void SwitchToPlayer(EntityInputs entityInputs, bool isCurrentDiviceMouse)
    {
        if (isCurrentDiviceMouse)
        {
            entityInputs.cursorLocked = true;
            entityInputs.cursorInputForLook = true;
        }

        EnableUIMap(false);
        EnablePlayerMap(true);
    }
}
