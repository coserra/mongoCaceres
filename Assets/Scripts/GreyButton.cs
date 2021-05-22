using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreyButton : MonoBehaviour
{
    private bool activo;
    public Image sprite;

    private void Start()
    {
        activo = false;
        sprite = GetComponent<Image>();
        sprite.color = new Color32(140, 146, 140, 255);
    }
    public void ChangeColor()
    {
        activo = !activo;
        if (!activo)
        {
            sprite.color=new Color32(140,146,140,255);
        }
        else
        {
            sprite.color = new Color32(255, 255, 255,255);
        }
    }
}
