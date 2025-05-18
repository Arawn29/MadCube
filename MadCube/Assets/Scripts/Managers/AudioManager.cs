using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioClip mainMusic;
    AudioSource audioSource;

    
    private void Awake()
    {
     
       
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void SetVolume()
    {
        float volume = PlayerPrefs.GetFloat("Main_Music_Volume",1f);
        audioSource.volume = volume;
    }


}
