using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMain : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("Music/BKMain");
        audioSource.volume = 0.2f;
        audioSource.loop = true;
        audioSource.Play();
    }
}
