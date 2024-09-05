using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource effectsSource; //Este es el source para los soniditos de OneShot
    [SerializeField] private AudioSource musicSource; //Este pa la bgm
    [Header("Audios")] //Aqui se agregan los audios que se meten
    [SerializeField] private AudioClip tampocoSe;
    [SerializeField] private AudioClip idk;
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
            AudioEvents.OnShoot.AddListener(PlayShootSound);//Suscribirse al evento de AudioEvents
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

    private void OnDestroy()
    {
        AudioEvents.OnShoot.RemoveListener(PlayShootSound);//No olvidar desuscribirlo
    }
    private void PlayShootSound()
    {
        effectsSource.PlayOneShot(gunShot); //Función de hacer sonar al sonidito
    }
}
