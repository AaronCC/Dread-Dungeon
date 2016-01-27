#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using System.IO;
#endregion

namespace BaseGen.Elements
{
    public class Level
    {
        private char[,] backCharMap;
        private char[,] charMap;
        private Tile[,] tileMap;
        private Tile[,] backTileMap;
        private Light[,] lightMap;
        private List<LightSource> lightSources;
        private Assets.Texture background;
        private Color nullColor;
        private float prop;
        private List<Point> lightable;
        private List<string> signData;
        private int signIndex;
        public Queue<GameObject> removeQueue;
        private Random random;
        BlendState multiply;
        private Assets.Texture border;
        public Vector2 Start
        {
            get { return start; }
        }
        private Vector2 start;
        public Player Player
        {
            get { return player; }
        }
        private Player player;

        public List<GameObject> Objects
        {
            get { return objects; }
        }
        private List<GameObject> objects;

        public int Index
        {
            get { return index; }
        }
        private int index;
        public int Width
        {
            get { return charMap.GetLength(1) * Tile.width; }
        }
        public int Height
        {
            get { return charMap.GetLength(0) * Tile.height; }
        }
        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }
        SpriteFont font;
        public Level(int _index)
        {
            random = new Random();
            removeQueue = new Queue<GameObject>();
            font = Main.font;
            prop = 0.08f;
            lightable = new List<Point>();
            nullColor = new Color(0, 0, 0, 0);
            multiply = new BlendState()
            {
                AlphaSourceBlend = Blend.DestinationAlpha,
                AlphaDestinationBlend = Blend.Zero,
                AlphaBlendFunction = BlendFunction.Add,
                ColorSourceBlend = Blend.DestinationColor,
                ColorDestinationBlend = Blend.Zero,
                ColorBlendFunction = BlendFunction.Add
            };
            index = _index;
            lightSources = new List<LightSource>();
            objects = new List<GameObject>();
            charMap = Managers.Executive.levels[index][0];
            backCharMap = Managers.Executive.levels[index][1];
            tileMap = new Tile[charMap.GetLength(0), charMap.GetLength(1)];
            backTileMap = new Tile[charMap.GetLength(0), charMap.GetLength(1)];
            lightMap = new Light[charMap.GetLength(0), charMap.GetLength(1)];

        }
        public void NewEditLevel(int changeIndex)
        {
            border = Managers.AssetManager.GetTextureAsset("Border");
            background = Managers.AssetManager.GetTextureAsset("Level0");
            if (changeIndex == 0 || changeIndex == 2)
                for (int x = 0; x < charMap.GetLength(0); x++)
                {
                    for (int y = 0; y < charMap.GetLength(1); y++)
                    {
                        charMap[x, y] = '.';
                    }
                }
            Managers.Camera.NewLevel();
        }
        public void LoadLevel()
        {
            signData = Managers.Executive.signData[index];
            signIndex = 0;
            try
            {
                background = Managers.AssetManager.GetTextureAsset("Level" + index);
            }
            catch
            {
                background = Managers.AssetManager.GetTextureAsset("Level0");
            }
            Main.ChangeState(Main.GameState.Playing);

            for (int x = 0; x < charMap.GetLength(0); x++)
            {
                for (int y = 0; y < charMap.GetLength(1); y++)
                {
                    tileMap[x, y] = GenerateTile(charMap[x, y], x, y);
                }
            }

            for (int x = 0; x < backCharMap.GetLength(0); x++)
            {
                for (int y = 0; y < backCharMap.GetLength(1); y++)
                {
                    backTileMap[x, y] = GenerateBackTile(backCharMap[x, y], x, y);
                }
            }
            if (player == null)
                throw new NotSupportedException(String.Format("No starting position on level {0}", index));
            Managers.Executive.level = this;
            Managers.Camera.NewLevel();
        }

