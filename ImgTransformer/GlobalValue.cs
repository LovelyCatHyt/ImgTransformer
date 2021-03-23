using System;
using Newtonsoft.Json;
using SixLabors.ImageSharp.PixelFormats;

namespace ImgTransformer.Global
{
    public static class GlobalValue
    {
        public static ImageArgument argument;
        public static string inPath;
        public static string outPath;

        private struct Config
        {
            public ImageArgument argument;
            public string argument_type;
            public string inPath;
            public string outPath;
        }

        public static void ImportConfig(string cfg_json)
        {
            var cfg = JsonConvert.DeserializeObject<Config>(cfg_json);
            argument = cfg.argument;
            inPath = cfg.inPath;
            outPath = cfg.outPath;

            argument.PixelType =
                Type.GetType($"SixLabors.ImageSharp.PixelFormats.{cfg.argument_type}") ?? typeof(Rgb24);
        }

        public static string ExportConfig(bool indent = false)
        {
            var cfg = new Config
            {
                argument = argument, argument_type = argument.PixelType.Name, inPath = inPath,
                outPath = outPath
            };
            return JsonConvert.SerializeObject(cfg, indent ? Formatting.Indented : Formatting.None);
        }
    }
}
