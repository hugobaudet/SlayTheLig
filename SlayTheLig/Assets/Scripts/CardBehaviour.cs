using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBehaviour : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler , IBeginDragHandler, IEndDragHandler
{
    private const float HEIGHT_CARD_PLAYED = 100f;

    private RectTransform draggingObject;
    private Vector3 globalMousePosition;
    private Vector3 initialPosition;
    bool isHidden;

    private Attack attack;


    private void Awake()
    {
        draggingObject = transform as RectTransform;
        initialPosition = transform.position;
        isHidden = false;
        ChangeAppearance(true);
    }

    public void ChangeAppearance(bool isHidden = true)
    {
        if (isHidden == this.isHidden) return;
        this.isHidden = isHidden;
        transform.position = initialPosition + (Vector3.down * 200 * (isHidden ? 1 : 0));
    }

    public void SetNewAttack(Attack attack)
    {
        this.attack = attack;
        ChangeAppearance(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingObject, eventData.position, eventData.pressEventCamera, out globalMousePosition))
        {
            draggingObject.position = globalMousePosition;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (globalMousePosition.y >= HEIGHT_CARD_PLAYED)
        {
            if (FightSystem.instance.PlayACard(attack))
            {
                ChangeAppearance(true);
                return;
            }
        }
        transform.position = initialPosition;
    }
}