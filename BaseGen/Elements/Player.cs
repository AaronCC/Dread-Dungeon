#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
#endregion
namespace BaseGen.Elements
{
    public class Player
    {
        private bool collCheck;
        public float gravityAcceleration;
        private float jumpAcceleration;
        public SpriteEffects flip;
        private List<Point> animStarts;
        private List<Point> animEnds;
        private Point animStart;
        private Point animEnd;
        private Point animCurrent;
        public Keys[] pressedKeys;
        public int hearts;
        public int startHearts;
        private Assets.Texture heartAsset;
        public Assets.Texture texture;
        private float frameTime;
        Vector2 endBound;
        Vector2 startBound;
        private float previousBottom;
        private float maxFallSpeed;
        public float moveAcceleration;
        public float maxSpeed;
        private Rectangle ignoreTile;
        public bool onLadder;
        public Level Level
        {
            get { return level; }
        }
        Level level;
        public List<Vector2> circCollPoints;
        public States.State state;
        public States.Equip equip;
        public States.EffectState effectState;
        public Rectangle Hitbox
        {
            get
            {
                int thinX = 23;
                int thinY = 4;
                //return new Rectangle((int)position.X - (texture.Width / 2) + (thinX / 2), (int)position.Y - (texture.Height) + thinY, texture.Width - thinX, texture.Height - thinY);
                Vector2 drawPos = new Vector2(position.X - ((texture.Width / 2) / 2), position.Y - (texture.Height / 2));
                return new Rectangle((int)(drawPos.X) + (thinX / 2), (int)(drawPos.Y) + (thinY / 2), (texture.Width / 2) - (thinX), (texture.Height / 2) - thinY);
            }
        }
        public Rectangle Hitbox_ex
        {
            get
            {
                int thinX = 8;
                int thinY = 4;
                //return new Rectangle((int)position.X - (texture.Width / 2) + (thinX / 2), (int)position.Y - (texture.Height) + thinY, texture.Width - thinX, texture.Height - thinY);
                Vector2 drawPos = new Vector2(position.X - ((texture.Width / 2) / 2), position.Y - (texture.Height / 2));
                return new Rectangle((int)(drawPos.X) + (thinX / 2), (int)(drawPos.Y) + (thinY / 2), (texture.Width / 2) - (thinX), (texture.Height / 2) - thinY);
            }
        }
        public bool immune;
        public Vector2 velocity;
        public Vector2 Position
        {
            get { return position; }
        }
        private Vector2 position;
        public string Name
        {
            get { return name; }
        }
        private string name;
        public Point MidPoint
        {
            get { return MidPoint; }
        }

        Point midPoint;
        public Player(Level _level, Vector2 _position)
        {
            name = "Player";
            pressedKeys = Managers.User.kState.GetPressedKeys();
            level = _level;
            heartAsset = Managers.AssetManager.GetTextureAsset("Heart");
            position = _position;
            startHearts = 3;
            velocity = new Vector2();
            texture = Managers.AssetManager.GetTextureAsset(name);
            animStarts = Managers.AssetManager.GetAnimIndexes(name)[0];
            animEnds = Managers.AssetManager.GetAnimIndexes(name)[1];
            state = new States.Idle();
            equip = new States.Equip();
            moveAcceleration = 0.25f;
            maxFallSpeed = 15f;
            ignoreTile = Rectangle.Empty;
            gravityAcceleration = 0.15f;
            maxSpeed = 2.5f;
            jumpAcceleration = 3.4f;
            flip = SpriteEffects.None;
            NewAnimation(state.animState);
            effectState = null;
            immune = false;
            collCheck = false;
            midPoint = new Point(Hitbox.X + (Hitbox.Width / 2), Hitbox.Y + (Hitbox.Height / 2));
            circCollPoints = new List<Vector2>();
            circCollPoints.Add
                (new Vector2(Hitbox.X, Hitbox.Y));
            circCollPoints.Add
                (new Vector2(Hitbox.X + Hitbox.Width, Hitbox.Y));
            circCollPoints.Add
                (new Vector2(Hitbox.X, Hitbox.Y + Hitbox.Height));
            circCollPoints.Add
                (new Vector2(Hitbox.X + Hitbox.Width, Hitbox.Y + Hitbox.Height));
            circCollPoints.Add
                (new Vector2(Hitbox.X, Hitbox.Y + (Hitbox.Height / 2)));
            circCollPoints.Add
                (new Vector2(Hitbox.X + Hitbox.Width, Hitbox.Y + (Hitbox.Height / 2)));
            equip.EquipItem(this, new Items.TorchItem(position, "Torch", new Light(Color.White, 0.2f, LightType.Point, new Point((int)position.X, (int)position.Y))));
        }

