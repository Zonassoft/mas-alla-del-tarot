using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySoundButton : MonoBehaviour
{
    public delegate void ClickButton(int index);
    public static event ClickButton OnClicked;
    public int SoundIndex;

    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(PlayButtonSound);
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(PlayButtonSound);
    }

    public void PlayButtonSound()
    {
        OnClicked?.Invoke(SoundIndex);
    }
}