using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += Play;
        Play(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void Play(Scene scene, LoadSceneMode mode)
    {
        var MusicClip = Resources.Load<AudioClip>("Audio/Music/" + scene.name);
        if (MusicClip != null)
        {
            gameObject.GetComponent<AudioSource>().clip = MusicClip;
            gameObject.GetComponent<AudioSource>().Play();
        }
        Debug.Log(gameObject.GetComponent<AudioSource>().clip.name);
    }
}
