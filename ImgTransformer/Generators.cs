using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImgTransformer.Geneartors
{
    public interface IGenerator
    {
        public List<Type> SupportedTypes { get; }
        /// <summary>
        /// 使用全局参数生成图像
        /// </summary>
        /// <returns></returns>
        public Image<TPixel> Generate<TPixel>() where TPixel : unmanaged, IPixel<TPixel>;
        /// <summary>
        /// 指定具体参数生成图像
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public Image<TPixel> Generate<TPixel>(ImageArgument argument) where TPixel : unmanaged, IPixel<TPixel>;
    }
}
