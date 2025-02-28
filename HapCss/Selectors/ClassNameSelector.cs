﻿using HtmlAgilityPack;

namespace HapCss.Selectors;

internal class ClassNameSelector : CssSelector
{
    public override string Token => ".";

    protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> currentNodes)
    {
        foreach (HtmlNode node in currentNodes)
        {
            if (node.GetClassList().Any(c => c.Equals(Selector, StringComparison.InvariantCultureIgnoreCase)))
                yield return node;
        }
    }
}
