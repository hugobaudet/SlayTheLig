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

    [HideInInspector]
    public Attack attack;
    
    [SerializeField]
    Image image;

    [SerializeField]
    public TMP_Text cardActionPoint, cardDescription;

    private void Awake()
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

    public void ChangeAppearance(bool isHidden = true)
    {
        if (isHidden == this.isHidden) return;
        this.isHidden = isHidden;
        gameObject.SetActive(!isHidden);
        
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
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RectTransform playerRect = FightSystem.instance.player.transform as RectTransform;
        if (transform.localPosition.y >= playerRect.localPosition.y - (playerRect.rect.height / 2))
        {
            if (FightSystem.instance.PlayACard(attack, transform.GetSiblingIndex()))
            {
                ChangeAppearance(true);
                return;
            }
        }
        transform.localPosition = initialPosition;
    }
}