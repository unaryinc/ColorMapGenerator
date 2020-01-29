using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorMapGenerator
{
    class Program
    {

        // https://stackoverflow.com/a/1626175

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public static void Work(int NewTextureCount, int NewTextureRes, int NewTextureDiv)
        {
            for(int i = 0; i < NewTextureCount; ++i)
            {
                float Hue = (360 / NewTextureCount) * i;

                List<List<Color>> Colors = new List<List<Color>>();

                using (Bitmap Bitmap = new Bitmap(NewTextureRes, NewTextureRes))
                {
                    float Delta = (NewTextureRes / NewTextureDiv * 100.0f) / NewTextureRes;

                    int ColorFillX = 0;

                    for (float Saturation = 0; Saturation < 100.0f; Saturation += Delta)
                    {
                        Colors.Add(new List<Color>());

                        int ColorFillY = 0;

                        for (float Value = 0; Value < 100.0f; Value += Delta)
                        {
                            Colors[ColorFillX].Add(new Color());

                            float RealSaturation = Saturation / 100.0f;
                            float RealValue = Value / 100.0f;

                            Colors[ColorFillX][ColorFillY] = ColorFromHSV(Hue, RealSaturation, RealValue);

                            ColorFillY++;
                        }

                        ColorFillX++;
                    }
                    
                    int ColorCountX = 0;

                    for (int ChunkX = 0; ChunkX < NewTextureRes; ChunkX += NewTextureRes / NewTextureDiv)
                    {
                        int ColorCountY = 0;

                        for (int ChunkY = 0; ChunkY < NewTextureRes; ChunkY += NewTextureRes / NewTextureDiv)
                        {
                            for(int x = ChunkX; x < ChunkX + NewTextureRes / NewTextureDiv; ++x)
                            {
                                for (int y = ChunkY; y < ChunkY + NewTextureRes / NewTextureDiv; ++y)
                                {
                                    Bitmap.SetPixel(x, y, Colors[ColorCountX][ColorCountY]);
                                }
                            }

                            ColorCountY++;
                        }

                        ColorCountX++;
                    }

                    Bitmap.Save(i + ".png");
                }
            }
        }

        static void Main(string[] args)
        {
            Work(36, 256, 16);
        }
    }
}
