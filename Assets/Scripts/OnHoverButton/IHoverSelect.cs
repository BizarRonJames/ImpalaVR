using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IHoverSelect : IEventSystemHandler
{
    void OnPointerEnter(PointerEventData eventData);

    void OnPointerExit(PointerEventData eventData);
}
