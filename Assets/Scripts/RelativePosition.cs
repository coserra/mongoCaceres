using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativePosition : MonoBehaviour
{
    [SerializeField] private RectTransform relativeTo;
    [SerializeField] private Vector2 relativePosition;
    private RectTransform selfTransform;

    private void Start()
    {
        selfTransform = gameObject.GetComponent<RectTransform>();
    }

    public void UpdatePosition()
    {
        if (relativeTo.gameObject.activeSelf)
        {
            selfTransform.anchoredPosition = relativeTo.anchoredPosition + relativePosition;
        }
        else
        {
            selfTransform.anchoredPosition = relativeTo.anchoredPosition;
        }
    }
}
