﻿using HtmlAgilityPack;
using System;
using System.Linq;

namespace HapCss.PseudoClassSelectors
{
    [PseudoClassName("last-child")]
    internal class LastChildPseudoClass : PseudoClass
    {
        protected override bool CheckNode(HtmlNode node, string parameter) =>
            node.ParentNode.GetChildElements().Last() == node;
    }
}