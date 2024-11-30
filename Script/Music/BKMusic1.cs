using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic1 : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource.clip = Resources.Load<AudioClip>("Music/BK1");
        audioSource.volume = 0.3f;
        audioSource.loop = true;
        audioSource.Play();
    }
}
