using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler , IBeginDragHandler, IEndDragHandler
{
    private RectTransform draggingObject;
    private Vector3 globalMousePosition, initialGlobalPosition;
    private bool isFaceDown, isBeingDrag;

    [SerializeField] private bool updateCardName;

    [HideInInspector]
    public int cardIndex;

    [HideInInspector]
    public Attack attack;
    
    [SerializeField]
    private Image image;

    [SerializeField]
    private TMP_Text cardActionPoint, cardDescription, cardName;

    [SerializeField]
    private Sprite cardBack;

    private Tween scaleTween, moveTween, rotateTween, mouseMoveTween;

    //IT WORKS
    public void Initialize()
    {
        draggingObject = transform as RectTransform;
        isFaceDown = true;
        isFaceDown = true;
        image.sprite = cardBack;
        initialGlobalPosition = transform.position;
        cardName.gameObject.SetActive(updateCardName);
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void SetInitialPosition(float positionX)
    {
        initialGlobalPosition.x = positionX;
        moveTween = transform.DOMove(initialGlobalPosition, 1f);
    }

    public void ChangeSide(bool flipItDown)
    {
        if (flipItDown == isFaceDown) return;
        isFaceDown = flipItDown;
        StartCoroutine(ChangeSprite());
    }

    IEnumerator ChangeSprite()
    {
        rotateTween = transform.DORotate(Vector3.up * 90, .5f);
        yield return rotateTween.WaitForCompletion();
        image.sprite = isFaceDown ? cardBack : attack.cardSprite;
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(!isFaceDown);
        }
        rotateTween = transform.DORotate(Vector3.zero, .5f);
        yield return rotateTween.WaitForCompletion();
        if (!isFaceDown)
        {
            FightSystem.instance.StartPlayerChoice();
        }
    }
            
    public void SetNewAttack(Attack attack)
    {
        if (attack == null) return;
        this.attack = attack;
        if (updateCardName)
        {
            cardName.text = attack.cardName;
        }
        cardActionPoint.text = attack.actionCost.ToString();
        cardDescription.text = attack.attackDescription.ToString();
        ChangeSide(false);
        transform.position = initialGlobalPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isFaceDown)
        {
            OnPointerExit(eventData);
            return;
        }
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingObject, eventData.position, eventData.pressEventCamera, out globalMousePosition))
        {
            mouseMoveTween = draggingObject.DOMove(globalMousePosition, .5f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isFaceDown)
        {
            OnPointerExit(eventData);
            return;
        }
        if (isBeingDrag) return;
        scaleTween = transform.DOScale(Vector3.one * 1.5f, 1).SetEase(Ease.OutQuint);
        moveTween = transform.DOMove(initialGlobalPosition + new Vector3(0, 50, 0), 1);
        transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isBeingDrag) return;
        scaleTween = transform.DOScale(Vector3.one, 1).SetEase(Ease.OutQuint);
        moveTween = transform.DOMove(initialGlobalPosition, 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isFaceDown) return;
        isBeingDrag = true;
        moveTween.Kill();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isFaceDown) return;
        isBeingDrag = false;
        if (globalMousePosition.y >= FightSystem.instance.uiManager.minimuHeight.position.y)
        {
            FightSystem.instance.PlayACard(attack, cardIndex);
        }
        moveTween = transform.DOMove(initialGlobalPosition, 1f).SetEase(Ease.OutQuint);
        scaleTween = transform.DOScale(Vector3.one, 1).SetEase(Ease.OutQuint);
    }
}