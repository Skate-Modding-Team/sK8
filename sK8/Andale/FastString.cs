namespace sK8.Andale
{
    /**<summary>
     * 
     * 
     * </summary>
     */

    public static class FastString
    {
        private static readonly uint Seed = 79235168;

        private static readonly char[] CharSet = new char[]
        {
        '0','1','2','3','4','5','6','7','8','9',
        'A','B','C','D','E','F','G','H','I','J',
        'K','L','M','N','O','P','Q','R','S','T',
        'U','V','W','X','Y','Z','_'
        };

        private static uint Encode6(string str)
        {
            uint val = 0;
            uint seed = Seed;
            int change;

            foreach (char c in str)
            {
                if (c < 'a')
                {
                    if (c > '9')
                    {
                        if (c > 'Z')
                        {
                            if (c != '_')
                            {
                                throw new Exception($"FastString: character {c} is not in FastString alphabet");
                            }
                            change = c - 58; // _ only -> 37
                        }
                        else
                        {
                            if (c < 'A')
                            {
                                throw new Exception($"FastString: character {c} is not in FastString alphabet");
                            }

                            change = c - 54; // A-Z -> 11-36
                        }
                    }
                    else
                    {
                        if (c < '0')
                        {
                            throw new Exception($"FastString: character {c} is not in FastString alphabet");
                        }

                        change = c - 47; // 0-9 -> 1-10
                    }
                }
                else
                {
                    if (c > 'z')
                    {
                        throw new Exception($"FastString: character {c} is not in FastString alphabet");
                    }

                    change = c - 86; // a-z -> 11-36
                }

                val += (uint)change * seed;
                seed /= 38;
            }

            return val;
        }

        public static byte[] Encode(string str)
        {
            if (str.Length <= 6)
            {
                return BitConverter.GetBytes(Encode6(str)).Reverse().ToArray();
            }

            string str2 = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (i % 6 == 0 && i != 0)
                {
                    str2 += ',';
                }
                str2 += str[i];
            }
            string[] split = str2.Split(',');

            List<byte> bytes = new List<byte>();

            foreach (string s in split)
            {
                bytes.AddRange(BitConverter.GetBytes(Encode6(s)).Reverse());
            }

            return bytes.ToArray();
        }

        private static string Decode6(uint encoded)
        {
            string decoded = "";

            uint seed = Seed;

            while (encoded > 0 && seed > 0)
            {
                if (encoded / seed > 37)
                {
                    throw new Exception("Input value is not a valid fast string.");
                }
                decoded += CharSet[encoded / seed - 1];
                encoded %= seed;
                seed /= 38;
            }

            return decoded;
        }

        public static string Decode(byte[] bytes)
        {
            string str = "";

            foreach (byte[] chunk in bytes.Chunk(4))
            {
                str += Decode6(BitConverter.ToUInt32(chunk.Reverse().ToArray()));
            }

            return str;
        }
    }
}
