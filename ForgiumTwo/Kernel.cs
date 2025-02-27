using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Cosmos.System.Graphics;
using Sys = Cosmos.System;

namespace ForgiumTwo
{
    public class Kernel : Sys.Kernel
    {
        Canvas canv;
        protected override void BeforeRun()
        {
            canv = FullScreenCanvas.GetFullScreenCanvas(new Mode(1280,720,ColorDepth.ColorDepth32));
            canv.Clear(Color.Blue);
            BitmapRenderingSurface surface = new(800,600,ColorDepth.ColorDepth32);
            Forgium.Forgium forgium = new(surface);
            forgium.Render(@"<html>
                <head>
                    <title>Sample Page</title>
                </head>
                <body>
                    <h1>Hello, World!</h1>
                    <p>This is a sample paragraph.</p>
                    <a href='https://example.com'>Example Link</a>
                </body>
            </html>");
            canv.DrawImage(surface,0,0);
            canv.Display();
        }

        protected override void Run()
        {
        }
    }
}
