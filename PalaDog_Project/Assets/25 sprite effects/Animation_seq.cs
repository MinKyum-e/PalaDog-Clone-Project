using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Animation_seq: MonoBehaviour
{
    public float fps = 24.0f;
    public Sprite[] frames;

    private int frameIndex;
    private Image rendererMy;
    

    void Start()
    {
        rendererMy = GetComponent<Image>();
        NextFrame();
        InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
    }

    void NextFrame()
    {
        rendererMy.sprite = frames[frameIndex];
        frameIndex = (frameIndex + 0001) % frames.Length;
    }
}