        public void Update(GameTime gameTime)
        {
            circCollPoints = new List<Vector2>();

            foreach (States.StateInput input in CheckInput())
            {
                SendStateInput(input);
            }
            state.Update(this, gameTime);
            equip.Update(this, gameTime);
            if (effectState != null)
                effectState.Update(this, gameTime);
            position += velocity;
            HandleCollisions();
            Animate(gameTime);
        }
        public void NewAnimation(States.AnimState animState)
        {
            animStart = animStarts[(int)state.animState];
            animEnd = animEnds[(int)state.animState];
            animCurrent = animStart;
        }
        public void SendStateInput(States.StateInput input)
        {
            States.State newState = state.HandleInput(this, input);
            if (state.GetType() != newState.GetType())
            {
                state = newState;
                NewAnimation(state.animState);
            }
        }
        private List<States.StateInput> CheckInput()
        {
            List<States.StateInput> inputs = new List<States.StateInput>();
            foreach (Keys key in pressedKeys)
            {
                if (Managers.User.kState.IsKeyUp(key))
                    SendStateInput(States.StateInput.KeyUp);
            }
            pressedKeys = Managers.User.kState.GetPressedKeys();
            foreach (Keys key in pressedKeys)
            {
                switch (key)
                {
                    case Keys.Left:
                        inputs.Add(States.StateInput.LeftKeyPress);
                        break;
                    case Keys.Right:
                        inputs.Add(States.StateInput.RightKeyPress);
                        break;
                    case Keys.Up:
                        inputs.Add(States.StateInput.UpKeyPress);
                        break;
                    case Keys.Down:
                        inputs.Add(States.StateInput.DownKeyPress);
                        break;
                    case Keys.E:
                        if (Managers.User.old_kState.IsKeyDown(Keys.E))
                            break;
                        Point pos = new Point(Hitbox.Y / Tile.height, Hitbox.X / Tile.width);
                        List<GameObject> collisions = level.AllCollide(Hitbox);
                        foreach (GameObject obj in collisions)
                        {
                            Items.Item item = obj.ToItem();
                            if (item != null)
                            {
                                equip.EquipItem(this, item);
                                level.removeQueue.Enqueue(obj);
                            }
                        }
                        break;
                    case Keys.A:
                        if (Managers.User.old_kState.IsKeyDown(Keys.A))
                            break;
                        SendStateInput(States.StateInput.AttackKeyPress);
                        break;
                    default:
                        break;
                }
            }
            return inputs;
        }
        public void MoveX(int direction)
        {
            velocity.X += direction * moveAcceleration;
            if (Math.Abs(velocity.X) < Math.Abs(direction) * moveAcceleration)
            {
                velocity.X = 0;
            }
        }
        public void MoveY(int direction)
        {
            velocity.Y += direction * moveAcceleration;
        }
        public void ApplyGravity(float multiplier)
        {
            velocity.Y += multiplier * gravityAcceleration;
        }
        public void ApplyFriction(float multiplier)
        {
            if (velocity.X != 0)
                MoveX((int)(-1 * multiplier * (velocity.X / (Math.Abs(velocity.X)))));
        }
        public void ApplyFrictionY(float multiplier)
        {
            if (velocity.Y != 0)
                MoveY((int)(-1 * multiplier * (velocity.Y / (Math.Abs(velocity.Y)))));
        }
        public void ClampX(float max, int direction)
        {
            if (Math.Abs(velocity.X) > max)
                velocity.X = max * direction;
        }
        public void ClampY(float max, int direction)
        {
            if (Math.Abs(velocity.Y) > max)
                velocity.Y = max * direction;
        }
        private void Animate(GameTime gameTime)
        {
            frameTime += gameTime.ElapsedGameTime.Milliseconds;
            if (frameTime >= texture.mpf)
            {
                if (animCurrent != animEnd)
                {
                    if (animCurrent.X < texture.cols - 1)
                    {
                        animCurrent.X++;
                    }
                    else
                    {
                        animCurrent.X = 0;
                        if (animCurrent.Y < texture.rows)
                        {
                            animCurrent.Y++;
                        }
                        else
                            throw new NotSupportedException("Animation overflow");
                    }
                }
                else if (animCurrent == animEnd)
                {
                    state = state.HandleInput(this, States.StateInput.AnimEnd);
                    animStart = animStarts[(int)state.animState];
                    animEnd = animEnds[(int)state.animState];
                    animCurrent = animStart;
                }
                frameTime = 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Rectangle startTest = new Rectangle((int)(Hitbox_ex.X - Managers.Camera.Offset.X), (int)(Hitbox_ex.Y - Managers.Camera.Offset.Y), (int)Hitbox_ex.Width, (int)Hitbox_ex.Height);
            //spriteBatch.Draw(Managers.AssetManager.GetTextureAsset("Level0").sprite, startTest, Color.Black);
            //Rectangle startTest = new Rectangle((int)(Hitbox.X - Managers.Camera.Offset.X), (int)(Hitbox.Y - Managers.Camera.Offset.Y), (int)Hitbox.Width, (int)Hitbox.Height);
            //spriteBatch.Draw(Managers.AssetManager.GetTextureAsset("Level0").sprite, startTest, Color.Black);
            Vector2 drawPos = new Vector2(position.X - (texture.Width / 4), position.Y - (texture.Height / 2));
            Rectangle destRect = new Rectangle(animCurrent.X * texture.Width, animCurrent.Y * texture.Height, texture.Width, texture.Height);
            Rectangle drawRect = new Rectangle((int)(drawPos.X - Managers.Camera.Offset.X), (int)(drawPos.Y - Managers.Camera.Offset.Y), texture.Width / 2, texture.Height / 2);
            if (effectState != null)
            {
                effectState.DrawPlayer(this, spriteBatch, drawRect, destRect, flip);
            }
            else
                spriteBatch.Draw(texture.sprite, drawRect, destRect, Color.White, 0.0f, new Vector2(0, 0), flip, 1.0f);
            string text = velocity.X.ToString();
            //spriteBatch.DrawString(Managers.Executive.level.Font, velocity.X.ToString(), position, Color.White);
        }
        public void DrawHud(SpriteBatch spriteBatch)
        {
            int i = 0;
            for (int x = 0; x < hearts; x++)
            {
                Vector2 start = new Vector2(0 + ((Tile.width / 2) * x) + (x * Tile.width / 2), Managers.ScreenManager.virtualScreen.Y - (Tile.height));
                Rectangle heartRect = new Rectangle((int)start.X, (int)start.Y, Tile.width, Tile.height);
                spriteBatch.Draw(heartAsset.sprite, heartRect, Color.White);
            }
            foreach (Items.Item item in equip.equipped)
            {
                if (item.display)
                {
                    item.Draw(spriteBatch, new Vector2(Tile.width * i, 0));
                    i++;
                }
            }
        }
        public States.State TakeHit(int damage)
        {
            if (effectState == null || effectState.GetType() != typeof(States.Immune))
                if (hearts > 1)
                {
                    hearts -= damage;

                    effectState = new States.Immune();

                }
                else
                {
                    hearts = 0;
                    return new States.Dead();
                }

            return state;
        }
        private void HandleCollisions()
        {
            Rectangle bounds = Hitbox;
            startBound = Managers.Extensions.Position_TopLeft(position);
            endBound = Managers.Extensions.Position_BottomRight(position);
            startBound = new Vector2(startBound.X / Tile.height, startBound.Y / Tile.width);
            endBound = new Vector2(endBound.X / Tile.height, endBound.Y / Tile.width);
            startBound.X -= 1;
            startBound.Y -= 1;
            endBound.X += 1;
            endBound.Y += 1;
            onLadder = false;
            CheckColls(bounds);
            if (onLadder)
                SendStateInput(States.StateInput.LadderCollision);
            else if (state.GetType() == typeof(States.Climbing))
                SendStateInput(States.StateInput.Null);
            circCollPoints.Add
                (new Vector2(Hitbox.X, Hitbox.Y));
            circCollPoints.Add
                (new Vector2(Hitbox.X + Hitbox.Width, Hitbox.Y));
            circCollPoints.Add
                (new Vector2(Hitbox.X, Hitbox.Y + Hitbox.Height));
            circCollPoints.Add
                (new Vector2(Hitbox.X + Hitbox.Width, Hitbox.Y + Hitbox.Height));
            circCollPoints.Add
                (new Vector2(Hitbox.X, Hitbox.Y + (Hitbox.Height / 2)));
            circCollPoints.Add
                (new Vector2(Hitbox.X + Hitbox.Width, Hitbox.Y + (Hitbox.Height / 2)));
        }
        public void CheckColls(Rectangle bounds)
        {
            for (int x = (int)startBound.X; x < endBound.X; x++)
            {
                for (int y = (int)startBound.Y; y < endBound.Y; y++)
                {
                    TileType collision = Managers.Executive.level.CheckCollision(x, y);
                    collCheck = false;
                    if (collision != TileType.Passable)
                    {
                        Rectangle oldBounds = new Rectangle(bounds.X - (int)velocity.X, bounds.Y, bounds.Width, bounds.Height);
                        Rectangle tile = Managers.Executive.level.IndexToRect(x, y);
                        //Vector2 oldDepth = Managers.Extensions.GetRampIntersectionDepth(oldBounds, tile, Level.RampFunction);
                        Vector2 depth = Managers.Extensions.GetIntersectionDepth(bounds, tile);
                        if (collision != TileType.Polynomial)
                        {
                            if (depth != Vector2.Zero)
                            {
                                if (collision == TileType.Ladder)
                                    onLadder = true;
                                if (collision == TileType.Victory)
                                {
                                    if (Managers.User.kState.IsKeyDown(Keys.E) && Managers.User.old_kState.IsKeyUp(Keys.E))
                                        SendStateInput(States.StateInput.Victory);
                                }
                                Vector2 absDepth = new Vector2(Math.Abs(depth.X), Math.Abs(depth.Y));
                                if (collision == TileType.BPlatform)
                                {
                                    Rectangle newTile = new Rectangle(tile.X, tile.Y + (int)(Tile.height * 0.75), tile.Width, (int)(Tile.height * 0.25));
                                    depth = Managers.Extensions.GetIntersectionDepth(bounds, newTile);
                                    if (previousBottom < tile.Bottom && velocity.Y > 0 && depth.Y != 0)
                                    {
                                        collCheck = true;
                                        if (state.GetType() == typeof(States.Falling) || state.GetType() == typeof(States.Jumping))
                                        {
                                            if (velocity.Y >= maxFallSpeed)
                                                SendStateInput(States.StateInput.Hit);
                                            SendStateInput(States.StateInput.GroundCollision);
                                        }
                                    }
                                    if (collCheck)
                                    {
                                        position = new Vector2(Position.X, Position.Y + (depth.Y));
                                        bounds = Hitbox;
                                        SendStateInput(States.StateInput.GroundCollision);

                                    }
                                }
                                else if ((absDepth.Y < absDepth.X || collision == TileType.Platform) && collision != TileType.Ladder)
                                {
                                    if (previousBottom <= tile.Top)
                                    {
                                        collCheck = true;
                                        if (state.GetType() == typeof(States.Falling) || state.GetType() == typeof(States.Jumping))
                                        {
                                            if (velocity.Y >= maxFallSpeed)
                                                SendStateInput(States.StateInput.Hit);
                                            if (tile.Bottom > position.Y)
                                                SendStateInput(States.StateInput.GroundCollision);
                                        }
                                    }

                                    if ((collision == TileType.Impassable || collCheck))
                                    {
                                        position = new Vector2(Position.X, Position.Y + depth.Y);
                                        bounds = Hitbox;
                                        if(tile.Bottom > position.Y)
                                            SendStateInput(States.StateInput.GroundCollision);

                                    }
                                }

                                else if (collision == TileType.Impassable)
                                {
                                    position = new Vector2(Position.X + depth.X, Position.Y);
                                    if (tile.Top < Hitbox.Y)
                                        SendStateInput(States.StateInput.WallCollision);
                                    bounds = Hitbox;
                                }
                            }
                        }
                    }
                }
            }
            previousBottom = bounds.Bottom;
        }
        public void Jump(float mag)
        {
            velocity.Y = -jumpAcceleration * mag;
        }
        //Keys[] pressedKeys;
        //Torch torch;
        //List<GameObject> holding;
        //public enum EffectState
        //{
        //    None = 0,
        //    Immune = 1,
        //}
        //public enum PlayerState
        //{
        //    Idle = 0,
        //    Walking = 1,
        //    Running = 2,
        //    Jumping = 3,
        //    Landing = 4,
        //    Falling = 5,
        //    Dead = 6,
        //    Victory = 7,
        //    Attacking = 8,
        //}

        //private List<Point> animStarts;
        //private List<Point> animEnds;
        //private Point animStart;
        //private Point animEnd;
        //private Point animCurrent;
        //private Assets.Sound playing;
        //private float frameTime;

        //public enum Sounds
        //{
        //    Run = 0,
        //    Jump = 1,
        //}
        //public Rectangle[] bounds;
        //public Level Level
        //{
        //    get { return level; }
        //}
        //Level level;
        //public float MoveAcceleration
        //{
        //    get { return moveAcceleration; }
        //}
        //private float moveAcceleration;
        //private float gravityAcceleration;
        //private float jumpAcceleration;
        //private float cutJumpSpeedLimit;
        //public Vector2 Velocity
        //{
        //    get { return velocity; }
        //}
        //private Vector2 velocity;
        //public Vector2 Position
        //{
        //    get { return position; }
        //}
        //private Vector2 position;
        //private Vector2 previousPosition;
        //private float maxSpeed;
        //private float maxFallSpeed;
        //private float movementX;
        //public int hearts;
        //private const int STARTHEARTS = 3;
        //private Assets.Texture heartAsset;
        //public Assets.Texture texture;
        //private float groundDrag;
        //private float airDrag;
        //private bool collCheck;
        //public bool onGround;
        //private float previousBottom;

        //public float immuneTimer;
        //public const float IMMUNE_CLOCK = 2000f;

        //private bool blink;
        //public float blinkTimer;
        //public const float BLINK_CLOCK = 100f;
        //List<Assets.Sound> sounds;
        //Vector2 endBound;
        //Vector2 startBound;

        //public PlayerState OldState
        //{
        //    get { return oldState; }
        //}
        //private PlayerState oldState;

        //public PlayerState State
        //{
        //    get { return state; }
        //}
        //private PlayerState state;

        //public EffectState EffState
        //{
        //    get { return effState; }
        //}
        //private EffectState effState;

        //public Rectangle Hitbox
        //{
        //    get
        //    {
        //        int thinX = 30;
        //        int thinY = 5;
        //        return new Rectangle((int)position.X - (texture.Width / 2) + (thinX / 2), (int)position.Y - (texture.Height) + thinY, texture.Width - thinX, texture.Height - thinY);
        //    }
        //}
        //public Rectangle Hitbox_ex
        //{
        //    get
        //    {
        //        int thickX = 10;
        //        int thinY = 5;
        //        return new Rectangle((int)position.X - (texture.Width / 2) - (thickX / 2), (int)position.Y - (texture.Height) + thinY, texture.Width + thickX, texture.Height - thinY);
        //    }
        //}
        //public Player(Level _level, Vector2 _position)
        //{

        //    holding = new List<GameObject>();
        //    hearts = STARTHEARTS;
        //    immuneTimer = 0;
        //    blinkTimer = 0;
        //    blink = false;
        //    effState = EffectState.None;
        //    heartAsset = Managers.AssetManager.GetTextureAsset("Heart");
        //    name = "Player";
        //    level = _level;
        //    position = _position;
        //    velocity = new Vector2();
        //    maxSpeed = 6f;
        //    maxFallSpeed = 20.0f;
        //    moveAcceleration = 2f;
        //    gravityAcceleration = 0.3f;
        //    jumpAcceleration = 8.0f;
        //    cutJumpSpeedLimit = -1 * gravityAcceleration;
        //    groundDrag = 1.5f;
        //    airDrag = 1.3f;
        //    sounds = new List<Assets.Sound>();
        //    texture = Managers.AssetManager.GetTextureAsset(name);
        //    animStarts = Managers.AssetManager.GetAnimIndexes(name)[0];
        //    animEnds = Managers.AssetManager.GetAnimIndexes(name)[1];
        //    sounds.Add(Managers.AssetManager.GetSoundAsset("Running"));
        //    playing = null;
        //    collCheck = false;
        //    flip = SpriteEffects.None;
        //    ChangeState(PlayerState.Idle);
        //}
        //public void ChangeState(PlayerState _state)
        //{
        //    if (_state == PlayerState.Dead && hearts > 0)
        //    {
        //        if (effState != EffectState.Immune)
        //        {
        //            hearts--;
        //            Jump(1f);
        //        }
        //        if (hearts != 0)
        //        {
        //            effState = EffectState.Immune;
        //            return;
        //        }
        //    }
        //    if (state == PlayerState.Running)
        //        oldState = PlayerState.Walking;
        //    else
        //        oldState = state;
        //    if (state == _state)
        //        return;
        //    //if (playing != null)
        //    //playing.StopSound();
        //    state = _state;
        //    animStart = animStarts[(int)state];
        //    animEnd = animEnds[(int)state];
        //    animCurrent = animStart;
        //    switch (state)
        //    {
        //        case PlayerState.Idle:
        //            onGround = true;
        //            break;
        //        case PlayerState.Running:
        //            onGround = true;
        //            break;
        //        case PlayerState.Walking:
        //            onGround = true;
        //            break;
        //        case PlayerState.Jumping:
        //            onGround = false;
        //            break;
        //        case PlayerState.Falling:
        //            onGround = false;
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //public void Update(GameTime gameTime)
        //{
        //    if (state != PlayerState.Dead && state != PlayerState.Victory)
        //        CheckInput(gameTime);
        //    CheckState(gameTime);
        //    Animate(gameTime);
        //}
        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    Vector2 drawPos = new Vector2(position.X - (texture.Width / 2), position.Y - texture.Height);
        //    Rectangle destRect = new Rectangle(animCurrent.X * texture.Width, animCurrent.Y * texture.Height, texture.Width, texture.Height);
        //    Rectangle drawRect = new Rectangle((int)(drawPos.X - Managers.Camera.Offset.X), (int)(drawPos.Y - Managers.Camera.Offset.Y), texture.Width, texture.Height);
        //    //Rectangle startTest = new Rectangle((int)(Hitbox_ex.X - Managers.Camera.Offset.X), (int)(Hitbox_ex.Y - Managers.Camera.Offset.Y), (int)Hitbox_ex.Width, (int)Hitbox_ex.Height);
        //    //spriteBatch.Draw(Managers.AssetManager.GetTextureAsset("Level0").sprite, startTest, Color.Black);
        //    //Rectangle startTest = new Rectangle((int)(Hitbox.X - Managers.Camera.Offset.X), (int)(Hitbox.Y - Managers.Camera.Offset.Y), (int)Hitbox.Width, (int)Hitbox.Height);
        //    //spriteBatch.Draw(Managers.AssetManager.GetTextureAsset("Level0").sprite, startTest, Color.Black);
        //    if (effState == EffectState.Immune)
        //    {
        //        switch (blink)
        //        {
        //            case false:
        //                spriteBatch.Draw(texture.sprite, drawRect, destRect, Color.White * 1f, 0.0f, new Vector2(0, 0), flip, 1.0f);
        //                break;
        //            case true:

        //                spriteBatch.Draw(texture.sprite, drawRect, destRect, Color.White * 0.5f, 0.0f, new Vector2(0, 0), flip, 1.0f);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    else
        //        spriteBatch.Draw(texture.sprite, drawRect, destRect, Color.White, 0.0f, new Vector2(0, 0), flip, 1.0f);
        //    //Vector2 endTest = new Vector2((endBound.X * Tile.width) - Managers.Camera.Offset.X, (endBound.Y * Tile.height) - Managers.Camera.Offset.Y);
        //    //spriteBatch.Draw(Managers.AssetManager.GetTextureAsset("Platform").sprite, startTest, Color.Black);
        //    //spriteBatch.Draw(Managers.AssetManager.GetTextureAsset("Platform").sprite, endTest, Color.Black);
        //}
        //public void DrawHud(SpriteBatch spriteBatch)
        //{
        //    int i = 0;
        //    for (int x = 0; x < hearts; x++)
        //    {
        //        Vector2 start = new Vector2(0 + (heartAsset.sprite.Width * (x + 1)) + (10 * x), Managers.ScreenManager.virtualScreen.Y - (heartAsset.sprite.Height * 2));
        //        spriteBatch.Draw(heartAsset.sprite, start, Color.White);
        //    }
        //    foreach (GameObject obj in holding)
        //    {
        //        obj.Draw(spriteBatch, new Vector2(Tile.width * i, 0));
        //        i++;
        //    }
        //}
        //private void Jump(float mag)
        //{

        //    onGround = false;
        //    velocity.Y = -jumpAcceleration * mag;
        //    ChangeState(PlayerState.Jumping);

        //}
        //private void CheckInput(GameTime gameTime)
        //{
        //    GameObject obj;
        //    pressedKeys = Managers.User.kState.GetPressedKeys();
        //    if (velocity == Vector2.Zero && (state == PlayerState.Running || state == PlayerState.Walking))
        //        ChangeState(PlayerState.Idle);
        //    if (pressedKeys.Length == 0 && state != PlayerState.Jumping && state != PlayerState.Dead)
        //    {
        //        movementX = 0.0f;
        //    }
        //    else
        //    {
        //        foreach (Keys key in pressedKeys)
        //        {
        //            switch (key)
        //            {
        //                case Keys.Left:
        //                    movementX = -1.0f;
        //                    break;
        //                case Keys.Right:
        //                    movementX = 1.0f;
        //                    break;
        //                case Keys.Up:
        //                    if (state != PlayerState.Jumping && state != PlayerState.Falling && onGround)
        //                    {
        //                        Jump(1f);
        //                    }
        //                    break;
        //                case Keys.E:
        //                    if (Managers.User.old_kState.IsKeyDown(Keys.E))
        //                        break;
        //                    Point pos = new Point(Hitbox.Y / Tile.height, Hitbox.X / Tile.width);

        //                    #region torch check
        //                    if (torch == null)
        //                    {
        //                        obj = Managers.Executive.level.SingleTypeCollide(typeof(Torch), Hitbox_ex);
        //                        torch = obj.GetType() == typeof(Torch) ? (Torch)obj : null;
        //                        if (torch != null)
        //                        {
        //                            torch.display = false;
        //                            holding.Add(torch);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        torch.display = true;
        //                        holding.Remove(torch);
        //                        torch = null;
        //                        break;
        //                    }
        //                    #endregion

        //                    break;
        //                case Keys.R:
        //                    obj = Managers.Executive.level.SingleTypeCollide(typeof(Sign), Hitbox);
        //                    Sign sign = obj.GetType() == typeof(Sign) ? (Sign)obj : null;
        //                    break;
        //                case Keys.A:
        //                    if (Managers.User.old_kState.IsKeyDown(Keys.A) || state == PlayerState.Attacking)
        //                        break;
        //                    ChangeState(PlayerState.Attacking);
        //                    AttackType(typeof(TileObject));
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }
        //    if (torch != null)
        //    {
        //        torch.light.position = new Point(Hitbox.Y / Tile.height, Hitbox.X / Tile.width);
        //    }
        //    ApplyPhysics(gameTime);
        //}
        //private void AttackType(Type type)
        //{
        //    var attackable = Managers.Executive.level.SingleTypeCollide(type, Hitbox_ex);
        //    if (attackable == null)
        //        return;
        //    attackable.Action();
        //}
        //private void ApplyPhysics(GameTime gameTime)
        //{
        //    velocity.X += movementX * moveAcceleration;
        //    velocity.Y += gravityAcceleration;
        //    if (state == PlayerState.Jumping && Managers.User.kState.IsKeyUp(Keys.Up) && velocity.Y < cutJumpSpeedLimit)
        //    {
        //        velocity.Y += gravityAcceleration;
        //    }

