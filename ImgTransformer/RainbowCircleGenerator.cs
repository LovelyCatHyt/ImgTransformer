using System;
using System.Collections.Generic;
using ImgTransformer.Global;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImgTransformer.Geneartors
{
    class RainbowCircleGenerator : IGenerator
    {
        private float _startRadian;
        private readonly List<Type> _supportedTypes = new List<Type> { typeof(Rgba32), typeof(Rgb24) };
        
        private bool CheckPixelType(Type type) => _supportedTypes.Contains(type);

        public RainbowCircleGenerator(float startRadian = 0f)
        {
            _startRadian = startRadian;
        }

        public List<Type> SupportedTypes => new List<Type>(_supportedTypes);

        public Image<TPixel> Generate<TPixel>() where TPixel : unmanaged, IPixel<TPixel>
        {
            return Generate<TPixel>(GlobalValue.argument);
        }

        public Image<TPixel> Generate<TPixel>(ImageArgument argument) where TPixel : unmanaged, IPixel<TPixel>
        {
            // 不支持类型
            if (!CheckPixelType(typeof(TPixel))) throw new NotImplementedException();
            Image<TPixel> image = new Image<TPixel>(argument.imageSize.Width, argument.imageSize.Height);
            
            return image;
        }
    }
}
