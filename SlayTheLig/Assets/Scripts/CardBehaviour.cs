using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBehaviour : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler , IBeginDragHandler, IEndDragHandler
{
    private RectTransform draggingObject;
    private Vector3 globalMousePosition;
    private Vector3 initialPosition;
    [SerializeField]
    private Attack cardAttack;
    [SerializeField]
    private LayerMask enemyLayer;

    private void Awake()
    {
        draggingObject = transform as RectTransform;
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
        initialPosition = transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Collider2D target = Physics2D.OverlapCircle(globalMousePosition, .01f, enemyLayer);
        if (target != null ? PlayerBehaviour.instance.CheckPossibilityAttack(target.GetComponent<CharacterBehaviour>(), cardAttack) : false)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = initialPosition;
        }
    }
}