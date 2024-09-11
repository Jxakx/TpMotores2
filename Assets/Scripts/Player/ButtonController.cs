using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : Controller
{
    Vector3 dir;
    [SerializeField] EventTrigger jumpButton;
    [SerializeField] EventTrigger dashButton;
    [SerializeField] EventTrigger rightButton;
    [SerializeField] EventTrigger leftButton;
    private bool jump = false;
    private bool dash = false;

    public override Vector3 GetMoveDir()
    {
        return dir;
    }

    public override bool IsJumping()
    {
        bool wasJumping = jump;
        jump = false;  // Restablecemos el estado de salto después de evaluarlo
        return wasJumping;
    }

    public override bool IsDashing()
    {
        bool wasDashing = dash;
        dash = false;  // Restablecemos el estado de salto después de evaluarlo
        return wasDashing;
    }
    public void PressJumpButton()
    {
        jump = true;
    }

    public void PressDashButton()
    {
        dash = true;
    }
    public void PressRightButton()
    {
        dir = Vector3.right;
    }

    public void PressLeftButton()
    {
        dir = -Vector3.right;
    }

    public void stopMovement()
    {
        dir = Vector3.zero;
        jump = false;
        dash = false;
    }
}
