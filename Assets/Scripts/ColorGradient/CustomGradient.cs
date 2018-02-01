using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class CustomGradient
{
    public enum BlendMode
    {
        Linear,
        Discrete,
        Bleed
    };

    public BlendMode blendMode;

    public bool randomizeColor;
    public float bleedAmount;

    [SerializeField]
    private List<ColorKey> keys = new List<ColorKey>();

    public CustomGradient()
    {
        AddKey(Color.white, 0);
        AddKey(Color.black, 1);
    }

    [System.Serializable]
    public struct ColorKey
    {
        [SerializeField]
        private Color color;
        [SerializeField]
        private float time;

        //Constructor
        public ColorKey(Color color, float time)
        {
            this.color = color;
            this.time = time;
        }

        public Color Color
        {
            get { return color; }
        }

        public float Time
        {
            get { return time; }
        }
    }

    public int NumKeys
    {
        get { return keys.Count; }
    }

	public Color Evaluate(float time)
    {        
        ColorKey keyLeft = keys[0];
        ColorKey keyRight = keys[keys.Count - 1];

        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Time < time )
            {
                keyLeft = keys[i]; 
            }

            if (keys[i].Time > time)
            {
                keyRight = keys[i];
                break;
            }
        }

        if (blendMode == BlendMode.Linear)
        {
            float blendTime = Mathf.InverseLerp(keyLeft.Time, keyRight.Time, time);

            return Color.Lerp(keyLeft.Color, keyRight.Color, blendTime);
        }

        if(blendMode == BlendMode.Bleed)
        {
            float blendTime = Mathf.InverseLerp(keyLeft.Time, keyRight.Time, time) /bleedAmount;
            return Color.Lerp(keyLeft.Color, keyRight.Color, blendTime);
        }

        return keyRight.Color;

    }

    public ColorKey GetKey(int i)
    {
        return keys[i];
    }

    public int AddKey(Color color, float time)
    {
        ColorKey newKey = new ColorKey(color, time);

        for(int i = 0; i < keys.Count; i++)
        {
            if(newKey.Time < keys[i].Time)
            {
                keys.Insert(i, newKey);
                return i;
            }
        }

        keys.Add(newKey);
        return keys.Count - 1;
    }

    public void RemoveKey(int index)
    {
        if (keys.Count >= 2)
        {
            keys.RemoveAt(index);
        }
    }

    public int UpdateKeyTime(int index, float time)
    {
        Color col = keys[index].Color;
        RemoveKey(index);
        return AddKey(col, time);
    }

    public void UpdateKeyColor(int index, Color color)
    {
        keys[index] = new ColorKey(color,keys[index].Time);
    }

    public Texture2D GetTexture(int width)
    {
        Texture2D texture = new Texture2D(width, 1);
        Color[] colors = new Color[width];

        for (int i = 0; i < width; i++)
        {
            colors[i] = Evaluate((float)i / (width - 1));
        }

        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }
}