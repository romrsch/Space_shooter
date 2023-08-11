using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _effectSource;
 
 
    public AudioSource MusicSource => _musicSource;
    public AudioSource EffectSource => _effectSource;

    
    public static Controller Instance;

    private void Awake()
    { 
        Instance = this;
    }


}
