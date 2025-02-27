using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Forgium
{
    public interface ITag
    {
        public void Render(Dictionary<string, object[]> CSSRules,HtmlNode node);
    }
}
