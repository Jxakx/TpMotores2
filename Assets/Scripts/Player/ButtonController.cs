using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : Controller
{
    Vector3 dir;
    [SerializeField] EventTrigger upButton;
    [SerializeField] EventTrigger rightButton;
    [SerializeField] EventTrigger leftButton;

    private void Start()
    {
        /*upButton.onClick.AddListener(new UnityEngine.Events.UnityAction(PressUpButton));
        rightButton.onClick.AddListener(new UnityEngine.Events.UnityAction(PressRightButton));
        leftButton.onClick.AddListener(new UnityEngine.Events.UnityAction(PressLeftButton));*/
    }
    public override Vector3 GetMoveDir()
    {
        return dir;
    }

    public void PressUpButton()
    {
        dir = Vector3.up;
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
    }
}
