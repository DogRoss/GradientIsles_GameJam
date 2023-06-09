using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
 * Author: Justin Scruggs
 * 
 * Originally made for Cosmender, this AudioManager returns, more powerful than ever.
 * This AudioManager only needs to be placed in the title screen scene. We now have the power of DontDestroyOnLoad.
 * 
 * To use, add your clips in the AudioClips array.
 * Then, you can use code to play audio from AudioManager.Play(index, oneshot, volume, location);
 * This system also uses an AudioMixer provided in Unity.
 * 
 */
public class AudioManager : MonoBehaviour
{
    readonly List<AudioSRC> sources = new List<AudioSRC>();

    public static AudioManager instance;

    [SerializeField] public AudioMixer mixer;

    public AudioClip[] clips = new AudioClip[0];

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        int count = Mathf.Min(32, transform.childCount);
        for (int i = 0; i < count; i++)
        {
            Transform c = transform.GetChild(i);
            if (c.GetComponent<AudioSource>() != null)
            {
                sources.Add(new AudioSRC(c.gameObject));
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    public static void Play(int index, bool oneshot, float volume, Vector3 location)
    {
        if (instance == null)
        {
            Debug.LogWarning("There is no Audio Manager loaded.");
            return;
        }
        instance.PlayClip(index, oneshot, volume, location);
    }

    public static void Play(string name, bool oneshot, float volume, Vector3 location)
    {
        if (instance == null)
        {
            Debug.LogWarning("There is no Audio Manager loaded.");
            return;
        }

        foreach (AudioClip clip in instance.clips)
        {
            if (clip.name.ToLower() == name.ToLower())
            {
                instance.PlayClip(clip, oneshot, volume, location);
                return;
            }
        }

        Debug.LogWarning("No sound named " + name + ", skipping");
    }

    public static void Play(int index, float volume, Vector3 location)
    {
        Play(index, false, volume, location);
    }

    public static void Play(string name, float volume, Vector3 location)
    {
        Play(name, false, volume, location);
    }

    public static void Play(int index, float volume)
    {
        Play(index, false, volume, new Vector3());
    }

    public static void Play(string name, float volume)
    {
        Play(name, false, volume, new Vector3());
    }

    public static void PlayLoop(int index, float volume, Vector3 location)
    {
        if (instance == null)
        {
            Debug.LogWarning("There is no Audio Manager loaded.");
            return;
        }
        instance.PlayClip(index, volume, location);
    }

    public static void PlayLoop(int index, float volume)
    {
        PlayLoop(index, volume, new Vector3());
    }

    [ContextMenu("Play Clip 0")]
    public void PlayClip()
    {
        PlayClip(0, false, 1f, transform.position);
    }

    public void PlayClip(int index, bool oneshot, float volume, Vector3 location)
    {
        PlayClip(clips[index], oneshot, volume, location);
    }
    public void PlayClip(AudioClip clip, bool oneshot, float volume, Vector3 location)
    {
        AudioSRC asrc = GetAudioSource();
        if (asrc != null)
        {
            if (oneshot)
            {
                asrc.PlayOneShot(clip, volume, location);
                return;
            }
            asrc.Play(clip, volume, location);
        }
    }

    public void PlayClip(int index, float volume, Vector3 location)
    {
        PlayClip(clips[index], false, volume, location);
    }

    public void PlayClip(AudioClip clip, float volume, Vector3 location)
    {
        AudioSRC asrc = GetAudioSource();
        if (asrc != null)
        {
            asrc.Play(clip, volume, location);
        }
    }

    public void PlayLoopClip(int index, float volume, Vector3 location)
    {
        AudioSRC asrc = GetAudioSource();
        if (asrc != null)
        {
            asrc.PlayLoop(clips[index], volume, location);
        }
    }

    public static void StopAllSounds()
    {
        if (instance == null)
        {
            Debug.LogWarning("There is no Audio Manager loaded.");
            return;
        }

        foreach (AudioSRC src in instance.sources)
        {
            src.go.GetComponent<AudioSource>().Stop();
        }
    }

    public static void Stop(string name)
    {
        if (instance == null)
        {
            Debug.LogWarning("There is no Audio Manager loaded.");
            return;
        }

        foreach (AudioSRC src in instance.sources)
        {
            AudioSource asrc = src.go.GetComponent<AudioSource>();
            if (asrc.clip != null && asrc.clip.name == name)
            {
                asrc.Stop();
            }
        }
    }

    public static void Stop(int index)
    {
        if (instance == null)
        {
            Debug.LogWarning("There is no Audio Manager loaded.");
            return;
        }

        foreach (AudioSRC src in instance.sources)
        {
            src.Stop();
        }
    }


    public void StopAll()
    {
        foreach (AudioSRC src in sources)
        {
            src.Stop();
        }
    }

    private AudioSRC GetAudioSource()
    {
        foreach (AudioSRC src in sources)
        {
            if (src.IsFree)
            {
                return src;
            }
        }
        return null;
    }

    class AudioSRC
    {
        public GameObject go;
        public bool IsFree
        {
            get
            {
                return Time.time > freeAt;
            }
        }

        private float freeAt = 0f;

        public AudioSRC(GameObject go)
        {
            this.go = go;
        }

        public void Play(AudioClip clip, float volume, Vector3 location)
        {
            freeAt = Time.time + clip.length;
            go.GetComponent<AudioSource>().clip = clip;
            PlayClip(volume, false, location);
        }

        public void PlayOneShot(AudioClip clip, float volume, Vector3 location)
        {
            go.GetComponent<AudioSource>().clip = clip;
            PlayClip(volume, true, location);
        }

        private void PlayClip(float volume, bool oneshot, Vector3 location)
        {
            AudioSource src = go.GetComponent<AudioSource>();
            src.gameObject.transform.position = location;
            src.volume = volume;
            src.clip.LoadAudioData();
            if (oneshot)
            {
                src.PlayOneShot(src.clip);
                return;
            }
            src.loop = false;
            src.Play();
        }

        public void PlayLoop(AudioClip clip, float volume, Vector3 location)
        {
            go.GetComponent<AudioSource>().clip = clip;
            AudioSource src = go.GetComponent<AudioSource>();
            src.gameObject.transform.position = location;
            src.volume = volume;
            src.clip.LoadAudioData();
            src.loop = true;
            freeAt = float.MaxValue;
            src.Play();
        }

        public void Stop()
        {
            go.GetComponent<AudioSource>().Stop();
            freeAt = Time.time;
        }
    }
}
