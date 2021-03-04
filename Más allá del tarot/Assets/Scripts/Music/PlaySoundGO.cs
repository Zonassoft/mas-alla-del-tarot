using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySoundGO : MonoBehaviour
{
    public delegate void ClickButton(int index);
    public static event ClickButton OnClicked;
    public int SoundIndex;

    public void PlayButtonSound()
    {
        OnClicked.Invoke(SoundIndex);
    }
}
