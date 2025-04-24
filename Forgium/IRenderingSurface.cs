using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forgium
{
    public interface IRenderingSurface
    {
        public int width { get; set; }
        public int height { get; set; }
        public void DrawRectangle(Dictionary<string, object[]> CSSClass,int X,int Y, Size region);
        public void DrawText(Dictionary<string, object[]> CSSClass,int X,int Y,string text,Size region);
        public void Clear(Dictionary<string, object[]> CSSClass);
        public Size CaculateTextSize(string text, Dictionary<string, object[]> CSSClass);

    }
}
