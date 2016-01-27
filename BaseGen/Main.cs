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
using System.IO;
#endregion

namespace BaseGen
{
    public class Main : Game
    {
        public static bool debug;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch lightBatch;
        BlendState blend;
        private float fps, fps_total;
        private float fpsClock;
        public static string levelPath;
        public static string signPath;
        public static SpriteFont font;
        /// <summary>
        /// 'Virtual' resolution of the game before it has been drawn to the screen
        /// </summary>
        public static Vector2 virtualResolution;
        /// <summary>
        /// Enum describing the game's state
        /// </summary>
        public enum GameState
        {
            Menu = 0,
            Playing = 1,
            Paused = 2,
            Editor = 3,
        }
        /// <summary>
        /// Current 'GameState'
        /// </summary>
        private static GameState gameState;
        /// <summary>
        /// Directory to the content folder
        /// </summary>
        public static string contentPath;
        public Main()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            contentPath = "../../../Content/";

            // Set the preffered buffer to the size of the screen
            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferWidth = 1080;
            graphics.IsFullScreen = true;

            // Game runs at a virtual 1920x1080 resolution
            virtualResolution = new Vector2(960, 540);

            // Initialize the screen manager
            //Managers.ScreenManager.Initialize(virtualResolution, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
            Managers.ScreenManager.Initialize(virtualResolution, new Vector2(1920, 1080));
            base.Initialize();
        }

        public static GameState GetState()
        {
            return gameState;
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            lightBatch = new SpriteBatch(GraphicsDevice);
            #region initialize managers
            Managers.AssetManager.Initialize();
            Managers.Executive.Initialize();
            Managers.User.Initialize();
            Managers.Camera.Initialize();
            #endregion
            #region load content
            font = Content.Load<SpriteFont>("Textures/Arial");
            System.IO.StreamReader reader;
            string[] files;
            files = Directory.GetFiles(contentPath + "TextureData");
            foreach (string fileName in files)
            {
                using (reader = new System.IO.StreamReader(fileName))
                    foreach (KeyValuePair<string, string> csv in Managers.Parser.ParseAsset(reader))
                    {
                        Texture2D texture;
                        string data;
                        texture = Content.Load<Texture2D>("Textures/" + csv.Value.Split('_')[0]);
                        data = csv.Value.Substring(csv.Value.IndexOf('_') + 1, (csv.Value.Length - csv.Value.IndexOf('_')) - 1);
                        Managers.AssetManager.CreateTextureAsset(csv.Key, data, texture);
                    }
            }
            //files = Directory.GetFiles(contentPath + "SoundData");
            //foreach (string fileName in files)
            //{
            //    using (reader = new System.IO.StreamReader(fileName))
            //        foreach (KeyValuePair<string, string> csv in Managers.Parser.ParseAsset(reader))
            //        {
            //            SoundEffect sound;
            //            bool focus;
            //            sound = Content.Load<SoundEffect>("Sounds/" + csv.Value.Split('_')[0]);
            //            focus = (csv.Value.Substring(csv.Value.IndexOf('_') + 1, (csv.Value.Length - csv.Value.IndexOf('_')) - 1)) == "1" ? true : false;
            //            Managers.AssetManager.CreateSoundAsset(csv.Key, focus, sound);
            //        }
            //}
            //files = Directory.GetFiles(Main.contentPath + "Music");
            //foreach (string fileName in files)
            //{
            //    Managers.AssetManager.NewSong(Content.Load<Song>(fileName));
            //}
            //Managers.AssetManager.SetMenuTheme(1);
            //Managers.AssetManager.SetEditorTheme(1);
            files = Directory.GetFiles(contentPath + "MenuData");
            foreach (string fileName in files)
            {
                Menus.Menu menu;
                using (reader = new System.IO.StreamReader(fileName))
                    menu = Managers.Parser.ParseMenuData(reader);
                Managers.Executive.menuDict.Add(menu.name, menu);
            }
            int index = 0;
            files = Directory.GetDirectories(contentPath + "Levels");
            using (reader = new System.IO.StreamReader(contentPath + "Save.txt"))
            {
                string tryLoad = Managers.Parser.LoadLastDirectory(reader);
                try
                {
                    if (tryLoad.Length != 0)
                        levelPath = tryLoad;
                }
                catch
                {
                    levelPath = files[0];
                }
            }

            files = Directory.GetFiles(levelPath);
            foreach (string fileName in files)
            {
                if (fileName.Contains("Level" + index.ToString()))
                    using (reader = new System.IO.StreamReader(fileName))
                        Managers.Executive.levels.Add(Managers.Parser.ParseLevel(reader));
                else if (fileName.Contains("Signs"))
                    using (reader = new System.IO.StreamReader(fileName))
                        Managers.Executive.signData.Add(Managers.Parser.ParseSignData(reader));
                index++;
            }
            files = Directory.GetFiles(contentPath + "AnimData");
            foreach (string fileName in files)
            {
                KeyValuePair<string, List<Point>[]> pair;
                using (reader = new System.IO.StreamReader(fileName))
                    pair = Managers.Parser.ParseAnimData(reader);
                Managers.AssetManager.CreateAnimData(pair.Key, pair.Value);
            }
            #endregion
            reader = new System.IO.StreamReader(contentPath + "TileData.txt");
            Managers.Executive.tileData = Managers.Parser.ParseTileDictionary(reader);
            newDemo();
        }
        public static void LoadSigns()
        {
            string[] files;
            int index = 0;
            files = Directory.GetDirectories(Main.levelPath);
            files = Directory.GetFiles(levelPath);
            foreach (string fileName in files)
            {
                if (fileName.Contains("Signs"))
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
                        Managers.Executive.signData.Add(Managers.Parser.ParseSignData(reader));
                index++;
            }

        }
        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Playing:
                    this.IsMouseVisible = false;
                    break;
                case GameState.Paused:
                    this.IsMouseVisible = true;
                    break;
                case GameState.Menu:
                    this.IsMouseVisible = true;
                    break;
                case GameState.Editor:
                    this.IsMouseVisible = true;
                    break;
                default:
                    break;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Managers.User.Update();
            Managers.Executive.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Managers.ScreenManager.scale);
            Managers.Executive.Draw(spriteBatch, lightBatch);
            if (debug)
            {
                fps++;
                fpsClock += gameTime.ElapsedGameTime.Milliseconds;
                if (fpsClock >= 1000)
                {
                    fps_total = fps;
                    fpsClock = 0;
                    fps = 0;
                }
                spriteBatch.DrawString(font, "FPS: " + fps_total.ToString(), new Vector2(5, 5), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected void newDemo()
        {
            ChangeState(GameState.Menu);
            Managers.Executive.menuStack.Push(Managers.Executive.menuDict["Menu"]);
        }
        public static void ChangeState(GameState state)
        {
            switch (state)
            {
                case GameState.Menu:

                    //Managers.AssetManager.PlaySong(Managers.AssetManager.menuTheme);
                    break;
                case GameState.Playing:
                    //Managers.AssetManager.PlayNextSong();
                    break;
                case GameState.Editor:

                    //Managers.AssetManager.PlaySong(Managers.AssetManager.editorTheme);
                    break;
                default:
                    break;
            }

            gameState = state;
        }
    }
}