        //    if (velocity.Y > maxFallSpeed)
        //        velocity.Y = maxFallSpeed;

        //    if (state == PlayerState.Idle || state == PlayerState.Running || state == PlayerState.Walking || state == PlayerState.Landing)
        //    {
        //        if (Math.Abs(velocity.X) < groundDrag)
        //            velocity.X = 0;
        //        velocity.X += velocity.X == 0 ? 0 : velocity.X > 0 ? groundDrag * -1 : groundDrag;
        //    }
        //    else
        //    {
        //        if (Math.Abs(velocity.X) < airDrag)
        //            velocity.X = 0;
        //        velocity.X += velocity.X == 0 ? 0 : velocity.X > 0 ? airDrag * -1 : airDrag;
        //    }
        //    //if(state == PlayerState.Landing)
        //    //    velocity.X *= velocity.X == 0 ? 0 : velocity.X > 0 ? 0.8f: 0.8f;


        //    if (Math.Abs(velocity.X) > maxSpeed)
        //        velocity.X = maxSpeed * movementX;

        //    previousPosition = position;
        //    position += velocity;
        //    HandleCollisions();
        //    if (position.X == previousPosition.X)
        //        velocity.X = 0;
        //    if (position.Y == previousPosition.Y)
        //        velocity.Y = 0;
        //}
        //private void HandleCollisions()
        //{
        //    Rectangle bounds = Hitbox;
        //    startBound = Managers.Extensions.Position_TopLeft(position);
        //    endBound = Managers.Extensions.Position_BottomRight(position);
        //    startBound = new Vector2(startBound.X / Tile.height, startBound.Y / Tile.width);
        //    endBound = new Vector2(endBound.X / Tile.height, endBound.Y / Tile.width);
        //    startBound.X -= 1;
        //    startBound.Y -= 1;
        //    endBound.X += 1;
        //    endBound.Y += 1;
        //    CheckColls(bounds);

