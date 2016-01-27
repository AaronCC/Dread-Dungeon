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

namespace BaseGen.Managers
{
    static class Extensions
    {
        public delegate int Polynomial(int x);
        public static Vector2 GetIntersectionDepth(Rectangle rectA, Rectangle rectB)
        {
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }
        public static Vector2 GetRampIntersectionDepth(Rectangle rect, Rectangle tile, Polynomial poly)
        {
            float halfWidth = rect.Width / 2.0f;
            float halfHeight = rect.Height / 2.0f;
            float halfTileWidth = Elements.Tile.width/2;
            float halfTileHeight = Elements.Tile.height/2;
            Vector2 rectCenter = new Vector2(rect.Left + halfWidth, rect.Top + halfHeight);
            Vector2 tileCenter = new Vector2(tile.X + halfTileWidth,tile.Y + halfTileHeight);
            float distanceX = rectCenter.X - tileCenter.X;
            float distanceY = (tile.Bottom - rectCenter.Y) - poly((int)rectCenter.X - tile.X);
            float minDistanceX = halfWidth + halfTileWidth;
            float minDistanceY = halfHeight;
            float depthX = 0, depthY = 0;
            if (Math.Abs(distanceX) < minDistanceX)
                depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            if (Math.Abs(distanceY) < minDistanceY)
                depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }
        public static Vector2 Rect_BottomCenter(Rectangle rect)
        {
            return new Vector2(rect.X + (rect.Width / 2.0f), rect.Y + rect.Height);
        }
        public static Vector2 Position_TopLeft(Vector2 vect)
        {
            return new Vector2((int)(vect.X / Elements.Tile.width) * Elements.Tile.width,
                ((int)(vect.Y - (int)(Elements.Tile.height / 2)) / Elements.Tile.height) * Elements.Tile.height);
        }
        public static Vector2 Erase_Position_TopLeft(Vector2 vect)
        {
            return new Vector2((int)(vect.X / Elements.Tile.width) * Elements.Tile.width,
                ((int)(vect.Y - (int)(Elements.Tile.height / 2) + 32) / Elements.Tile.height) * Elements.Tile.height);
        }
        public static Vector2 Position_BottomRight(Vector2 vect)
        {
            return new Vector2(((int)(vect.X + Elements.Tile.width) / Elements.Tile.width) * Elements.Tile.width,
                (((int)(vect.Y - (Elements.Tile.height / 2)) + Elements.Tile.height) / Elements.Tile.height) * Elements.Tile.height);
        }
        public static Point Animate(Point animEnd, Point animStart, Point animCurrent, Assets.Texture texture)
        {
            return animCurrent;
        }
        #region physics

        public static Vector2 ClampVector(Vector2 vect, float max)
        {
            if (Math.Sqrt(Math.Pow(vect.X, 2) + Math.Pow(vect.Y, 2)) > max)
            {
                vect.Normalize();
                vect *= max;
            }
            return vect;
        }

        #endregion
    }
}
