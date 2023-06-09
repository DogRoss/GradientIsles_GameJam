using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    Image musicIcon;

    Color color = new Color(255, 255, 255, 255);

    private void Start()
    {
        musicIcon = GetComponent<Image>();
    }

    public void ToggleSprite()
    {
        color.a = color.a == 0 ? 255 : 0;
        musicIcon.color = color;
    }
}
