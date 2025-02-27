using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forgium
{
    public interface IRenderingSurface
    {
        public void DrawRectangle(Dictionary<string, object[]> CSSClass,int X,int Y);
        public void DrawText(Dictionary<string, object[]> CSSClass,int X,int Y,string text);
        public void Clear(Dictionary<string, object[]> CSSClass);

    }
}
