using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColors : MonoBehaviour
{
    public new Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        RandomColor();
    }

    void RandomColor()
    {
        Color randomColor1 = Saturate(Random.ColorHSV());
        Color randomColor2 = Saturate(Random.ColorHSV());

        try
        {
            renderer.materials[1].color = randomColor1; // 1 == pants
            renderer.materials[4].color = randomColor2; // 4 == shirt
        }
        catch
        {
            // It throws an IndexOutOfRangeException early when starting the scene, because this also applies to the instanceable.
        }
    }

    Color Saturate(Color unsaturatedColor)
    {
        // Note: range of RGB colors are between 0 and 1.
        Color saturatedColor = new Color();

        switch (LeastSaturatedColor(unsaturatedColor.r, unsaturatedColor.g, unsaturatedColor.b))
        {
            case 1:
                saturatedColor.r = 0;
                if (LeastSaturatedColor(unsaturatedColor.g, unsaturatedColor.b) == 1)
                {
                    saturatedColor.g = unsaturatedColor.g;
                    saturatedColor.b = 1;
                }
                else
                {
                    saturatedColor.g = 1; 
                    saturatedColor.b = unsaturatedColor.b;
                }
                break;
            case 2:
                saturatedColor.g = 0;
                if (LeastSaturatedColor(unsaturatedColor.r, unsaturatedColor.b) == 1)
                {
                    saturatedColor.r = unsaturatedColor.r;
                    saturatedColor.b = 1;
                }
                else
                {
                    saturatedColor.r = 1;
                    saturatedColor.b = unsaturatedColor.b;
                }
                break;
            case 3:
                saturatedColor.b = 0;
                if (LeastSaturatedColor(unsaturatedColor.r, unsaturatedColor.g) == 1)
                {
                    saturatedColor.r = unsaturatedColor.r;
                    saturatedColor.g = 1;
                }
                else
                {
                    saturatedColor.r = 1;
                    saturatedColor.g = unsaturatedColor.g;
                }
                break;
        }

        return saturatedColor;
    }

    int LeastSaturatedColor(float color1 = 1, float color2 = 1, float color3 = 1)
    {
        float leastSaturated = Mathf.Min(color1, color2, color3);

        if (leastSaturated == color1)
        {
            return 1;
        }
        else if (leastSaturated == color2)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }
}
