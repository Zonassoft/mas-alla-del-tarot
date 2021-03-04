using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUi : Singleton<SoundUi>
{
    public AudioClip[] SoundClips;
    public AudioSource AudioSource;
    public static SoundUi Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void OnEnable()
    {
        PlaySoundButton.OnClicked += PlaySound;
    }

    void OnDisable()
    {
        PlaySoundButton.OnClicked -= PlaySound;
    }

    public void PlaySound(int index)
    {
        AudioSource.PlayOneShot(SoundClips[index]);
    }
}