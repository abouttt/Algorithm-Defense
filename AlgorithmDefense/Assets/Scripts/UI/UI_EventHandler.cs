using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler
{
    public Action<PointerEventData> OnClickHandler { get; set; }

    public void OnPointerClick(PointerEventData eventData)
       => OnClickHandler?.Invoke(eventData);
}
