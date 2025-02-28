﻿using HtmlAgilityPack;

namespace HapCss;

public abstract class PseudoClass
{
    private static readonly Dictionary<string, PseudoClass> s_Classes = LoadPseudoClasses();

    public virtual IEnumerable<HtmlNode> Filter(IEnumerable<HtmlNode> nodes, string parameter) =>
        nodes.Where(i => CheckNode(i, parameter));

    protected abstract bool CheckNode(HtmlNode node, string parameter);

    public static PseudoClass GetPseudoClass(string pseudoClass)
    {
        if (!s_Classes.ContainsKey(pseudoClass))
            throw new NotSupportedException("Pseudo classe não suportada: " + pseudoClass);

        return s_Classes[pseudoClass];
    }

    private static Dictionary<string, PseudoClass> LoadPseudoClasses()
    {
        Dictionary<string, PseudoClass> rt = new(StringComparer.InvariantCultureIgnoreCase);

        // Try to be resilient against Assembly.GetType() throwing an exception:
        // - dynamic assemblies will fail
        // - I have observed the non-dynamic assembly  "DotNetOpenAuth, Version=3.4.7.11121, Culture=neutral, PublicKeyToken=2780ccd10d57b246" also fail with no obvious way of knowing it will
        //  fall ahead of time.  For this reason, I have wrapped "GetTypes" in a try/catch block so that the code can continue on somewhat gracefully
        Func<System.Reflection.Assembly, Type[]> tryGetTypes = a =>
        {
            if (!a.IsDynamic)
            {
                try
                {
                    return a.GetTypes();
                }
                catch (Exception) { }
            }
            return new Type[] { };
        };

        IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => tryGetTypes(asm).Where(i => !i.IsAbstract && i.IsSubclassOf(typeof(PseudoClass))));

        types = types.OrderBy(i => i.Assembly == typeof(PseudoClass).Assembly ? 0 : 1).ToList();

        foreach (Type type in types)
        {
            PseudoClassNameAttribute attr = type.GetCustomAttributes(typeof(PseudoClassNameAttribute), false).Cast<PseudoClassNameAttribute>().FirstOrDefault();
            rt.Add(attr.FunctionName, (PseudoClass)Activator.CreateInstance(type));
        }

        return rt;
    }
}
