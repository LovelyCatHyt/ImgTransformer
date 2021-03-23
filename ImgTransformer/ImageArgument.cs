using System;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;

namespace ImgTransformer
{
    /// <summary>
    /// 本程序相关算法需要的图像参数的并集
    /// </summary>
    public struct ImageArgument
    {
        /// <summary>
        /// 图像尺寸
        /// </summary>
        public Size imageSize;

        [JsonIgnore]
        private Type _pixelType;

        /// <summary>
        /// 像素类型
        /// </summary>
        [JsonIgnore]
        public Type PixelType
        {
            get => _pixelType;
            set
            {
                // TODO
                _pixelType = value;
            }
        }
    }
}
