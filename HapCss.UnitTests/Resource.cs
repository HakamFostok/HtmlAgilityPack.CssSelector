﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HapCss.UnitTests
{
    internal class Resource
    {
        private static Dictionary<string, byte[]> s_Cache = new(StringComparer.InvariantCultureIgnoreCase);

        public static string GetString(string name)
        {
            return Encoding.UTF8.GetString(GetBytes(name));
        }

        public static byte[] GetBytes(string name)
        {
            byte[] data;

            if (s_Cache.TryGetValue(name, out data))
                return data;

            System.Reflection.Assembly asm = typeof(Resource).Assembly;
            string resourceName = Path.GetFileNameWithoutExtension(asm.GetLoadedModules()[0].Name) + "." + name;
            Stream stream = asm.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new InvalidOperationException("Stream não encontrado: " + resourceName);

            MemoryStream ms = new();
            stream.CopyTo(ms);
            ms.Position = 0;
            data = ms.ToArray();
            s_Cache.Add(name, data);
            return data;
        }
    }
}