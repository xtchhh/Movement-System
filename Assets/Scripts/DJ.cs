using UnityEngine;
using UnityEngine.InputSystem;

public class DJ : MonoBehaviour
{
    private GameObject music;
    private AudioSource audiosource;

    void Awake()
    {
        music = GameObject.Find("Music");
        audiosource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && IsMusicOn())
        {
            audiosource.PlayDelayed(0.1f);
            music.SetActive(false);
        }

        else if (Keyboard.current.fKey.wasPressedThisFrame && !IsMusicOn())
        {
            music.SetActive(true);
        }
        
    }

    private bool IsMusicOn()
    {
        music.SetActive(true);
        return true;
    }
}
