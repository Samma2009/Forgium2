using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Cosmos.Core;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using HtmlAgilityPack;
using IL2CPU.API.Attribs;
using Sys = Cosmos.System;

namespace ForgiumTwo
{
    public class Kernel : Sys.Kernel
    {
        [ManifestResourceStream(ResourceName = "ForgiumTwo.arial.ttf")]
        static byte[] arial;
        [ManifestResourceStream(ResourceName = "ForgiumTwo.arialbd.ttf")]
        static byte[] arialb;
        Canvas canv;
        protected override void BeforeRun()
        {
            canv = FullScreenCanvas.GetFullScreenCanvas(new Mode(1280,720,ColorDepth.ColorDepth32));
            canv.Clear(Color.Blue);
            BitmapRenderingSurface surface = new(800,600);
            surface.fonts["arialnormal"] = new(arial);
            surface.fonts["arialbold"] = new(arialb);
            Forgium.Forgium forgium = new(surface);
            var d = new HtmlDocument();
            d.LoadHtml(@"<html><head></head><body><h1 style='color: #FF0000;'>Hello, World!</h1><a>test</a><a>test2</a><a>test3</a></body></html>");
            forgium.Render(d);
            canv.DrawImage(surface.image,0,0);
            canv.Display();
        }

        protected override void Run()
        {

        }
    }
}