        //}
        //public void CheckColls(Rectangle bounds)
        //{
        //    for (int x = (int)startBound.X; x < endBound.X; x++)
        //    {
        //        for (int y = (int)startBound.Y; y < endBound.Y; y++)
        //        {
        //            TileType collision = Managers.Executive.level.CheckCollision(x, y);
        //            collCheck = false;
        //            if (collision != TileType.Passable)
        //            {
        //                Rectangle oldBounds = new Rectangle(bounds.X - (int)velocity.X, bounds.Y, bounds.Width, bounds.Height);
        //                Rectangle tile = Managers.Executive.level.IndexToRect(x, y);
        //                Vector2 oldDepth = Managers.Extensions.GetRampIntersectionDepth(oldBounds, tile, Level.RampFunction);
        //                Vector2 depth = Managers.Extensions.GetIntersectionDepth(bounds, tile);
        //                if (collision != TileType.Polynomial)
        //                {
        //                    if (depth != Vector2.Zero)
        //                    {
        //                        if (collision == TileType.Victory)
        //                        {
        //                            ChangeState(PlayerState.Victory);
        //                        }
        //                        Vector2 absDepth = new Vector2(Math.Abs(depth.X), Math.Abs(depth.Y));
        //                        if (absDepth.Y < absDepth.X || collision == TileType.Platform)
        //                        {
        //                            if (previousBottom <= tile.Top)
        //                            {
        //                                collCheck = true;
        //                                if (state == PlayerState.Falling || state == PlayerState.Jumping)
        //                                {
        //                                    ChangeState(PlayerState.Landing);
        //                                    if (velocity.Y >= maxFallSpeed)
        //                                        ChangeState(PlayerState.Dead);
        //                                    velocity.Y = 0;
        //                                    movementX = 0.0f;
        //                                }
        //                            }

