using HtmlAgilityPack;

namespace Forgium
{
    public class Forgium
    {
        IRenderingSurface renderingSurface;
        public Dictionary<string, Dictionary<string, object[]>> CSSRuleSet;
        Dictionary<string, ITag> Tags;
        static string BaseCssRules = @".default {
color: #000000;
}";

        public Forgium(IRenderingSurface surface)
        {
            CSSRuleSet = CSS.Parse(BaseCssRules);
            Tags = new();
            renderingSurface = surface;
        }

        public void RegisterTag(string name,ITag tag) => Tags[name] = tag;

        public void Render(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            Iterate(doc.DocumentNode);
        }

        void Iterate(HtmlNode node)
        {
            if (node.NodeType == (HtmlNodeType.Text | HtmlNodeType.Comment)) return;

            var classes = node.GetAttributeValue("class","default").Split(" ");

            Dictionary<string, object[]> CSSRules = new();

            foreach (var item in classes)
            {
                if (!CSSRuleSet.ContainsKey(item)) continue;

                foreach (var item1 in CSSRuleSet[item])
                {
                    CSSRules[item1.Key] = item1.Value;
                }
            }

            if (Tags.ContainsKey(node.Name)) Tags[node.Name].Render(CSSRules,node,renderingSurface);

            foreach (var item in node.ChildNodes) Iterate(item);
        }
    }
}
