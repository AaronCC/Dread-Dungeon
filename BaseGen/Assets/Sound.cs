#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace BaseGen.Assets
{
    public class Sound
    {
        SoundEffect soundEffect { get; set; }
        SoundEffectInstance soundInstance { get; set; }
        bool focus { get; set; }

        public string name;

        public Sound(string _name, SoundEffect _soundEffect, bool _focus)
        {
            soundEffect = _soundEffect;
            focus = _focus;
            name = _name;
            soundInstance = null;
        }
        public void PlaySound()
        {
            if (soundInstance == null || soundInstance.IsDisposed)
                soundInstance = soundEffect.CreateInstance();
            if (soundInstance.State == SoundState.Playing)
                return;
            soundInstance.Play();
        }
        public void PlaySound(float volume, float pan, float pitch, bool looped)
        {
            if ( soundInstance == null || soundInstance.IsDisposed)
                soundInstance = soundEffect.CreateInstance();
            if (soundInstance.State == SoundState.Playing)
                return;
            soundInstance.Volume = volume;
            soundInstance.Pan = pan;
            soundInstance.Pitch = pitch;
            soundInstance.IsLooped = looped;
            soundInstance.Play();
        }
        public void PauseSound()
        {
            if (soundInstance == null || soundInstance.IsDisposed)
                soundInstance = soundEffect.CreateInstance();
            if (soundInstance.State == SoundState.Paused)
                return;
            soundInstance.Pause();
        }
        public void StopSound()
        {
            if (soundInstance == null || soundInstance.IsDisposed)
                soundInstance = soundEffect.CreateInstance();
            if (soundInstance.State == SoundState.Stopped)
                return;
            soundInstance.Stop();
        }
        public void DisposeSound()
        {
            if (soundInstance != null)
            {
                if (soundInstance.IsDisposed)
                    return;
                soundInstance.Dispose();
            }
        }
        public bool DisposeIfStopped()
        {
            if (soundInstance.State == SoundState.Stopped)
            {
                soundInstance.Dispose();
                return true;
            }
            return false;
        }
    }
}
