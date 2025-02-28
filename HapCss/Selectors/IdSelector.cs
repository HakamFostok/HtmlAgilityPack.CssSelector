﻿using HtmlAgilityPack;

namespace HapCss.Selectors;

internal class IdSelector : CssSelector
{
    public override string Token => "#";

    protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> currentNodes)
    {
        foreach (HtmlNode node in currentNodes)
        {
            if (node.Id.Equals(Selector, StringComparison.InvariantCultureIgnoreCase))
                return new[] { node };
        }

        return new HtmlNode[0];
    }
}