        //                            if (collision == TileType.Impassable || collCheck)
        //                            {
        //                                position = new Vector2(Position.X, Position.Y + depth.Y);
        //                                bounds = Hitbox;
        //                                movementX = 0.0f;
        //                                if (state != PlayerState.Falling && state != PlayerState.Jumping)
        //                                {
        //                                    velocity.Y = 0;
        //                                }

        //                            }
        //                        }
        //                        else if (collision == TileType.Impassable)
        //                        {
        //                            position = new Vector2(Position.X + depth.X, Position.Y);
        //                            bounds = Hitbox;
        //                        }
        //                    }
        //                }
        //                else if (collision == TileType.Polynomial)
        //                {
        //                    if (oldDepth.Y != 0.0f)
        //                    {
        //                        Vector2 absDepth = new Vector2(Math.Abs(depth.X), Math.Abs(depth.Y));
        //                        collCheck = true;
        //                        if (state == PlayerState.Falling)
        //                        {
        //                            ChangeState(PlayerState.Landing);
        //                            if (velocity.Y >= maxFallSpeed)
        //                                ChangeState(PlayerState.Dead);
        //                            velocity.Y = 0;
        //                            movementX = 0.0f;
        //                        }
        //                        if (collCheck)
        //                        {
        //                            position = new Vector2(Position.X, Position.Y - oldDepth.Y);
        //                            bounds = Hitbox;
        //                            velocity.Y = 0.0f;
        //                        }
        //                    }
        //                    else if (depth.X != 0.0f && ((bounds.Left < tile.X) || (bounds.Left > tile.Right && (int)(bounds.Left - velocity.Y) > tile.Bottom)))
        //                    {
        //                        Vector2 absDepth = new Vector2(Math.Abs(depth.X), Math.Abs(depth.Y));
        //                        if (absDepth.Y < absDepth.X || collision == TileType.Platform)
        //                        {
        //                            collCheck = true;
        //                            position = new Vector2(Position.X, Position.Y + depth.Y);
        //                            bounds = Hitbox;
        //                            velocity.Y = 0.0f;
        //                            if (state == PlayerState.Falling)
        //                            {
        //                                ChangeState(PlayerState.Landing);
        //                                if (velocity.Y >= maxFallSpeed)
        //                                    ChangeState(PlayerState.Dead);
        //                                velocity.Y = 0;
        //                                movementX = 0.0f;
        //                            }

