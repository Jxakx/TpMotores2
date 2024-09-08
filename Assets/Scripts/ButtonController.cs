using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : Controller
{
    Vector3 dir;
    [SerializeField] Button upButton;
    [SerializeField] Button rightButton;
    [SerializeField] Button leftButton;

    private void Start()
    {
        upButton.onClick.AddListener(new UnityEngine.Events.UnityAction(PressUpButton));
        rightButton.onClick.AddListener(new UnityEngine.Events.UnityAction(PressRightButton));
        leftButton.onClick.AddListener(new UnityEngine.Events.UnityAction(PressLeftButton));
    }
    public override Vector3 GetMoveDir()
    {
        return dir;
    }

    void PressUpButton()
    {
        dir = Vector3.forward;
    }

    void PressRightButton()
    {
        dir = Vector3.right;
    }

    void PressLeftButton()
    {
        dir = -Vector3.right;
    }
}
