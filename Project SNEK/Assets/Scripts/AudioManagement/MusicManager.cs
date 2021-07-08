using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Player;
using Saving;
using UnityEngine.Audio;

namespace AudioManagement
{
    public class MusicManager : Singleton<MusicManager>
    {
        private void Awake()
        {
            CreateSingleton(true);
        }        


        [Header("Current Music")]

        public Sound currentMusic;

        private Sound lastMusic;

        [Header("AudioSources")]

        public AudioSource playingMusicSource1;

        public AudioSource playingMusicSource2;

        [Header("List et Volumes")]

        public SoundsList musicList;

        public float defaultFadeTime;

        [Header("Mixers")]

        public AudioMixer soundsMixer;

        public AudioMixerGroup musicMixer;

        private bool isChangingMusic;
        private bool fadeIn;
        private bool fadeOut;


        // Start is called before the first frame update
        void Start()
        {
            playingMusicSource1 = gameObject.AddComponent<AudioSource>();
            playingMusicSource2 = gameObject.AddComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        [ContextMenu("TestMusic")]
        public void TestMusic()
        {
            StartCoroutine(ChangeMusic("FallVillage"));

        }

        public void Music(String music)
        {
            StartCoroutine(ChangeMusic(music));
        }


        public IEnumerator ChangeMusic(String music)
        {
            if (isChangingMusic == false)
            {
                isChangingMusic = true;

                if(music == null)
                {
                    fadeOut = true;

                    StartCoroutine(FadeOut(playingMusicSource1, defaultFadeTime));

                    while (fadeOut == true)
                    {
                        yield return null;
                    }
                }
                else if (playingMusicSource1.isPlaying == false)
                {
                    AttributeMusic(playingMusicSource1, music);

                    fadeIn = true;

                    StartCoroutine(FadeIn(playingMusicSource1, defaultFadeTime));

                    while (fadeIn == true)
                    {
                        yield return null;
                    }
                }
                else
                {
                    AttributeMusic(playingMusicSource2, music);

                    fadeIn = true;
                    fadeOut = true;

                    StartCoroutine(FadeIn(playingMusicSource2, defaultFadeTime));
                    StartCoroutine(FadeOut(playingMusicSource1, defaultFadeTime));

                    while (fadeIn != false && fadeOut != false)
                    {
                        yield return null;
                    }

                    playingMusicSource2 = playingMusicSource1;

                    playingMusicSource1 = currentMusic.source;
                }

                isChangingMusic = false;
            }
        }

        public void AttributeMusic(AudioSource source, String name)
        {
            //On cherche la musique dans la liste des musiques        

            Sound m = Array.Find(musicList.sounds, Sound => Sound.name == name);

            //Ensuite, o créer une variable sound qui va garder les informations ud son que l'on va jouer.

            Sound sound = new Sound();

            sound.clip = m.clip;
            sound.volume = m.volume;
            sound.source = source;

            currentMusic = sound;

            //Enfin, on fait jouer la musique par la source

            source.clip = m.clip;
            source.volume = m.volume;

            source.loop = true;

            source.outputAudioMixerGroup = musicMixer;

        }

        //Fonction qui permet de FadeOut un audiosource
        public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
            fadeOut = false;
        }

        //Fonction qui permet de FadeIn une audiosource.
        public IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
        {
            
            audioSource.volume = 0;
            audioSource.Play();

            float FinalVolume = currentMusic.volume;

            while (audioSource.volume < FinalVolume)
            {
                audioSource.volume += FinalVolume * Time.deltaTime / FadeTime;
                yield return null;
            }

            audioSource.volume = FinalVolume;

            fadeIn = false;
        }
    }
}

