using UnityEngine;
using System.Collections;

public class music_script : MonoBehaviour
{
    Global_Variables _GlobalVariables;
    GameObject GC;

    public AudioClip[] clips;
    private AudioSource audioSource;
    public GameObject musicPlayer;

    public bool random = true;
    public int max_memory = 5;

    int current_clip = 0;
    int[] clip_history = new int[10];
    int clip_history_entries = 0;

    void Awake()
    {
        musicPlayer = GameObject.Find("MUSIC");
        if (musicPlayer == null)
        {
            musicPlayer = this.gameObject;
            musicPlayer.name = "MUSIC";
            DontDestroyOnLoad(musicPlayer);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            if (this.gameObject.name != "MUSIC")
            {
                Destroy(this.gameObject);
            }
        }
    }

    AudioLowPassFilter t1;

    void Start()
    {
        t1 = this.GetComponent("AudioLowPassFilter") as AudioLowPassFilter;

        GC = GameObject.Find("GlobalVariables_holder");
        if (GC != null) _GlobalVariables = GC.GetComponent<Global_Variables>();
        audioSource.loop = false;
        if (!audioSource.isPlaying)
        {
            audioSource.clip = GetRandomClip();
            audioSource.Play();
        }
        for (int i = 0; i < 10; i++)
        {
            clip_history[i] = -1;
        }
    }

    private AudioClip GetRandomClip()
    {
        int test = -1;

        int count = 0;
        while(test==-1)
        {
            int test_candidate = Random.Range(0, clips.Length);
            if (count < 50)
            {
                for (int i = 0; i < max_memory; i++)
                {
                    if (test_candidate == clip_history[i])
                    {
                        test_candidate = -1;
                    }
                }
                test = test_candidate;
            } else
            {
                test = test_candidate;
            }
            count++;
        }

        //Debug.Log(test);
        if (random == false) test = current_clip++;
        if (current_clip >= clips.LongLength) current_clip = 0;

        clip_history[clip_history_entries] = test;
        clip_history_entries++;
        if(clip_history_entries>=max_memory)
        {
            clip_history_entries = 0;
        }        
        return clips[test];
    }

    float pause_position = 100f;
    void Update()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.clip = GetRandomClip();
            audioSource.Play();
        }

        if (GC != null)
        {
            if (_GlobalVariables.pause == true)
            {
                //PAUSE
                if (pause_position < 100f) pause_position += 0.1f;
                if (pause_position > 100f) pause_position = 100f;
                t1.enabled = true;

            }
            else
            {
                //UNPAUSE
                if (pause_position > 99f) pause_position -= 0.1f;
                if (pause_position < 99f) pause_position = 99f;
                t1.enabled = false;
            }

        }
        transform.localPosition = new Vector3(0f, 0f, -pause_position);

    }

}