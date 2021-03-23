using System;
using System.Collections.Generic;
using System.Numerics;
using ImgTransformer.Global;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImgTransformer.Geneartors
{
    class RainbowCircleGenerator : IGenerator
    {
        private readonly float _startRadian;
        private readonly float _thicknessRate;
        private readonly List<Type> _supportedTypes = new List<Type> { typeof(Rgba32), typeof(Rgb24) };

        private bool CheckPixelType(Type type) => _supportedTypes.Contains(type);

        private float GetRadianOfPoint(float x, float y, float r)
        {
            var temp = Math.Asin(y / r);
            if (x < 0)
            {
                temp = Math.PI - temp;
                if (y < 0)
                {
                    temp -= 2 * Math.PI;
                }
            }

            return (float)temp;
        }

        /// <summary>
        /// Init with optional arguments
        /// </summary>
        /// <param name="startRadian"></param>
        /// <param name="thicknessRate">1.0f means full circle, 0.1f means a ring</param>
        public RainbowCircleGenerator(float startRadian = 0f, float thicknessRate = 1.0f)
        {
            _startRadian = startRadian;
            _thicknessRate = thicknessRate;
        }

        public List<Type> SupportedTypes => new List<Type>(_supportedTypes);

        public Image Generate<TPixel>() where TPixel : unmanaged, IPixel<TPixel>
        {
            return Generate<TPixel>(GlobalValue.argument);
        }

        public Image Generate<TPixel>(ImageArgument argument) where TPixel : unmanaged, IPixel<TPixel>
        {
            
            // 不支持类型
            if (!CheckPixelType(typeof(TPixel))) throw new NotImplementedException();
            bool useAlpha = typeof(TPixel) == typeof(Rgba32);
            var center = new Point(argument.imageSize.Width / 2, argument.imageSize.Height / 2);
            var radio = center.X;
            float lowThreshold = radio * (1-_thicknessRate);
            float highThreshold = radio * _thicknessRate;
            void RowModifier(Span<Vector4> row, Point p)
            {
                for (int x = 0; x < row.Length; x++)
                {
                    var centerDelta_X = x - center.X;
                    var centerDelta_Y = p.Y - center.Y;

                    var disToCenter = (float)Math.Sqrt(centerDelta_X * centerDelta_X + centerDelta_Y * centerDelta_Y);
                    if (disToCenter >= lowThreshold && disToCenter < highThreshold)
                    {
                        // some simple mathematics
                        float radian = GetRadianOfPoint(centerDelta_X, centerDelta_Y, disToCenter);

                        float r = (float)Math.Sin(radian - _startRadian) * 0.5f + 0.5f;
                        float g = (float)Math.Sin(radian - _startRadian + 2 * Math.PI / 3) * 0.5f + 0.5f;
                        float b = (float)Math.Sin(radian - _startRadian + 4 * Math.PI / 3) * 0.5f + 0.5f;
                        row[x] = new Vector4(r, g, b, 1);
                    }
                }
            }

            if (useAlpha)
            {
                Image<Rgba32> image = new Image<Rgba32>(argument.imageSize.Width, argument.imageSize.Height);
                image.Mutate(img => img.ProcessPixelRowsAsVector4(RowModifier));
                image[center.X,center.Y] = new Rgba32(255, 255, 255, 255);
                return image;
            }
            else
            {
                Image<Rgb24> image = new Image<Rgb24>(argument.imageSize.Width, argument.imageSize.Height);
                image.Mutate(img => img.ProcessPixelRowsAsVector4(RowModifier));
                image[center.X, center.Y] = new Rgb24(255, 255, 255);
                return image;
            }
        }
    }
}
