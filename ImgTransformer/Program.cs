using System;
using System.IO;
using ImgTransformer.Geneartors;
using Newtonsoft.Json;
using ImgTransformer.Global;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImgTransformer.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalValue.ImportConfig(File.ReadAllText("Config.json"));
            File.WriteAllText("Config.json",GlobalValue.ExportConfig());
            IGenerator generator = new RainbowCircleGenerator();
            var img = generator.Generate<Rgba32>();
            Console.WriteLine("Generate finished.");
            img.Save(GlobalValue.outPath);
            Console.WriteLine("Saved to " + GlobalValue.outPath);
        }
    }
}