        //                        }
        //                        else
        //                        {
        //                            position = new Vector2(Position.X + depth.X, Position.Y);
        //                            bounds = Hitbox;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    previousBottom = bounds.Bottom;
        //}
        //public void CheckRectangular(TileType collision, Vector2 depth, Rectangle bounds, int x, int y)
        //{

        //}
        //private void CheckState(GameTime gameTime)
        //{
        //    if (state == PlayerState.Dead)
        //        return;
        //    switch (effState)d
        //    {
        //        case EffectState.Immune:
        //            immuneTimer += gameTime.ElapsedGameTime.Milliseconds;
        //            blinkTimer += gameTime.ElapsedGameTime.Milliseconds;
        //            if (blinkTimer >= BLINK_CLOCK)
        //            {
        //                blink = !blink;
        //                blinkTimer = 0f;
        //            }
        //            if (immuneTimer >= IMMUNE_CLOCK)
        //            {
        //                effState = EffectState.None;
        //                immuneTimer = 0f;
        //            }
        //            break;
        //        default:
        //            break;
        //    }

        //    if ((Math.Abs(movementX) == 1.0f && state == PlayerState.Idle)
        //        || (state == PlayerState.Running && Math.Abs(Velocity.X) < maxSpeed)
        //        )
        //    {
        //        ChangeState(PlayerState.Walking);
        //    }
        //    if (Velocity.Y > gravityAcceleration * 4 && state != PlayerState.Jumping && state != PlayerState.Landing && state != PlayerState.Attacking)
        //    {
        //        collCheck = false;
        //        ChangeState(PlayerState.Falling);
        //    }
        //    else if (Math.Abs(Velocity.X) == maxSpeed && state == PlayerState.Walking)
        //    {
        //        ChangeState(PlayerState.Running);
        //    }
        //    switch (state)
        //    {
        //        case PlayerState.Walking:
        //            onGround = true;
        //            playing = sounds[(int)Sounds.Run];
        //            break;
        //        case PlayerState.Running:
        //            onGround = true;
        //            playing = sounds[(int)Sounds.Run];
        //            break;
        //        case PlayerState.Landing:
        //            onGround = true;
        //            playing = null;
        //            break;
        //        case PlayerState.Idle:
        //            if (playing != null)
        //            {
        //                onGround = true;
        //                playing.DisposeSound();
        //                playing = null;
        //            }
        //            break;
        //        default:
        //            playing = null;
        //            break;
        //    }
        //    if (movementX != 0.0f)
        //    {
        //        flip = movementX > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        //    }
        //    if (playing != null)
        //    {
        //playing.PlaySound();
        //    }
        //}
        //private void Animate(GameTime gameTime)
        //{
        //    frameTime += gameTime.ElapsedGameTime.Milliseconds;
        //    if (frameTime >= texture.mpf)
        //    {
        //        if (animCurrent != animEnd)
        //        {
        //            if (animCurrent.X < texture.cols - 1)
        //            {
        //                animCurrent.X++;
        //            }
        //            else
        //            {
        //                if (state != PlayerState.Jumping)
        //                {
        //                    animCurrent.X = 0;
        //                    if (animCurrent.Y < texture.rows)
        //                    {
        //                        animCurrent.Y++;
        //                    }
        //                    else
        //                        throw new NotSupportedException("Animation overflow");
        //                }
        //            }
        //        }
        //        else if (animCurrent == animEnd)
        //        {
        //            switch (state)
        //            {
        //                case PlayerState.Jumping:
        //                    ChangeState(PlayerState.Falling);
        //                    break;
        //                case PlayerState.Landing:
        //                    onGround = true;
        //                    ChangeState(PlayerState.Idle);
        //                    break;
        //                case PlayerState.Dead:
        //                    Main.gameState = Main.GameState.Menu;
        //                    break;
        //                case PlayerState.Victory:
        //                    Level next = new Level(level.Index + 1);
        //                    next.LoadLevel();
        //                    break;
        //                case PlayerState.Attacking:
        //                    if (Velocity == Vector2.Zero)
        //                    {
        //                        ChangeState(PlayerState.Idle);
        //                        return;
        //                    }
        //                    ChangeState(oldState);
        //                    break;
        //                default:
        //                    animCurrent = animStart;
        //                    break;
        //            }

        //        }
        //        frameTime = 0;
        //    }
        //}
    }
}
