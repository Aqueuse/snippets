using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Dialogues {
    public class SpeechToVoice : MonoBehaviour {
        // place your sounds (A to Z sounds) on a scriptableObject to optimize your data access
        // inside this scriptable Object, create a public GenericDictionary<Char, AudioClip> characterToClip
        
        [SerializeField] private GibberishAudioDataScriptableObject gibberishAudioDataScriptableObject;
        [SerializeField] private AudioSource miniChimpAudioSource;

        private Queue<AudioClip> clipsQueue;

        private void Start() {
            clipsQueue = new Queue<AudioClip>();
            
            Play("miaou");
        }

        public void Play(string text) {
            Char[] charactersArray = text.ToLower().ToCharArray();
            
            foreach (var character in charactersArray) {
                clipsQueue.Enqueue(Char.IsLetter(character)
                    ? gibberishAudioDataScriptableObject.characterToClip[character]
                    : gibberishAudioDataScriptableObject.characterToClip['f']);
            }
        }

        private void Update() {
            if (miniChimpAudioSource.isPlaying || clipsQueue.Count <= 0) return;
            
            miniChimpAudioSource.clip = clipsQueue.Dequeue();
            miniChimpAudioSource.Play();
        }
    }
}
