using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : FixedJoystick
{
    private enum FUNCTIONALITY_JOYSTICK { MOVE, ATTACK }
    [Header("MyAttributes")]
    [SerializeField] private FUNCTIONALITY_JOYSTICK function = FUNCTIONALITY_JOYSTICK.MOVE;
    private Vector3 mousePosition = Vector3.zero;
    private Vector2 initPos = Vector2.zero;
    private bool dragged = false;
#if !UNITY_EDITOR
    private int numTouch = 0;
#endif

    private void Awake()
    {
        initPos = background.anchoredPosition;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        dragged = false;
#if UNITY_EDITOR
        mousePosition = Input.mousePosition;
#else
        numTouch = Input.touchCount;
        mousePosition = Input.GetTouch(numTouch - 1).position;
#endif
        background.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        background.position = new Vector3(mousePosition.x - (Horizontal * 150), mousePosition.y - (Vertical * 150), transform.position.z);
        joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        //base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        background.anchoredPosition = initPos;
        joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);

        if (function == FUNCTIONALITY_JOYSTICK.ATTACK && !dragged)
        {
            GameManager.Instance.LevelController.player.Shoot(true);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        dragged = true;
        base.OnDrag(eventData);
        if (inputVector.magnitude >= 0.9f)
        {
#if UNITY_EDITOR
            mousePosition = Input.mousePosition;
#else
            mousePosition = Input.GetTouch(numTouch - 1).position;
#endif
            background.position = new Vector3(mousePosition.x - (Horizontal * 150), mousePosition.y - (Vertical * 150), transform.position.z);
            joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        }
    }
}
