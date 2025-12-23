using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioClip sound;
    public float volume = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position, volume);
        CoinsText.coins++;
        Destroy(gameObject);
    }
}
