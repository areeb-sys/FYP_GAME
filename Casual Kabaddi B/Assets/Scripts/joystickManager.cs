using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class joystickManager : MonoBehaviour, IDragHandler, IPointerDownHandler,IPointerUpHandler
{
    private Image imgJoystickbg;
    private Image imgJoystick;
    private Vector2 PosInp;
    // Start is called before the first frame update
    void Start()
    {
        imgJoystickbg = GetComponent<Image>();
        imgJoystick = transform.GetChild(0).GetComponent<Image>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgJoystickbg.rectTransform, eventData.position,
            eventData.pressEventCamera,
            out PosInp))
        {
            PosInp.x = PosInp.x / (imgJoystickbg.rectTransform.sizeDelta.x);
            PosInp.y = PosInp.y / (imgJoystickbg.rectTransform.sizeDelta.y);
           // Debug.Log(PosInp.x.ToString() + "/" + PosInp.y.ToString());
        }

        if(PosInp.magnitude > 1.0f)
        {
            PosInp = PosInp.normalized;
        }    
        //joystick Movement
        imgJoystick.rectTransform.anchoredPosition = new Vector2(
            PosInp.x * (imgJoystickbg.rectTransform.sizeDelta.x / 2),
            PosInp.y * (imgJoystickbg.rectTransform.sizeDelta.y / 2));
    } 

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PosInp = Vector2.zero;
        imgJoystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float inputHorizontal()
    {
        if (PosInp.x != 0)
            return PosInp.x;
        else
            return Input.GetAxis("Horizontal"); 
    }

    public float inputVertical()
    {
        if (PosInp.y != 0)
            return PosInp.y;
        else
            return Input.GetAxis("Vertical");
    }

}
