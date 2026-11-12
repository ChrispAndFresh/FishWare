using UnityEngine;
using UnityEngine.InputSystem;

public class Touch_Minigame : Base_Minigame
{
    /*
     * Chris Pimentel
     * 11-11-26
     * Provides controls for all touched based minigames
     */

   public virtual void OnTouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            print("Screen Touched");
    }
}
