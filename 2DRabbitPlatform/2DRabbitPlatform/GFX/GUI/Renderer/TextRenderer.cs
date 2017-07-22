using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX.GUI.Renderer
{
    public class TextRenderer
    {
        public static string render(SpriteFont font, Rectangle destination, string text, float scale, Color color, float yoffset = 0)
        {
            float line = font.LineSpacing * scale;
            string[] words = text.Split(' ');
            string old = "";
            string current = "";
            string drawString = "";
            Vector2 currentSize;

            for (int i = 0; i < words.Length; i++)
            {
                old = current;

                current += words[i] + " ";

                if (font.MeasureString(words[i]).X * scale > destination.Width)
                {
                    drawString += longStringSpace(words[i], destination, font, scale) + " ";
                    current = "";
                    continue;
                }

                currentSize = font.MeasureString(current);
                currentSize = currentSize * scale;

                if (currentSize.X > destination.Width)
                {
                    current = old;
                    current += "\r\n";
                    drawString += current;
                    current = "";
                    i--;
                }
                else
                    continue;
            }

            drawString += current;

            //Viewport vtext = new Viewport(destination);
            //Viewport vold = sb.GraphicsDevice.Viewport;

            //sb.GraphicsDevice.Viewport = vtext;
           // sb.DrawString(font, drawString, new Vector2(destination.X, destination.Y - yoffset), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
            //sb.GraphicsDevice.Viewport = vold;

            return drawString;
        }

        private static string longStringSpace(string text, Rectangle destination, SpriteFont font, float scale)
        {
            string word = text, rest = "";

            while (font.MeasureString(word).X * scale > destination.Width)
            {
                word = word.Substring(0, word.Length - 1);
                rest = text.Substring(word.Length);
            }

            if (font.MeasureString(rest).X * scale > destination.Width)
                return word + "\r\n" + longStringSpace(rest, destination, font, scale);

            return word + "\r\n" + rest;
        }

        public static string parseText(string text, SpriteFont font, Rectangle bounds, float scale)
        {
            string line = "";
            string returnString = "";
            string[] wordArray = text.Split(' ');

            foreach (string word in wordArray)
            {
                if (font.MeasureString(line + word).Length() * scale > bounds.Width)
                {
                    returnString = returnString + line + '\n';

                    line = "";
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }

        public static bool isOverHeight(string text, SpriteFont font, Rectangle bounds, float scale)
        {
            return font.MeasureString(text).Y * scale > bounds.Height;
        }
    }
}
