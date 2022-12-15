using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler , IBeginDragHandler, IEndDragHandler
{
    private RectTransform draggingObject;
    private Vector3 globalMousePosition;
    private Vector3 initialPosition;
    private Vector3 initialGlobalPosition;
    bool isHidden;

    [HideInInspector]
    public int cardIndex;

    [HideInInspector]
    public Attack attack;
    
    [SerializeField]
    Image image;

    [SerializeField]
    public TMP_Text cardActionPoint, cardDescription;

    private void Awake()
    {
        draggingObject = transform as RectTransform;
        initialPosition = transform.localPosition;
        initialGlobalPosition = transform.position;
        isHidden = false;
        ChangeAppearance(true);
    }

    public void ChangeAppearance(bool isHidden = true)
    {
        if (isHidden == this.isHidden) return;
        this.isHidden = isHidden;
        gameObject.SetActive(!isHidden);
    }

    public void SetNewAttack(Attack attack)
    {
        this.attack = attack;
        image.sprite = attack.cardSprite;
        cardActionPoint.text = attack.actionCost.ToString();
        cardDescription.text = attack.attackDescription.ToString();
        ChangeAppearance(false);
        transform.localPosition = initialPosition;
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
        Debug.Log("Salue");
        transform.DOScale(Vector3.one * 1.5f, 1).SetEase(Ease.OutQuint);
        transform.DOMoveY(initialGlobalPosition.y + 50, 1);
        transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, 1).SetEase(Ease.OutQuint);
        transform.DOMoveY(initialGlobalPosition.y, 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RectTransform playerRect = FightSystem.instance.player.transform as RectTransform;
        if (transform.localPosition.y >= playerRect.localPosition.y - (playerRect.rect.height / 2))
        {
            if (FightSystem.instance.PlayACard(attack, cardIndex))
            {
                ChangeAppearance(true);
                return;
            }
        }
        transform.localPosition = initialPosition;
    }
}