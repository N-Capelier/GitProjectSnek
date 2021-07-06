using System.Collections.Generic;
using UnityEngine;
using System;
using Player;
using Saving;
using UnityEngine.Audio;

namespace AudioManagement
{
    /// <summary>
    /// William Schmitt
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {

        private void Awake()
        {
            CreateSingleton(true);
        }

        //Le tableau contenant les Audosources sur l'objet
        public Source[] sourcesAudio;

        //La List qui gère tous les sons actuellement joués sur une audiosource de cet objet
        public List<Sound> sounds = new List<Sound>();

        //La List qui gère tous les sons actuellement joués sur une audiosource de cet objet
        public List<Sound> musics = new List<Sound>();

        //Variable qui récupère la liste de tous les sons dans la base de donnée.
        public SoundsList soundsList;

        //La variable sui controle le volume global des effet sonores. Multiplicateur de 0 a 1.
        [Range(0f, 1f)]
        public float SoundEffectsVolume = 0.9f;

        //La variable sui controle le volume global des musiques. Multiplicateur de 0 a 1.
        [Range(0f, 1f)]
        public float MusicsVolume = 0.9f;

        //Une variable public pour centraliser la distance a laquelle on joue les sons.
        public float minimumSoundPlayDistance;

        public AudioMixer soundsMixer;

        public AudioMixerGroup musicMixer;
        public AudioMixerGroup sfxMixer;


        void Start()
        {
            SaveManager.Instance.Load();
            MusicsVolume = SaveManager.Instance.state.musicVolume;
            SoundEffectsVolume = SaveManager.Instance.state.soundVolume;

            //Création des Audiosources au start en fonction du nombre d'audiosources dans le tableau.

            foreach (var source in sourcesAudio)
            {

                source.audioSource = gameObject.AddComponent<AudioSource>();


            }
            



        }

        private void Update()
        {
            UdpdateList();
            //UpdateSliders();
        }

        public void UpdateSliders()
        {
            foreach (var sound in sounds)
            {
                sound.source.volume = sound.volume * SoundEffectsVolume;
            }

            foreach (var music in musics)
            {
                music.source.volume = music.volume * MusicsVolume;
            }

        }


        //Fontion pour jouer un son simplement.
        public void PlaySoundEffect(string soundName)
        {

            //On cherche le son que l'on va jouer dans la liste de son.

            Sound s = Array.Find(soundsList.sounds, Sound => Sound.name == soundName);




            //Ensuite, on cherche la première source qui n'est pas entrain de jouer un son et le fait jouer le son.
            foreach (var source in sourcesAudio)
            {

                if (source.audioSource.isPlaying == false)
                {
                    Sound sound = new Sound();

                    sound.clip = s.clip;
                    sound.volume = s.volume * SoundEffectsVolume;

                    sound.source = source.audioSource;

                    sounds.Add(sound);

                    source.audioSource.clip = s.clip;
                    source.audioSource.volume = s.volume * SoundEffectsVolume;

                    source.audioSource.loop = false;

                    source.audioSource.outputAudioMixerGroup = sfxMixer;

                    source.audioSource.Play();

                    break;
                }
            }
        }

        //Fonction qui permet de jouer un son mais qui sauvgarde la référence a son Audiosource pour être utilisé par l'auteur du script
        public Source PlayThisSoundEffect(string soundName)
        {

            //On cherche le son que l'on va jouer dans la liste de son.
            Sound s = Array.Find(soundsList.sounds, Sound => Sound.name == soundName);

            Sound m = Array.Find(soundsList.musics, Sound => Sound.name == soundName);

            //Variable locale pour stocker la référence a l'aurisource.
            Source sourceTemp = null;


            //Ensuite, on cherche la première source qui n'est pas entrain de jouer un son et le fait jouer le son.
            foreach (var source in sourcesAudio)
            {
                if (source.audioSource.isPlaying == false)
                {
                    Sound sound = new Sound();

                    if (s != null)
                    {
                        sound.clip = s.clip;
                        sound.volume = s.volume * SoundEffectsVolume;

                        sound.source = source.audioSource;

                        sounds.Add(sound);

                        source.audioSource.outputAudioMixerGroup = sfxMixer;
                    }
                    else if (m != null)
                    {
                        sound.clip = m.clip;
                        sound.volume = m.volume * SoundEffectsVolume;

                        sound.source = source.audioSource;

                        musics.Add(sound);

                        source.audioSource.outputAudioMixerGroup = musicMixer;
                        s = m;
                    }                    

                    source.audioSource.clip = s.clip;
                    source.audioSource.volume = s.volume * SoundEffectsVolume;

                    source.audioSource.Play();

                    sourceTemp = source;

                    break;
                }


            }
            return sourceTemp;

        }

        public Source PlayThisSoundEffect(string soundName, bool loop)
        {
            //On cherche le son que l'on va jouer dans la liste de son.
            Sound s = Array.Find(soundsList.sounds, Sound => Sound.name == soundName);

            Sound m = Array.Find(soundsList.musics, Sound => Sound.name == soundName);

            //Variable locale pour stocker la référence a l'aurisource.
            Source sourceTemp = null;


            //Ensuite, on cherche la première source qui n'est pas entrain de jouer un son et le fait jouer le son.
            foreach (var source in sourcesAudio)
            {

                if (source.audioSource.isPlaying == false)
                {
                    Sound sound = new Sound();

                    if (s != null)
                    {
                        sound.clip = s.clip;
                        sound.volume = s.volume;

                        sound.source = source.audioSource;

                        sounds.Add(sound);

                        source.audioSource.outputAudioMixerGroup = sfxMixer;
                    }
                    else if (m != null)
                    {
                        sound.clip = m.clip;
                        sound.volume = m.volume;

                        sound.source = source.audioSource;

                        musics.Add(sound);

                        source.audioSource.outputAudioMixerGroup = musicMixer;
                        s = m;
                    }

                    source.audioSource.volume = s.volume;
                    source.audioSource.clip = s.clip;

                    source.audioSource.loop = loop;

                    source.audioSource.Play();

                    sourceTemp = source;

                    break;
                }


            }
            return sourceTemp;
        }
        //On update la list en retirant tous les sons qui ne se jouent plus.
        private void UdpdateList()
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                if (sounds[i].source.isPlaying == false)
                {
                    InitialiseSource(sounds[i].source);
                }
            }
        }

        //Fonction alternative qui joue le son uniquement si la distance au joueur de l'objet est suffisement petite. Cette version de la fonction utilise la variable de distace globale.
        public Source PlayThisSoundEffect(string soundName, Transform here)
        {
            if(PlayerManager.Instance.currentController == null)
                return null;

            if (Vector3.Distance(here.position, PlayerManager.Instance.currentController.transform.position) <= minimumSoundPlayDistance)
            {
                //On cherche le son que l'on va jouer dans la liste de son.
                Sound s = Array.Find(soundsList.sounds, Sound => Sound.name == soundName);

                //Variable locale pour stocker la référence a l'aurisource.
                Source sourceTemp = null;

                //Ensuite, on cherche la première source qui n'est pas entrain de jouer un son et le fait jouer le son.
                foreach (var source in sourcesAudio)
                {

                    if (source.audioSource.isPlaying == false)
                    {

                        Sound sound = new Sound();

                        sound.clip = s.clip;
                        sound.volume = s.volume;

                        sound.source = source.audioSource;

                        sounds.Add(sound);

                        source.audioSource.clip = s.clip;
                        source.audioSource.volume = s.volume;

                        source.audioSource.outputAudioMixerGroup = sfxMixer;

                        source.audioSource.Play();

                        sourceTemp = source;

                        break;
                    }


                }
                return sourceTemp;
            }

            return null;
        }

        //Fonction alternative qui joue le son uniquement si la distance au joueur de l'objet est suffisement petite. Cette version de la fonction permet d'entrer manuellement la distance.
        public Source PlayThisSoundEffect(string soundName, Transform here, float distance)
        {

            if (Vector3.Distance(here.position, PlayerManager.Instance.currentController.transform.position) <= distance)
            {
                //On cherche le son que l'on va jouer dans la liste de son.
                Sound s = Array.Find(soundsList.sounds, Sound => Sound.name == soundName);

                //Variable locale pour stocker la référence a l'aurisource.
                Source sourceTemp = null;

                //Ensuite, on cherche la première source qui n'est pas entrain de jouer un son et le fait jouer le son.
                foreach (var source in sourcesAudio)
                {

                    if (source.audioSource.isPlaying == false)
                    {

                        Sound sound = new Sound();

                        sound.clip = s.clip;
                        sound.volume = s.volume;

                        sound.source = source.audioSource;

                        sounds.Add(sound);

                        source.audioSource.clip = s.clip;
                        source.audioSource.volume = s.volume;

                        source.audioSource.outputAudioMixerGroup = sfxMixer;

                        source.audioSource.Play();

                        sourceTemp = source;

                        break;
                    }


                }
                return sourceTemp;
            }

            return null; 
        }

        public void PlayOneSoundEffect(string soundName)
        {
            bool isAlreadyPlaying = true;

            //On cherche le son que l'on va jouer dans la liste de son.
            Sound s = Array.Find(soundsList.sounds, Sound => Sound.name == soundName);

            //Ensuite, on cherche la première source qui n'est pas entrain de jouer un son et le fait jouer le son.
            foreach (var source in sourcesAudio)
            {

                if (source.audioSource.isPlaying == true && source.audioSource.clip.name == "soundName")
                {

                    isAlreadyPlaying = true;
                    break;

                }
                else
                {
                    isAlreadyPlaying = false;
                }

            }

            if (isAlreadyPlaying == false)
            {
                foreach (var source in sourcesAudio)
                {
                    if (source.audioSource.isPlaying == false)
                    {
                        Sound sound = new Sound();

                        sound.clip = s.clip;
                        sound.volume = s.volume;

                        sound.source = source.audioSource;

                        sounds.Add(sound);


                        source.audioSource.clip = s.clip;
                        source.audioSource.volume = s.volume;

                        source.audioSource.outputAudioMixerGroup = sfxMixer;

                        source.audioSource.loop = false;


                        source.audioSource.Play();

                        break;
                    }
                }
            }






        }

        public void PlayThisOneSoundEffect(string soundName, Transform here)
        {
            if (Vector3.Distance(here.position, PlayerManager.Instance.currentController.transform.position) <= minimumSoundPlayDistance)
            {
                bool isAlreadyPlaying = true;

                //On cherche le son que l'on va jouer dans la liste de son.
                Sound s = Array.Find(soundsList.sounds, Sound => Sound.name == soundName);

                //Ensuite, on cherche la première source qui n'est pas entrain de jouer un son et le fait jouer le son.
                foreach (var source in sourcesAudio)
                {

                    if (source.audioSource.isPlaying == true && source.audioSource.clip.name == "soundName")
                    {

                        isAlreadyPlaying = true;
                        break;

                    }
                    else
                    {
                        isAlreadyPlaying = false;
                    }

                }

                if (isAlreadyPlaying == false)
                {
                    foreach (var source in sourcesAudio)
                    {
                        if (source.audioSource.isPlaying == false)
                        {
                            Sound sound = new Sound();

                            sound.clip = s.clip;
                            sound.volume = s.volume;

                            sound.source = source.audioSource;

                            sounds.Add(sound);


                            source.audioSource.clip = s.clip;
                            source.audioSource.volume = s.volume;

                            source.audioSource.loop = false;

                            source.audioSource.outputAudioMixerGroup = sfxMixer;


                            source.audioSource.Play();

                            break;
                        }
                    }
                }
            }
        }

        
        private void InitialiseSource(AudioSource audioSource)
        {
            audioSource.clip = null;
            audioSource.volume = 0;

            audioSource.loop = false;
        }
    }


}