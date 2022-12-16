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
    private Vector3 globalMousePosition;
    private Vector3 initialGlobalPosition;
    bool isFaceDown;
    bool hasBeenPlayed;

    [HideInInspector]
    public int cardIndex;

    [HideInInspector]
    public Attack attack;
    
    [SerializeField]
    private Image image;

    public TMP_Text cardActionPoint, cardDescription;

    [SerializeField]
    private Sprite cardBack;

    private Tween scaleTween, moveTween, rotateTween, mouseMoveTween;

    private void Awake()
    {
        draggingObject = transform as RectTransform;
        initialGlobalPosition = transform.position;
        isFaceDown = true;
        image.sprite = cardBack;
        cardActionPoint.text = "";
        cardDescription.text = "";
        ChangeSide(false);
        DOTween.SetTweensCapacity(500,500);
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
        hasBeenPlayed = isFaceDown;
        image.sprite = isFaceDown ? cardBack : attack.cardSprite;
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(!isFaceDown);
        }
        rotateTween = transform.DORotate(Vector3.zero, .5f);
    }

    public void SetNewAttack(Attack attack)
    {
        this.attack = attack;
        cardActionPoint.text = attack.actionCost.ToString();
        cardDescription.text = attack.attackDescription.ToString();
        ChangeSide(false);
        transform.position = initialGlobalPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (hasBeenPlayed) return;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingObject, eventData.position, eventData.pressEventCamera, out globalMousePosition))
        {
            mouseMoveTween = draggingObject.DOMove(globalMousePosition, .5f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hasBeenPlayed) return;
        scaleTween = transform.DOScale(Vector3.one * 1.5f, 1).SetEase(Ease.OutQuint);
        moveTween = transform.DOMove(initialGlobalPosition + new Vector3(0, 50, 0), 1);
        transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hasBeenPlayed) return;
        scaleTween = transform.DOScale(Vector3.one, 1).SetEase(Ease.OutQuint);
        moveTween = transform.DOMove(initialGlobalPosition, 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (hasBeenPlayed) return;
        moveTween.Kill();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.position.y >= FightSystem.instance.uiManager.minimuHeight.position.y)
        {
            if (FightSystem.instance.PlayACard(attack, cardIndex))
            {
                return;
            }
        }
        transform.DOMove(initialGlobalPosition, 1f).SetEase(Ease.OutQuint);
    }
}