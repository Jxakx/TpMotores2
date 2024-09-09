using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : Controller
{
    Vector3 dir;
    [SerializeField] EventTrigger jumpButton;
    [SerializeField] EventTrigger rightButton;
    [SerializeField] EventTrigger leftButton;
    private bool jump = false;

    public override Vector3 GetMoveDir()
    {
        return dir;
    }

    public override bool IsJumping()
    {
        return jump;
    }
    public void PressJumpButton()
    {
        jump = true;
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
    }
}
