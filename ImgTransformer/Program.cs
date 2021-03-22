using System;
using System.IO;
using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImgTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "output.png";
            var path_anti_aliasing = "output_1.png";
            Size size = new Size(256, 256);
            int radio = size.Width / 2 - 2;
            Point center = new Point(size.Width / 2, size.Height / 2);
            //             Console.WriteLine("Step: ");
            var step = 0.002f;
            var thickness = 128f;
            using (Image<Rgba32> image = new Image<Rgba32>(size.Width, size.Height))
            {
                for (float t = 0; t < Math.PI * 2; t += step)
                {
                    float r = (float)Math.Sin(t) * 128 + 128;
                    float g = (float)Math.Sin(t + 2 * Math.PI / 3) * 128 + 128;
                    float b = (float)Math.Sin(t + 4 * Math.PI / 3) * 128 + 128;
                    for (float delta = -thickness; delta <= 0; delta += 0.5f)
                    {
                        try
                        {

                            image[(int)(center.X + (radio + delta) * Math.Cos(t)),
                                    (int)(center.Y + (radio + delta) * Math.Sin(t))] =
                                new Rgba32((byte)r, (byte)g, (byte)b);
                        }
                        catch (Exception e)
                        {
                            // skip
                        }
                    }

                }
                image.Save(path);
                // 尝试瞎编一个抗锯齿
                int samplerange = 4;
                // 扫描全图
                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        var raw = image[x, y].ToVector4();
                        var temp = new Vector4();
                        // 扫描周围点
                        for (int dx = -samplerange + 1; dx < samplerange + 1; dx++)
                        {
                            if (x + dx < 0 || x + dx >= size.Width) continue;
                            for (int dy = -samplerange + 1; dy < samplerange + 1; dy++)
                            {
                                if (y + dy < 0 || y + dy >= size.Height) continue;
                                var target = image[x + dx, y + dy].ToVector4();
                                var dis = (float)Math.Abs(dx) + Math.Abs(dy);
                                if (dis > samplerange) continue;
                                // 插值
                                temp = target * (1 / (dis));
                            }

                        }
                        var tempRgba = new Rgba32();
                        tempRgba.FromVector4(Vector4.Lerp(raw, temp, 0.15f));
                        // 赋值
                        image[x, y] = tempRgba;
                    }
                }
                image.Save(path_anti_aliasing);
                var test = new GraphicsOptions();
                Console.WriteLine("Saved.");
                Image empty = new Image<Rgb24>(233,233);
            }
        }
    }
}
