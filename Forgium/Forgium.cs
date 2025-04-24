using System.Drawing;
using Cosmos.Core.Memory;
using Cosmos.System;
//using Forgium.Tags;
using HtmlAgilityPack;

namespace Forgium
{
    public class Forgium
    {
        IRenderingSurface renderingSurface;
        public Dictionary<string, Dictionary<string, object[]>> CSSRuleSet;
        static string BaseCssRules = @"
default {
    color: #000000;
    font-family: arial;
    font-size: 16px;
    font-weight: normal;
    margin-bottom: 0px;
    margin-top: 0px;
    margin-left: 0px;
    margin-right: 0px;
    padding-bottom: 0px;
    padding-top: 0px;
    padding-left: 0px;
    padding-right: 0px;
}

h1 {
    font-size: 40px;
    font-weight: bold;
    display: block;
    margin-bottom: 21px;
    margin-top: 21px;
    --forgium-custom-behavior: dynamic-margin;
}

body {
    background-color: #FFFFFF;
    margin-top: 8px;
    margin-left: 8px;
    margin-right: 8px;
    --forgium-custom-behavior: clear reset-cursor immulate-margin;
}

div {
    --forgium-custom-behavior: additive-css precalculate-size;
}
";

        public Point Cursor { get; set; }

        public Forgium(IRenderingSurface surface)
        {
            CSSRuleSet = CSS.Parse(BaseCssRules);
            //Tags = new();
            renderingSurface = surface;
            //RegisterTag("h1",new TextTag());

            Cursor = Point.Empty;
        }


        public void Render(HtmlDocument document)
        {
            Dictionary<string, object[]> BoilerplateCSS = new();
            foreach (var child in document.DocumentNode.ChildNodes)
                Iterate(child, BoilerplateCSS);
        }

        int textrendered = 0;
        Size PreviousSize = Size.Empty;
        Dictionary<string, object[]> PreviousCSS = new();

