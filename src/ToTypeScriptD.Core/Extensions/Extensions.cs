﻿using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToTypeScriptD;
using ToTypeScriptD.Core.WinMD;

namespace ToTypeScriptD
{

    public static class Extensions
    {
        public static string Dup(this string value, int count)
        {
            return string.Join("", Enumerable.Range(0, count).Select(s => value));
        }

        [System.Diagnostics.DebuggerHidden]
        public static string Join(this IEnumerable<string> items, string separator = "")
        {
            if (items == null) return string.Empty;

            return string.Join(separator, items);
        }

        public static bool Matches(this string value, string pattern)
        {
            var result = System.Text.RegularExpressions.Regex.IsMatch(value, pattern ?? "");
            return result;
        }

        public static bool ShouldIgnoreType(this Mono.Cecil.TypeDefinition name)
        {
            if (!name.IsPublic)
                return true;

            return false;
        }

        // TODO: look to move this to the WinMDExtensions.cs
        public static string ToTypeScriptItemName(this Mono.Cecil.TypeReference typeReference)
        {
            // Nested classes don't report their namespace. So we have to walk up the 
            // DeclaringType tree to find the root most type to grab it's namespace.
            var parentMostType = typeReference;
            while (parentMostType.DeclaringType != null)
            {
                parentMostType = parentMostType.DeclaringType;
            }

            var mainTypeName = typeReference.FullName;

            // trim namespace off of the front.
            mainTypeName = mainTypeName.Substring(parentMostType.Namespace.Length + 1);

            // replace the nested class slash with an underscore
            mainTypeName = mainTypeName.Replace("/", "_").StripGenericTick();

            mainTypeName = mainTypeName.StripGenericTick();
            return mainTypeName;
        }

        [System.Diagnostics.DebuggerHidden]
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            var hashset = new HashSet<T>();
            if (items != null)
            {
                foreach (var item in items)
                {
                    hashset.Add(item);
                }
            }
            return hashset;
        }

        static Dictionary<string, string> _specialEnumNames = new Dictionary<string, string>
        {
            {"GB2312", "gb2312"},
            {"PC437", "pc437"},
            {"NKo", "nko"},
        };

        // Copied and modified from Json.Net
        // https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Utilities/StringUtils.cs
        public static string ToCamelCase(this string s, bool camelCaseConfig)
        {
            if (!camelCaseConfig)
                return s;

            if (_specialEnumNames.ContainsKey(s))
            {
                return _specialEnumNames[s];
            }

            if (string.IsNullOrEmpty(s))
                return s;

            if (!char.IsUpper(s[0]))
                return s;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                bool hasNext = (i + 1 < s.Length);
                if ((i == 0 || !hasNext) || char.IsUpper(s[i + 1]))
                {
                    char lowerCase;
#if !(NETFX_CORE || PORTABLE)
                    lowerCase = char.ToLower(s[i], System.Globalization.CultureInfo.InvariantCulture);
#else
                    lowerCase = char.ToLower(s[i]);
#endif

                    sb.Append(lowerCase);
                }
                else
                {
                    sb.Append(s.Substring(i));
                    break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// For iterator extension method that also includes a bool with the 'isLastItem' value.
        /// </summary>
        /// <example>
        ///     new[] { 1, 2, 3 }.For((item, i, isLastItem) => { });
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Items to iterate</param>
        /// <param name="action">generic action with T1=Item, T2=i</param>
        [System.Diagnostics.DebuggerHidden]
        public static void For<T>(this IEnumerable<T> items, Action<T, int, bool> action)
        {
            if (items != null)
            {
                var count = items.Count();
                int i = 0;
                foreach (var item in items)
                {
                    action(item, i, i == (count - 1));
                    i++;
                }
            }
        }

        [System.Diagnostics.DebuggerHidden]
        public static IEnumerable<int> Times(this int value)
        {
            return Enumerable.Range(0, value);
        }

        [System.Diagnostics.DebuggerHidden]
        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    action(item);
                }
            }
        }

        [System.Diagnostics.DebuggerHidden]
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, format, args);
        }

        public static string StripGenericTick(this string value)
        {
            4.Times().Each(x =>
            {
                value = value.Replace("`" + x, "");
            });
            return value;
        }

        public static void NewLine(this System.IO.TextWriter textWriter)
        {
            textWriter.WriteLine("");
        }

        public static void AppendFormatLine(this System.Text.StringBuilder sb, string format=null, params object[] args)
        {
            if (!string.IsNullOrEmpty(format))
                sb.AppendFormat(format, args);
            sb.AppendLine();
        }

        public static string RenameInvalidNamesToSaveName(this string value)
        {
            if (value == "function")
            {
                return "function_";
            }
            if (value == "Function")
            {
                return "Function_";
            }

            if (value == "arguments")
            {
                return "arguments_";
            }

            if (value == "Arguments")
            {
                return "Arguments_";
            }
            return value;

        }
    }
}
