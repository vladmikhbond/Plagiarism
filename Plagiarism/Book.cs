﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism
{
    public class Book
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Digest { get; set; }

        public Book(string name, string source)
        {
            Name = name;
            Source = source;
            Digest = CreateDigest(source);
        }

        public string CreateDigest(string s)
        {
            var abc = "цкнгшщзхфвпрлджчсмтбqwrtpsdfghklzxcvbnm";

            s = s.ToLower();
            List<char> digest = new List<char>();
            map = new List<int>();

            for (int i = 0; i < Source.Length; i++)
            {
                var c = s[i];
                if (abc.Contains(c))
                {
                    // change russian 'x','c','p' to english
                    if (c == 'х')
                        c = 'x';
                    else if (c == 'с')
                        c = 'c';
                    else if (c == 'р')
                        c = 'p';

                    digest.Add(c);
                    map.Add(i);
                }
            }
            return new string(digest.ToArray());
        }


        public List<int> map;

        public string Slice(int i, int len)
        {
            int i1 = map[i];
            int i2 = map[i + len];
            i = i1;
            len = i2 - i1;
            return Source.Substring(i, len);
        }
    }
}