        void Iterate(HtmlNode node, Dictionary<string, object[]> ParentCSS)
        {
            switch (node.NodeType)
            {
                case HtmlNodeType.Element:
                    {
                        Kernel.PrintDebug("hit tag");

                        Dictionary<string, object[]> CSSRules = new();

                        void ProcessCustomBehaviour(object[] behaviors)
                        {
                            foreach (string item in behaviors)
                            {
                                switch (item)
                                {
                                    case "clear":
                                        renderingSurface.Clear(CSSRules);
                                        break;
                                    case "dynamic-margin":
                                        if (textrendered == 0)
                                        {
                                            CSSRules["margin-top"] = ParentCSS["margin-top"];
                                        }
                                        break;
                                    case "reset-cursor":
                                        Cursor = new(0, 0);
                                        break;
                                    case "immulate-margin":
                                        //Cursor = new(Cursor.X + (int)ParentCSS["margin-left"][0], Cursor.Y+ (int)ParentCSS["margin-top"][0]);
                                        CSSRules["margin-top"] = ParentCSS["margin-top"];
                                        CSSRules["margin-left"] = ParentCSS["margin-left"];
                                        CSSRules["margin-right"] = ParentCSS["margin-right"];
                                        CSSRules["margin-bottom"] = ParentCSS["margin-bottom"];
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        foreach (var item in CSSRuleSet["default"])
                        {
                            if (item.Key == "--forgium-custom-behavior")
                            {
                                ProcessCustomBehaviour(item.Value);
                                continue;
                            }
                            CSSRules[item.Key] = item.Value;
                        }

                        foreach (var item in ParentCSS)
                        {
                            if (item.Key == "--forgium-custom-behavior")
                            {
                                ProcessCustomBehaviour(item.Value);
                                continue;
                            }
                            CSSRules[item.Key] = item.Value;
                        }

                        if (CSSRuleSet.ContainsKey(node.Name))
                        {
                            foreach (var item1 in CSSRuleSet[node.Name])
                            {
                                if (item1.Key == "--forgium-custom-behavior")
                                {
                                    ProcessCustomBehaviour(item1.Value);
                                    continue;
                                }
                                CSSRules[item1.Key] = item1.Value;
                            }
                            Kernel.PrintDebug("processed tagcss");
                        }

                        Dictionary<string,string> Attributes = new();
                        foreach (var item in node.Attributes)
                        {
                            Kernel.PrintDebug(item.Name + " " + item.Value);
                            Attributes[item.Name] = item.Value;
                        }

                        if (Attributes.ContainsKey("class"))
                        {
                            var classes = Attributes["class"].Split(" ");
                            Kernel.PrintDebug("classes " + classes.Length);
                            foreach (var r in classes)
                            {
                                var item = "." + r;
                                Kernel.PrintDebug("ruleset");
                                if (!CSSRuleSet.ContainsKey(item)) continue;

                                Kernel.PrintDebug("still here");
                                foreach (var item1 in CSSRuleSet[item])
                                {
                                    if (item1.Key == "--forgium-custom-behavior")
                                    {
                                        ProcessCustomBehaviour(item1.Value);
                                        continue;
                                    }
                                    CSSRules[item1.Key] = item1.Value;
                                }
                            }
                        }

                        if (Attributes.ContainsKey("style"))
                        {
                            string Inline = "Inline {\n"+ Attributes["style"].Replace(";",";\n")+"}";
                            var attribs = CSS.Parse(Inline)["Inline"];
                            foreach (var item in attribs)
                            {
                                Kernel.PrintDebug(item.Key + " " + item.Value[0]);
                                if (item.Key == "--forgium-custom-behavior")
                                {
                                    ProcessCustomBehaviour(item.Value);
                                    continue;
                                }
                                CSSRules[item.Key] = item.Value;
                            }
                            Kernel.PrintDebug("Pased style");
                            Kernel.PrintDebug(Inline);
                        }

                        foreach (var item in node.ChildNodes) Iterate(item, CSSRules);
                    }
                    break;
                case HtmlNodeType.Text:
                    {
                        Kernel.PrintDebug("hit text");

                        if (string.IsNullOrWhiteSpace(node.InnerText) | string.IsNullOrEmpty(node.InnerText)) return;

                        var size = renderingSurface.CaculateTextSize(node.InnerText, ParentCSS);

                        if (ParentCSS.ContainsKey("display") && (string)ParentCSS["display"][0] == "block")
                        {
                            size.Width = renderingSurface.width;
                            size.Width -= (int)CSSRuleSet["body"]["margin-right"][0];
                            size.Width -= Cursor.X;
                            size.Width -= (int)ParentCSS["margin-left"][0] + (int)CSSRuleSet["body"]["margin-left"][0];
                        }

                        if (Cursor.X + size.Width + (int)ParentCSS["margin-left"][0] >= renderingSurface.width)
                        {
                            Cursor = Cursor = new(Cursor.X - PreviousSize.Width, Cursor.Y + PreviousSize.Height);
                        }

                        size.Width += (int)ParentCSS["padding-right"][0]+ (int)ParentCSS["padding-left"][0];
                        size.Height += (int)ParentCSS["padding-bottom"][0]+ (int)ParentCSS["padding-top"][0];

                        renderingSurface.DrawRectangle(ParentCSS,
                            Cursor.X + (int)ParentCSS["margin-left"][0] + (int)CSSRuleSet["body"]["margin-left"][0],
                            Cursor.Y + (int)ParentCSS["margin-top"][0] + (int)CSSRuleSet["body"]["margin-top"][0],
                            size);
                        renderingSurface.DrawText(ParentCSS,
                            Cursor.X + (int)ParentCSS["margin-left"][0] + (int)CSSRuleSet["body"]["margin-left"][0],
                            Cursor.Y + (int)ParentCSS["margin-top"][0] + (int)CSSRuleSet["body"]["margin-top"][0],
                            node.InnerText,size);

                        size.Width += (int)ParentCSS["margin-right"][0];
                        size.Height += (int)ParentCSS["margin-bottom"][0];

                        if (size.Width + (int)ParentCSS["margin-left"][0] + (int)CSSRuleSet["body"]["margin-left"][0] >= renderingSurface.width)
                            Cursor = new(Cursor.X, Cursor.Y + size.Height);
                        else
                            Cursor = new(Cursor.X + size.Width, Cursor.Y);

                        PreviousCSS = ParentCSS;
                        PreviousSize = size;

                        textrendered++;
                    }
                    break;
            }
        }
    }
}
