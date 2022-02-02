using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraAlpha : MonoBehaviour
{
    Image AlphaImage;
    bool AlphaBool = false;
    float speed = 2f;

    void Start()
    {
        AlphaImage = GetComponent<Image>();
    }

    void Update()
    {
        if (AlphaBool && AlphaImage.color.a < 0.95f)
        {
            AlphaImage.color = new Color(AlphaImage.color.r, AlphaImage.color.g,
                AlphaImage.color.b, AlphaImage.color.a + speed * Time.deltaTime);
        } else if (!AlphaBool && AlphaImage.color.a > 0.05f)
            {
                AlphaImage.color = new Color(AlphaImage.color.r, AlphaImage.color.g,
                AlphaImage.color.b, AlphaImage.color.a - speed * Time.deltaTime);
            }

        if (AlphaImage.color.a >= (0.95f) || AlphaImage.color.a <= (0.05f)) gameObject.SetActive(false);
    
    }

    public void Alpha()
    {
        AlphaBool = !AlphaBool;
    }
}
