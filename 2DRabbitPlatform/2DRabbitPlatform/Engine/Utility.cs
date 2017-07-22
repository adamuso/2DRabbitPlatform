using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace _2DRabbitPlatform.Engine
{
    public static class Utility
    {
        public static readonly DepthStencilState zerostate = new DepthStencilState() { DepthBufferEnable = false, StencilEnable = true, StencilFunction = CompareFunction.Always, StencilPass = StencilOperation.Replace, ReferenceStencil = 0 };
        public static readonly DepthStencilState keeplessone = new DepthStencilState() { DepthBufferEnable = false, StencilEnable = true, StencilFunction = CompareFunction.Less, StencilPass = StencilOperation.Zero, ReferenceStencil = 1 };
        public static readonly DepthStencilState keepgreaterone = new DepthStencilState() { DepthBufferEnable = false, StencilEnable = true, StencilFunction = CompareFunction.Greater, StencilPass = StencilOperation.Zero, ReferenceStencil = 1 };

        public static RenderTarget2D maskTexture(GraphicsDevice gd, Texture2D mask, Texture2D texture, Vector2 origin, bool invert = false)
        {
            SpriteBatch sb = new SpriteBatch(gd);
            RenderTarget2D target = new RenderTarget2D(gd, texture.Width, texture.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PreserveContents);
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, texture.Width, texture.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            AlphaTestEffect alphatest = new AlphaTestEffect(gd) { VertexColorEnabled = true, DiffuseColor = Color.White.ToVector3(), AlphaFunction = CompareFunction.Equal, ReferenceAlpha = 255, Projection = halfPixelOffset * projection };

            sb.GraphicsDevice.SetRenderTarget(target);
            sb.GraphicsDevice.Clear(ClearOptions.Stencil | ClearOptions.Target, Color.Transparent, 0, 2);

            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, zerostate, null);
            sb.Draw(mask, new Rectangle((int)origin.X, (int)origin.Y, texture.Width, texture.Height), Color.Transparent);
            sb.End();

            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, invert ? keeplessone : keepgreaterone, null, alphatest);
            sb.Draw(texture, new Vector2(0, 0), Color.White);
            sb.End();

            sb.GraphicsDevice.SetRenderTarget(null);
            sb.Dispose();
            return target;
        }

        public static RenderTarget2D rectangleMaskTexture(GraphicsDevice gd, Texture2D onepixelmask, Texture2D texture, Vector2 maskorigin, float xscale, float yscale, bool invert = false)
        {
            SpriteBatch sb = new SpriteBatch(gd);
            RenderTarget2D target = new RenderTarget2D(gd, texture.Width, texture.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PreserveContents);
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, texture.Width, texture.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            AlphaTestEffect alphatest = new AlphaTestEffect(gd) { VertexColorEnabled = true, DiffuseColor = Color.White.ToVector3(), AlphaFunction = CompareFunction.Equal, ReferenceAlpha = 255, Projection = halfPixelOffset * projection };


            sb.GraphicsDevice.SetRenderTarget(target);
            sb.GraphicsDevice.Clear(ClearOptions.Stencil | ClearOptions.Target, Color.Transparent, 0, 2);

            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, zerostate, null);
            sb.Draw(onepixelmask, new Rectangle((int)maskorigin.X, (int)maskorigin.Y, (int)(texture.Width * xscale), (int)(texture.Height * yscale)), Color.Transparent);
            sb.End();

            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, invert ? keeplessone : keepgreaterone, null, alphatest);
            sb.Draw(texture, new Vector2(0, 0), Color.White);
            sb.End();

            sb.GraphicsDevice.SetRenderTarget(null);
            sb.Dispose();
            return target;
        }

#if DEBUG
        public static void skipSpace(BinaryReader reader)
        {
            while(char.IsWhiteSpace((char)reader.PeekChar()))
            {
                reader.ReadChar();
            }
        }

        public static string readToBrakpoint(BinaryReader reader)
        {
            skipSpace(reader);

            char c = reader.ReadChar();
            string s = "";

            while (!char.IsWhiteSpace(c) && c != ',')
            {
                s += c;

                if (reader.BaseStream.Length != reader.BaseStream.Position)
                    c = reader.ReadChar();
                else break;
            }

            return s;
        }
#endif
    }
}
