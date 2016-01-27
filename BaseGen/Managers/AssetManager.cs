#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
#endregion
namespace BaseGen.Managers
{
    public static class AssetManager
    {
        public static List<Song> songs;
        private static Dictionary<string, Assets.Texture> textures;
        private static Dictionary<string, Assets.Sound> sounds;
        private static Dictionary<string, List<Point>[]> animIndexes;
        private static Assets.Texture texture;
        private static Assets.Sound sound;
        private static string[] split;
        private static int songIndex;
        public static Song menuTheme;
        public static Song editorTheme;

        public static Assets.Texture GetTextureAsset(string name)
        {
            return textures[name];
        }
        public static Assets.Sound GetSoundAsset(string name)
        {
            return sounds[name];
        }
        public static List<Point>[] GetAnimIndexes(string name)
        {
            if (animIndexes.ContainsKey(name))
                return animIndexes[name];
            else return null;
        }
        public static void Initialize()
        {
            textures = new Dictionary<string, Assets.Texture>();
            sounds = new Dictionary<string, Assets.Sound>();
            animIndexes = new Dictionary<string, List<Point>[]>();
            songs = new List<Song>();
            songIndex = 0;
        }

        public static void CreateTextureAsset(string name, string data, Texture2D sprite)
        {
            int i = 0;
            int[] vertData = new int[3];
            split = data.Split(',');
            foreach (string csv in split)
            {
                vertData[i] = Convert.ToInt32(csv);
                i++;
            }
            texture = new Assets.Texture(name, sprite, vertData[0], vertData[1], vertData[2]);
            textures.Add(name, texture);
        }
        public static void CreateAnimData(string name, List<Point>[] startend)
        {
            animIndexes.Add(name, startend);
        }
        public static void CreateSoundAsset(string name, bool focus, SoundEffect soundEffect)
        {
            sound = new Assets.Sound(name, soundEffect, focus);
            sounds.Add(name, sound);
        }
        public static void NewSong(Song song)
        {
            songs.Add(song);
        }
        public static void SetMenuTheme(int index)
        {
            menuTheme = songs[index];
            songs.Remove(songs[index]);
        }
        public static void SetEditorTheme(int index)
        {
            editorTheme = songs[index];
            songs.Remove(songs[index]);
        }
        public static void PlaySong(Song song)
        {
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
        }
        public static void PlaySongFromIndex(int index)
        {
            MediaPlayer.Play(songs[index]);
            MediaPlayer.IsRepeating = true;
        }
        public static void PlayNextSong()
        {
            songIndex++;
            if (songIndex > songs.Count - 1)
                songIndex = 0;
            PlaySongFromIndex(songIndex);
        }
    }
}
