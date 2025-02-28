﻿using HtmlAgilityPack;

namespace System;

public static partial class HapCssExtensionMethods
{
    public static IEnumerable<HtmlNode> GetChildElements(this HtmlNode node) =>
        node.ChildNodes.Where(i => i.NodeType == HtmlNodeType.Element);

    public static IList<string> GetClassList(this HtmlNode node)
    {
        HtmlAttribute attr = node.Attributes["class"];
        if (attr == null)
            return new string[0];
        return attr.Value.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public static int GetIndexOnParent(this HtmlNode node)
    {
        int idx = 0;
        foreach (HtmlNode n in node.ParentNode.GetChildElements())
        {
            if (n == node)
                return idx;
            idx++;
        }

        throw new InvalidOperationException("Node not found in its parent!");
    }

    public static HtmlNode NextSiblingElement(this HtmlNode node)
    {
        HtmlNode rt = node.NextSibling;

        while (rt != null && rt.NodeType != HtmlNodeType.Element)
            rt = rt.NextSibling;

        return rt;
    }

    public static HtmlNode PreviousSiblingElement(this HtmlNode node)
    {
        HtmlNode rt = node.PreviousSibling;

        while (rt != null && rt.NodeType != HtmlNodeType.Element)
            rt = rt.PreviousSibling;

        return rt;
    }
}
