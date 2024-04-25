using System.Collections.Generic;
using UnityEngine;

namespace Helpers.TextureManagement
{
    public static class TextureUtility
    {
        ///<summary>
        ///Change color hue.
        ///</summary>
        public static Color SetPixelColorHue(Color color, float h)
        {
            Color.RGBToHSV(color, out var currentH, out var currentS, out var currentV);

            currentH = h;
            color = Color.HSVToRGB(currentH, currentS, currentV);

            return color;
        }

        ///<summary>
        ///Change color saturation.
        ///</summary>
        public static Color SetPixelColorSaturation(Color color, float s)
        {
            Color.RGBToHSV(color, out var currentH, out var currentS, out var currentV);

            currentS = s;
            color = Color.HSVToRGB(currentH, currentS, currentV);

            return color;
        }

        ///<summary>
        ///Change color value-brightness.
        ///</summary>
        public static Color SetPixelColorValue(Color color, float v)
        {
            Color.RGBToHSV(color, out var currentH, out var currentS, out var currentV);

            currentV = v;
            color = Color.HSVToRGB(currentH, currentS, currentV);

            return color;
        }

        ///<summary>
        ///Get coordinates of pixels to be painted which are not with 0 alpha value.
        ///</summary>
        public static List<Vector2> GetPixelsToBePainted(Texture texture)
        {
            List<Vector2> pixelCoordinates = new List<Vector2>();

            int width = texture.width;
            int height = texture.height;

            Texture2D texture2D = (Texture2D)texture;

            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    Color currentColor = texture2D.GetPixel(i, j);
                    if(currentColor.a == 0f)
                        continue;

                    Vector2 coordinate = new Vector2(i, j);
                    pixelCoordinates.Add(coordinate);
                }
            }

            return pixelCoordinates;
        }

        public static Texture2D CropTexture(Texture2D texture, Vector2 cropBlockSize, Vector2 startingCoordinate = default(Vector2))
        {
            Texture2D croppedTexture = new Texture2D((int)cropBlockSize.x, (int)cropBlockSize.y);
            var pixels = texture.GetPixels((int)startingCoordinate.x, (int)startingCoordinate.y, croppedTexture.width,
                croppedTexture.height);
            croppedTexture.SetPixels(pixels);
            
            croppedTexture.Apply();
            return croppedTexture;
        }
    }
}

