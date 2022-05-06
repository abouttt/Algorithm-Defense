using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    private class MouseData
    {
        public bool IsPressed { get; set; } = false;
        public bool IsSingleClick { get; set; } = false;
        public float PressedTime { get; set; } = 0.0f;
        public float SingleClickTime { get; set; } = 0.0f;
    }

    public Action KeyAction { get; set; } = null;
    public Action<Define.MouseEvent> LMouseAction { get; set; } = null;
    public Action<Define.MouseEvent> RMouseAction { get; set; } = null;

    private MouseData _LMouseData = new MouseData();
    private MouseData _RMouseData = new MouseData();

    private const float NEXT_CLICK_TIME_LIMIT = 0.2f;

    public void OnUpdate()
    {
        keyActionUpdate();

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        mouseActionUpdate(Input.GetMouseButton(0), _LMouseData, LMouseAction);
        mouseActionUpdate(Input.GetMouseButton(1), _RMouseData, RMouseAction);
    }

    public void Clear()
    {
        KeyAction = null;
        LMouseAction = null;
        RMouseAction = null;
    }

    public void ClearKeyAction() => KeyAction = null;
    public void ClearMouseLAction() => LMouseAction = null;
    public void ClearMouseRAction() => RMouseAction = null;

    private void keyActionUpdate()
    {
        if (Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();
        }
    }

    private void mouseActionUpdate(bool isGetMouseButton, MouseData mouseData, Action<Define.MouseEvent> mouseAction)
    {
        if (mouseAction != null)
        {
            if (isGetMouseButton)
            {
                if (!mouseData.IsPressed)
                {
                    mouseAction.Invoke(Define.MouseEvent.PointerDown);
                    mouseData.PressedTime = Time.time;
                }

                mouseAction.Invoke(Define.MouseEvent.Press);
                mouseData.IsPressed = true;
            }
            else
            {
                if (mouseData.IsPressed)
                {
                    if (mouseData.IsSingleClick)
                    {
                        mouseAction.Invoke(Define.MouseEvent.DoubleClick);
                    }

                    if (Time.time < mouseData.PressedTime + NEXT_CLICK_TIME_LIMIT)
                    {
                        mouseAction.Invoke(Define.MouseEvent.SingleClick);

                        if (!mouseData.IsSingleClick)
                        {
                            mouseData.SingleClickTime = Time.time;
                            mouseData.IsSingleClick = true;
                        }
                    }

                    mouseAction.Invoke(Define.MouseEvent.PointerUp);
                }

                if (Time.time > mouseData.SingleClickTime + NEXT_CLICK_TIME_LIMIT)
                {
                    mouseData.IsSingleClick = false;
                }

                mouseData.IsPressed = false;
            }
        }
    }
}