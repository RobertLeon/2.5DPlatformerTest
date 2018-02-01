using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GradientEditor : EditorWindow
{
    private CustomGradient gradient;
    private const int borderSize = 10;
    private const float keyWidth = 10;
    private const float keyHeight = 20;
    private Rect[] keyRects;
    private Rect gradientPreviewRect;
    private bool mouseIsDownOverKey;
    private int keyIndex;
    private bool needsRepaint;


    private void OnGUI()
    {
        Draw();
        HandleInput();


        if (needsRepaint)
        {
            Repaint();
            needsRepaint = false;
        }
    }

    private void Draw()
    {
        gradientPreviewRect = new Rect(borderSize, borderSize,
            position.width - borderSize * 2, 25);

        GUI.DrawTexture(gradientPreviewRect,
            gradient.GetTexture((int)gradientPreviewRect.width));

        keyRects = new Rect[gradient.NumKeys];
        for (int i = 0; i < gradient.NumKeys; i++)
        {
            CustomGradient.ColorKey key = gradient.GetKey(i);
            Rect keyRect = new Rect(gradientPreviewRect.x +
                gradientPreviewRect.width * key.Time - keyWidth / 2f,
                gradientPreviewRect.yMax + borderSize, keyWidth, keyHeight);

            if (i == keyIndex)
            {
                EditorGUI.DrawRect(new Rect(keyRect.x - 2, keyRect.y - 2,
                    keyRect.width + 4, keyRect.height + 4), Color.black);
            }

            EditorGUI.DrawRect(keyRect, key.Color);
            keyRects[i] = keyRect;
        }

        Rect settingsRect = new Rect(borderSize, keyRects[0].yMax + borderSize,
            position.width - borderSize * 2, position.height);

        GUILayout.BeginArea(settingsRect);
        EditorGUI.BeginChangeCheck();

        Color newColor = EditorGUILayout.ColorField(gradient.GetKey(keyIndex).Color);

        if(EditorGUI.EndChangeCheck())
        {
            gradient.UpdateKeyColor(keyIndex, newColor);
        }

        gradient.blendMode = (CustomGradient.BlendMode)EditorGUILayout.EnumPopup(
            "Blend mode", gradient.blendMode);

        gradient.randomizeColor = EditorGUILayout.Toggle("Randomize color",
            gradient.randomizeColor);

        if (gradient.blendMode == CustomGradient.BlendMode.Bleed)
        {
            gradient.bleedAmount = EditorGUILayout.Slider("Bleed", gradient.bleedAmount, 0f, 1f);
        }

        GUILayout.EndArea();
    }

    private void HandleInput()
    {
        Event guiEvent = Event.current;

        if (guiEvent.type == EventType.mouseDown && guiEvent.button == 0)
        {

            for (int i = 0; i < keyRects.Length; i++)
            {
                if (keyRects[i].Contains(guiEvent.mousePosition))
                {
                    mouseIsDownOverKey = true;
                    keyIndex = i;
                    needsRepaint = true;
                    break;
                }
            }

            if (!mouseIsDownOverKey)
            {
                float keyTime = Mathf.InverseLerp(gradientPreviewRect.x, gradientPreviewRect.xMax, guiEvent.mousePosition.x);
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                Color interpolatedColor = gradient.Evaluate(keyTime);

                keyIndex = gradient.AddKey((gradient.randomizeColor)?randomColor:interpolatedColor, keyTime);
                mouseIsDownOverKey = true;
                needsRepaint = true;
            }
        }

        if (guiEvent.type == EventType.MouseUp && guiEvent.button == 0)
        {
            mouseIsDownOverKey = false;
        }

        if (mouseIsDownOverKey && guiEvent.type == EventType.MouseDrag && guiEvent.button == 0)
        {
            float keyTime = Mathf.InverseLerp(gradientPreviewRect.x, gradientPreviewRect.xMax, guiEvent.mousePosition.x);
            keyIndex = gradient.UpdateKeyTime(keyIndex, keyTime);
            needsRepaint = true;
        }

        if(guiEvent.keyCode == KeyCode.Backspace && guiEvent.type == EventType.KeyDown)
        {
            gradient.RemoveKey(keyIndex);

            if(keyIndex>=gradient.NumKeys)
            {
                keyIndex--;
            }

            needsRepaint = true;
        }

    }

    public void SetGradient(CustomGradient gradient)
    {
        this.gradient = gradient;
    }

    private void OnEnable()
    {
        titleContent.text = "Gradient Editor";
        position.Set(position.x, position.y, 400, 150);
        minSize = new Vector2(200, 150);
        maxSize = new Vector2(1920, 150);
    }

    private void OnDisable()
    {
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
}