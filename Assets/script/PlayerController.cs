using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    float healthbar = 1;

    public Image healthBarImage;

    public bool isAlive = true;

    public AudioSource kobeAudioSource;

    public AudioClip[] audioClips;

    public void SetHealthBar(float health)
    {
        healthbar -= health;
        healthBarImage.fillAmount = healthbar;
    }

    void Update()
    {
        if (healthbar <= 0) {
            isAlive = false;
        }
    }

    public void GetHitSound()
    {
        kobeAudioSource.clip = audioClips[0];
        kobeAudioSource.Play();
    }

    public void GetDeadSound()
    {
        kobeAudioSource.clip = audioClips[1];
        kobeAudioSource.Play();
    }
}