        public void ChangeTile(char input, int x, int y, bool back)
        {
            y = y < 0 ? 0 : y;
            if (!back)
                foreach (GameObject obj in objects)
                {
                    Vector2 eraseStart = Managers.Extensions.Erase_Position_TopLeft(obj.Position);
                    if (new Rectangle((int)eraseStart.X,(int)eraseStart.Y, Tile.width, Tile.height).Intersects(new Rectangle(y * Tile.width, x * Tile.height, Tile.width, Tile.height)))
                        removeQueue.Enqueue(obj);
                }
            while (removeQueue.Count > 0)
            {
                if (removeQueue.Peek().GetType() == typeof(Elements.Sign))
                    signIndex--;
                objects.Remove(removeQueue.Dequeue());
            }
            if (x < tileMap.GetLength(0) && y < tileMap.GetLength(1))
            {
                if (!back)
                {
                    tileMap[x, y] = GenerateTile(input, x, y);
                    charMap[x, y] = input;
                }
                else
                {
                    backTileMap[x, y] = GenerateBackTile(input, x, y);
                    backCharMap[x, y] = input;
                }
            }

        }
        public void EditUpdate()
        {
            for (int x = 0; x < charMap.GetLength(0); x++)
            {
                for (int y = 0; y < charMap.GetLength(1); y++)
                {
                    tileMap[x, y] = GenerateTile(charMap[x, y], x, y);
                }
            }

            for (int x = 0; x < backCharMap.GetLength(0); x++)
            {
                for (int y = 0; y < backCharMap.GetLength(1); y++)
                {
                    backTileMap[x, y] = GenerateBackTile(backCharMap[x, y], x, y);
                }
            }
        }
        public GameObject SingleTypeCollide(Type t, Rectangle hitbox)
        {
            foreach (GameObject obj in objects)
            {
                if (obj.IsColliding(hitbox) && t.IsAssignableFrom(obj.GetType()))
                {
                    return obj;
                }
            }
            return new GameObject();
        }

