using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHovered : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image background;
    [SerializeField] private Sprite image;
    [SerializeField] private Sprite defaultImage;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        background.sprite = image;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.sprite = defaultImage;
    }
}
