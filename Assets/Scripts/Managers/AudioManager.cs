using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource effectsSource; //Este es el source para los soniditos de OneShot
    [SerializeField] private AudioSource musicSource; //Este pa la bgm
    [Header("Audios")] //Aqui se agregan los audios que se meten
    [SerializeField] private AudioClip hitEnemy;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip gunShot;

    [SerializeField] private AudioClip backgroundMusicClip;

    [SerializeField] private static AudioManager instance;
    public static AudioManager GetInstance() => instance;

    private void Awake()
    {
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            PlayBackgroundMusic();
        }
        effectsSource = GetComponent<AudioSource>();
    }

    public void PlayBackgroundMusic()
    {
        musicSource.clip = backgroundMusicClip;
        musicSource.loop = true;  
        musicSource.Play();
    }

    public void PlayShootSound(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip); //Funci�n de hacer sonar al sonidito
    }
    public void PlayReloadSound(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip); //Funci�n de hacer sonar al sonidito
    }

    public void PlayHitEnemySound() => effectsSource.PlayOneShot(hitEnemy);
    public void PlayHitPlayerSound() => effectsSource.PlayOneShot(hit);
    public void PlayDeathSound() => effectsSource.PlayOneShot(death);

}