        public List<GameObject> AllCollide(Rectangle hitbox)
        {
            List<GameObject> collisions = new List<GameObject>();
            foreach (GameObject obj in objects)
            {
                if (obj.IsColliding(hitbox))
                {
                    collisions.Add(obj);
                }
            }
            return collisions;
        }
        public TileType CheckCollision(int x, int y)
        {
            if (x < 0 || x >= tileMap.GetLength(1))
                return TileType.Impassable;
            if (y < 0 || y >= tileMap.GetLength(0))
                return TileType.Passable;
            return tileMap[y, x].type;
        }
        public Tile GenerateBackTile(char tile, int x, int y)
        {
            switch (tile)
            {
                case ' ':
                    int i = random.Next(5);
                    return LoadBackTile("Back" + i.ToString(), x, y);
                default:
                    return LoadNullTile();
            }
        }
        public Tile GenerateTile(char tile, int x, int y)
        {
            Point pos = new Point(x, y);
            if (!lightable.Contains(pos))
                lightable.Add(pos);
            switch (tile)
            {
                case '.':

                    return LoadNullTile();
                case 'p':
                    LoadStart("Start", x, y);
                    //LoadBackTile("Start", x, y);
                    return LoadStartTile(x, y);
                case '-':

                    //LoadBackTile("Back", x, y);
                    return LoadTile("Platform", TileType.Platform);
                case '_':
                    //LoadBackTile("Back", x, y);
                    return LoadTile("BPlatform", TileType.BPlatform);
                case 'm':
                    return LoadMinotaur("Minotaur", x, y);
                case 'w':
                    backTileMap[x, y] = LoadBackTile("Back0", x, y);
                    return LoadTile("Wall" + random.Next(5).ToString(), TileType.Impassable);
                case '*':
                    //LoadBackTile("Back", x, y);
                    return LoadTile("Victory", TileType.Victory);
                case ',':
                    //LoadBackTile("Back", x, y);
                    return LoadSpike("Spikes", TileType.Obstacle, x, y, false, 0);
                case '"':
                    //LoadBackTile("Back", x, y);
                    return LoadSpike("Spikes", TileType.Obstacle, x, y, true, 0);
                case '>':
                    //LoadBackTile("Back", x, y);
                    return LoadSpike("Spikes", TileType.Obstacle, x, y, false, 1);
                case '<':
                    //LoadBackTile("Back", x, y);
                    return LoadSpike("Spikes", TileType.Obstacle, x, y, false, -1);
                case '/':
                    //LoadBackTile("Back", x, y);
                    return LoadTile("Ramp", TileType.Polynomial);
                case 'x':
                    return LoadBreakBlock("BreakBlock", x, y);
                //LoadBackTile("BreakBlock", x, y);
                case 'o':
                    return LoadFire("Fire", x, y);//LoadBackTile("Fire", x, y);
                case 't':
                    return LoadTorch("Torch", x, y);//LoadBackTile("Torch", x, y);
                case 's':
                    return LoadSign("Sign", x, y);//LoadBackTile("Sign", x, y);
                case 'a':
                    //LoadBackTile("Back", x, y);
                    return LoadSaw("Saw", TileType.Obstacle, x, y, 1);
                case '@':
                    //LoadBackTile("Back", x, y);
                    return LoadSaw("BigSaw", TileType.Obstacle, x, y, 2);
                #region Crushers
                case 'c':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 2, 0, true);
                case 'u':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 2, 0, false);
                case '(':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 3, 0, true);
                case ')':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 3, 0, false);
                case 'C':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 4, 0, true);
                case 'U':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 4, 0, false);
                case '[':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 4, 1, false);
                case '!':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 4, 1, true);
                case ']':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 4, 2, false);
                case '?':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 4, 2, true);
                case '{':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 3, 1, true);
                case '+':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 3, 2, false);
                case '}':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 3, 2, true);
                case '=':
                    //LoadBackTile("Back", x, y);
                    return LoadCrusher("Crusher", TileType.Obstacle, x, y, 3, 2, false);
                #endregion
                case 'h':
                    //LoadBackTile("Back", x, y);
                    return LoadHeart(x, y, "Heart");
                case 'q':
                    //LoadBackTile("Back", x, y);
                    return LoadSwingingBall(x, y, "SpikeBall", 0);
                case 'Q':
                    //LoadBackTile("Back", x, y);
                    return LoadSwingingBall(x, y, "SpikeBall", 500f);
                case 'l':
                    //LoadBackTile("Back", x, y);
                    return LoadTile("Ladder", TileType.Ladder);
                default:
                    return LoadNullTile();
            }
        }
        public void ExportLevel(SpriteBatch spriteBatch, int changeIndex, int index)
        {
            string path = Main.levelPath;
            System.IO.StreamWriter writer;
            FileStream fs;
            if (changeIndex == 0)
                fs = File.Create(path + "/" + "Level" + (Managers.Executive.levels.Count - 1).ToString() + ".txt");
            else
            {
                File.Delete(path + "/" + "Level" + index.ToString() + ".txt");
                fs = File.Create(path + "/" + "Level" + index.ToString() + ".txt");
            }
            writer = new System.IO.StreamWriter(fs);
            int i = 0;
            spriteBatch.DrawString(Main.font, "Exporting level...", new Vector2(0, 0), Color.White);
            i++;
            int rows = tileMap.GetLength(0);
            int cols = tileMap.GetLength(1);
            string line = string.Empty;
            string typed = string.Empty;
            line = rows.ToString() + "," + cols.ToString();

            writer.WriteLine(line);
            line = string.Empty;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    line += charMap[r, c];
                }
                writer.WriteLine(line);
                line = string.Empty;
            }
            line = string.Empty;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    line += backCharMap[r, c];
                }
                writer.WriteLine(line);
                line = string.Empty;
            }

            writer.Close();
            FileStream fs2;
            if (changeIndex == 0)
                fs2 = File.Create(path + "/" + "Signs" + (Managers.Executive.levels.Count - 1).ToString() + ".txt");
            else
            {
                File.Delete(path + "/" + "Signs" + index.ToString() + ".txt");
                fs2 = File.Create(path + "/" + "Signs" + index.ToString() + ".txt");
            }
            using (StreamWriter writer2 = new System.IO.StreamWriter(fs2))
            {
                for (int x = 0; x < signIndex; x++)
                {
                    writer2.WriteLine("Enter sign text here");
                }
            }
            Main.LoadSigns();
            Managers.Executive.menuStack.Pop();
            Main.ChangeState(Main.GameState.Menu);
        }
        public Tile LoadFire(string textureName, int x, int y)
        {
            Light l = new Light(Color.White, 0.6f, LightType.Point, new Point(x, y));
            Fire source = new Fire(new Vector2(y * Tile.width, x * Tile.height), textureName, l);
            objects.Add(source);
            lightSources.Add(source);
            lightMap[x, y] = l;
            lightable.Add(new Point(x, y));
            return LoadNullTile();
        }
        public Tile LoadTorch(string textureName, int x, int y)
        {
            lightMap[x, y] = LoadTorch(LightType.Point, Color.White, 0.2f, x, y, textureName);
            lightable.Add(new Point(x, y));
            return LoadNullTile();
        }
        public Tile LoadSign(string textureName, int x, int y)
        {
            LoadNullLight(x, y);

            try
            {
                objects.Add(new Sign(new Vector2(y * Tile.width, x * Tile.height), textureName, signData[signIndex]));
            }
            catch
            {
                objects.Add(new Sign(new Vector2(y * Tile.width, x * Tile.height), textureName, string.Empty));
            }
            signIndex++;
            return LoadNullTile();
        }
        public Tile LoadBreakBlock(string textureName, int x, int y)
        {
            LoadNullLight(x, y);
            TileObject breakBlock = new BreakBlock(new Vector2(y * Tile.width, x * Tile.height), textureName);
            objects.Add(breakBlock);
            return LoadTile("Back", TileType.Impassable);
        }
        public Tile LoadStart(string textureName, int x, int y)
        {
            LoadNullLight(x, y);
            return (LoadTile("Start", TileType.Passable));
        }
        public Tile LoadBackTile(string textureName, int x, int y)
        {
            LoadNullLight(x, y);
            lightable.Add(new Point(x, y));
            return backTileMap[x, y] = (LoadTile(textureName, TileType.Passable));
        }
        public void ChangeTileType(Point index, TileType type)
        {
            tileMap[index.Y, index.X].type = type;
        }
        public void LoadNullLight(int x, int y)
        {
            lightMap[x, y] = new Light(Color.White, 0, LightType.Null, new Point(x, y));
            lightable.Add(new Point(x, y));
        }
        public void RemoveLightSource(LightSource source)
        {
            lightSources.Remove(source);
        }
        public Light LoadTorch(LightType light, Color color, float intensity, int x, int y, string name)
        {
            Light l = new Light(color, intensity, light, new Point(x, y));
            Torch source = new Torch(new Vector2(y * Tile.width, x * Tile.height), name, new Light(color, intensity, light, new Point(x, y)));
            objects.Add(source);
            lightSources.Add(source);
            return l;
        }
        public Light LoadLightSource(LightType light, Color color, float intensity, int x, int y, string name)
        {
            Light l = new Light(color, intensity, light, new Point(x, y));
            LightSource source = new LightSource(new Vector2(y * Tile.width, x * Tile.height), name, new Light(color, intensity, light, new Point(x, y)));
            objects.Add(source);
            lightSources.Add(source);
            return l;
        }
        public LightSource GetLightSource(Point index)
        {
            foreach (LightSource s in lightSources)
            {
                if (s.light.position == index)
                    return s;
            }
            return null;
        }
        public Light AddLightSource(ref LightSource source)
        {
            lightSources.Add(source);
            return source.light;
        }
        public void BuildLightMap(GameTime gameTime)
        {
            foreach (Point p in lightable)
            {
                lightMap[p.X, p.Y].intensity = 0;
            }
            foreach (LightSource source in lightSources)
            {
                source.Update(gameTime);
                PropogateLight(source.light.intensity + prop, source.light.position.X, source.light.position.Y);
            }
        }
        public Color Mix(Color c1, Color c2, float alpha)
        {
            if (c1 != c2 && c1 != nullColor)
            {
                Color c = new Color()
                {
                    R = (Byte)((c1.R + c2.R) / 2.0f),
                    G = (Byte)((c1.G + c2.G) / 2.0f),
                    B = (Byte)((c1.B + c2.B) / 2.0f),
                    A = (Byte)(alpha)
                };
                return c;
            }
            else
                return c1;
        }
        public void PropogateLight(float source, int x, int y)
        {
            if (x < 0 || y < 0 || x >= charMap.GetLength(0) || y >= charMap.GetLength(1))
                return;

            if (lightMap[x, y].intensity >= source)
                return;

            float newLight = source - prop;

            if (newLight < 0)
            {
                lightMap[x, y].intensity = 0; return;
            }

            lightMap[x, y].intensity = newLight;

            if (tileMap[x, y].type == TileType.Impassable)
                return;

            PropogateLight(newLight, x + 1, y);
            PropogateLight(newLight, x - 1, y);
            PropogateLight(newLight, x, y + 1);
            PropogateLight(newLight, x, y - 1);
        }

        public Tile LoadSpike(string name, TileType type, int x, int y, bool flipV, int flipH)
        {
            objects.Add(new Spike(new Vector2(y * Tile.height, x * Tile.width), "Spike", flipV, flipH));
            return LoadNullTile();
        }
        public Tile LoadSaw(string name, TileType type, int x, int y, int size)
        {
            objects.Add(new Saw(new Vector2(y * Tile.height, x * Tile.width), "Saw", size));
            return LoadNullTile();
        }
        public Tile LoadCrusher(string name, TileType type, int x, int y, int railLength, int ctype, bool wait)
        {
            objects.Add(new Crusher(new Vector2(y * Tile.height, x * Tile.width), "Weight", railLength, ctype, wait));
            return LoadNullTile();
        }
        public Tile LoadHeart(int x, int y, string name)
        {
            objects.Add(new Items.Heart(new Vector2(y * Tile.width, x * Tile.height), name));
            return LoadNullTile();

        }
        public Tile LoadSwingingBall(int x, int y, string name, float delay)
        {
            objects.Add(new SwingingBall(new Vector2(y * Tile.width, x * Tile.height), "SpikeBall", delay));
            return LoadNullTile();
        }
        public Tile LoadNullTile()
        {
            return new Tile(null, TileType.Passable, "Null");
        }
        public Tile LoadTile(string textureName, TileType type)
        {
            return new Tile(Managers.AssetManager.GetTextureAsset(textureName), type, textureName);
        }
        public Tile LoadEnemy(string name, int x, int y)
        {
            Vector2 position = Managers.Extensions.Rect_BottomCenter(IndexToRect(x, y));
            objects.Add(new Enemy(position, name));

            return new Tile(null, TileType.Passable, name);
        }
        public Tile LoadMinotaur(string name, int x, int y)
        {
            Vector2 position = Managers.Extensions.Rect_BottomCenter(IndexToRect(y, x));
            objects.Add(new Minotaur(position, name));
            return LoadNullTile();
        }
        public Tile LoadStartTile(int x, int y)
        {
            if (player != null && Main.GetState() != Main.GameState.Editor)
                throw new NotSupportedException(String.Format("Level '{0}' has more than one starting point", index));
            start = Managers.Extensions.Rect_BottomCenter(new Rectangle(y * Tile.width, x * Tile.height, Tile.width, Tile.height));
            player = new Player(this, start);
            return new Tile(Managers.AssetManager.GetTextureAsset("Start"), TileType.Passable, "Start");
        }
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            while (removeQueue.Count > 0)
            {
                GameObject rm = removeQueue.Peek();
                if (rm.GetType().IsSubclassOf(typeof(LightSource)))
                    RemoveLightSource((LightSource)rm);
                objects.Remove(removeQueue.Dequeue());

            }
            foreach (GameObject obj in objects)
            {
                obj.Update(gameTime);
            }

            BuildLightMap(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch, SpriteBatch lightBatch)
        {
            Point start, end;
            start = new Point(Managers.Camera.Visible.X / Tile.width, Managers.Camera.Visible.Y / Tile.height);
            end = new Point((start.X + 1) + (Managers.Camera.Visible.Width / Tile.width) + 1, (start.Y + 1) + (Managers.Camera.Visible.Height / Tile.height) + 1);
            if (end.X > tileMap.GetLength(1))
                end.X = tileMap.GetLength(1);
            if (end.Y > tileMap.GetLength(0))
                end.Y = tileMap.GetLength(0);
            spriteBatch.Draw(background.sprite, new Rectangle(0, 0, (int)Main.virtualResolution.X, (int)Main.virtualResolution.Y), Color.White);
            DrawTiles(spriteBatch, lightBatch, start, end);

        }
        public void EditDraw(SpriteBatch spriteBatch)
        {
            Point start, end;
            start = new Point(Managers.Camera.Visible.X / Tile.width, Managers.Camera.Visible.Y / Tile.height);
            end = new Point((start.X + 1) + (Managers.Camera.Visible.Width / Tile.width) + 1, (start.Y + 1) + (Managers.Camera.Visible.Height / Tile.height) + 1);
            if (end.X > tileMap.GetLength(1))
                end.X = tileMap.GetLength(1);
            if (end.Y > tileMap.GetLength(0))
                end.Y = tileMap.GetLength(0);
            spriteBatch.Draw(background.sprite, new Rectangle(0, 0, (int)Main.virtualResolution.X, (int)Main.virtualResolution.Y), Color.Black);
            spriteBatch.Draw(background.sprite, new Rectangle((int)Managers.Camera.Offset.X * -1, (int)Managers.Camera.Offset.Y * -1, Tile.width * charMap.GetLength(1), Tile.height * charMap.GetLength(0)), Color.White);
            DrawEditTiles(spriteBatch, start, end);
        }
        public void DrawEditTiles(SpriteBatch spriteBatch, Point start, Point end)
        {
            for (int x = start.X; x < end.X; x++)
            {
                for (int y = start.Y; y < end.Y; y++)
                {

                    Assets.Texture backTexture = backTileMap[y, x].texture;
                    Vector2 position = new Vector2((x * Tile.width) - Managers.Camera.Offset.X, (y * Tile.height) - Managers.Camera.Offset.Y);
                    Rectangle drawRect = new Rectangle((int)position.X, (int)position.Y, Tile.width, Tile.height);
                    if (backTexture != null)
                    {
                        spriteBatch.Draw(backTexture.sprite, drawRect, Color.White);
                    }
                    Assets.Texture texture = tileMap[y, x].texture;
                    if (texture != null)
                    {
                        spriteBatch.Draw(texture.sprite, drawRect, Color.White);
                    }
                    spriteBatch.Draw(border.sprite, drawRect, Color.White * 0.2f);
                }
            }

            foreach (GameObject obj in objects)
            {
                Vector2 topLeft = Managers.Extensions.Position_TopLeft(obj.Position);
                Vector2 bottomRight = Managers.Extensions.Position_BottomRight(obj.Position);
                if ((bottomRight.X >= start.X * Tile.width && bottomRight.Y >= start.Y * Tile.height)
                    && (topLeft.X <= end.X * Tile.width && topLeft.Y <= end.Y * Tile.height))
                {
                    obj.Draw(spriteBatch);
                }
            }
        }
        public LightSource GetTorch(Rectangle Hitbox)
        {
            foreach (LightSource source in lightSources)
            {
                if (source.Name == "Torch")
                    if (source.IsColliding(Hitbox))
                        return source;
            }
            return null;
        }
        public List<Tile> GetBackTiles(Rectangle hitBox)
        {
            int w = 1, h = 1;
            List<Tile> tiles = new List<Tile>();
            while (hitBox.Width / Tile.width > 0)
            {
                w++;
                hitBox.Width -= Tile.width;
            }
            while (hitBox.Height / Tile.width > 0)
            {
                h++;
                hitBox.Height -= Tile.height;
            }

            Point pos = new Point(hitBox.X / Tile.width, hitBox.Y / Tile.height);
            for (int r = pos.Y; r < pos.Y + h; r++)
            {
                for (int c = pos.X; c < pos.X + w; c++)
                {
                    tiles.Add(backTileMap[r, c]);
                }
            }
            return tiles;
        }
        public void DrawTiles(SpriteBatch spriteBatch, SpriteBatch lightBatch, Point start, Point end)
        {
            for (int x = start.X; x < end.X; x++)
            {
                for (int y = start.Y; y < end.Y; y++)
                {
                    Assets.Texture backTexture = backTileMap[y, x].texture;
                    Vector2 position = new Vector2((x * Tile.width) - Managers.Camera.Offset.X, (y * Tile.height) - Managers.Camera.Offset.Y);
                    Rectangle drawRect = new Rectangle((int)position.X, (int)position.Y, Tile.width, Tile.height);
                    if (backTexture != null)
                    {
                        spriteBatch.Draw(backTexture.sprite, drawRect, Color.White);
                    }
                    Assets.Texture texture = tileMap[y, x].texture;
                    if (texture != null)
                    {

                        spriteBatch.Draw(texture.sprite, drawRect, Color.White);
                    }
                }
            }

            foreach (GameObject obj in objects)
            {
                Vector2 topLeft = Managers.Extensions.Position_TopLeft(obj.Position);
                Vector2 bottomRight = Managers.Extensions.Position_BottomRight(obj.Position);
                if ((bottomRight.X >= start.X * Tile.width && bottomRight.Y >= start.Y * Tile.height)
                    && (topLeft.X <= end.X * Tile.width && topLeft.Y <= end.Y * Tile.height))
                {
                    obj.Draw(spriteBatch);
                }
            }
            player.Draw(spriteBatch);
            spriteBatch.End();
            DrawLight(lightBatch, start, end);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Managers.ScreenManager.scale);
            player.DrawHud(spriteBatch);
        }
        public void DrawLight(SpriteBatch lightBatch, Point start, Point end)
        {
            lightBatch.Begin(SpriteSortMode.Immediate, multiply, null, null, null, null, Managers.ScreenManager.scale);
            Assets.Texture light = Managers.AssetManager.GetTextureAsset("Light");

            for (int x = start.X; x < end.X; x++)
            {
                for (int y = start.Y; y < end.Y; y++)
                {
                    Vector2 position = new Vector2((x * Tile.width) - Managers.Camera.Offset.X, (y * Tile.height) - Managers.Camera.Offset.Y);
                    Rectangle drawRect = new Rectangle((int)position.X, (int)position.Y, Tile.width, Tile.height);

                    lightBatch.Draw(light.sprite, drawRect, lightMap[y, x].color * lightMap[y, x].intensity);
                }
            }
            lightBatch.End();
        }
        public Rectangle IndexToRect(int x, int y)
        {
            return new Rectangle(x * Tile.width, y * Tile.height, Tile.width, Tile.height);
        }

        public static int RampFunction(int x)
        {
            if (x < 0)
                return 0;
            return (Tile.height - x);
        }
    }
}
