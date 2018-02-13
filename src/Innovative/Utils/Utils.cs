using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace PSY.Innovative.Utils
{
    public static class Utils
    {
        private static readonly Dictionary<string, int> AsciiExtendedKeys = new Dictionary<string, int>
        {
            {"€", 128},
            {"‚", 130},
            {"ƒ", 131},
            {"„", 132},
            {"…", 133},
            {"†", 134},
            {"‡", 135},
            {"ˆ", 136},
            {"‰", 137},
            {"Š", 138},
            {"‹", 139},
            {"Œ", 140},
            {"Ž", 142},
            {"‘", 145},
            {"’", 146},
            {"“", 147},
            {"”", 148},
            {"•", 149},
            {"–", 150},
            {"—", 151},
            {"˜", 152},
            {"™", 153},
            {"š", 154},
            {"›", 155},
            {"œ", 156},
            {"ž", 158},
            {"Ÿ", 159},
            {"¡", 161},
            {"¢", 162},
            {"£", 163},
            {"¤", 164},
            {"¥", 165},
            {"¦", 166},
            {"§", 167},
            {"¨", 168},
            {"©", 169},
            {"ª", 170},
            {"«", 171},
            {"¬", 172},
            {"®", 174},
            {"¯", 175},
            {"°", 176},
            {"±", 177},
            {"²", 178},
            {"³", 179},
            {"´", 180},
            {"µ", 181},
            {"¶", 182},
            {"·", 183},
            {"¸", 184},
            {"¹", 185},
            {"º", 186},
            {"»", 187},
            {"¼", 188},
            {"½", 189},
            {"¾", 190},
            {"¿", 191},
            {"À", 192},
            {"Á", 193},
            {"Â", 194},
            {"Ã", 195},
            {"Ä", 196},
            {"Å", 197},
            {"Æ", 198},
            {"Ç", 199},
            {"È", 200},
            {"É", 201},
            {"Ê", 202},
            {"Ë", 203},
            {"Ì", 204},
            {"Í", 205},
            {"Î", 206},
            {"Ï", 207},
            {"Ð", 208},
            {"Ñ", 209},
            {"Ò", 210},
            {"Ó", 211},
            {"Ô", 212},
            {"Õ", 213},
            {"Ö", 214},
            {"×", 215},
            {"Ø", 216},
            {"Ù", 217},
            {"Ú", 218},
            {"Û", 219},
            {"Ü", 220},
            {"Ý", 221},
            {"Þ", 222},
            {"ß", 223},
            {"à", 224},
            {"á", 225},
            {"â", 226},
            {"ã", 227},
            {"ä", 228},
            {"å", 229},
            {"æ", 230},
            {"ç", 231},
            {"è", 232},
            {"é", 233},
            {"ê", 234},
            {"ë", 235},
            {"ì", 236},
            {"í", 237},
            {"î", 238},
            {"ï", 239},
            {"ð", 240},
            {"ñ", 241},
            {"ò", 242},
            {"ó", 243},
            {"ô", 244},
            {"õ", 245},
            {"ö", 246},
            {"÷", 247},
            {"ø", 248},
            {"ù", 249},
            {"ú", 250},
            {"û", 251},
            {"ü", 252},
            {"ý", 253},
            {"þ", 254},
            {"ÿ", 255}
        };

        public static ushort[] _crc16_table =
        {
            0X0000, 0XC0C1, 0XC181, 0X0140, 0XC301, 0X03C0, 0X0280, 0XC241,
            0XC601, 0X06C0, 0X0780, 0XC741, 0X0500, 0XC5C1, 0XC481, 0X0440,
            0XCC01, 0X0CC0, 0X0D80, 0XCD41, 0X0F00, 0XCFC1, 0XCE81, 0X0E40,
            0X0A00, 0XCAC1, 0XCB81, 0X0B40, 0XC901, 0X09C0, 0X0880, 0XC841,
            0XD801, 0X18C0, 0X1980, 0XD941, 0X1B00, 0XDBC1, 0XDA81, 0X1A40,
            0X1E00, 0XDEC1, 0XDF81, 0X1F40, 0XDD01, 0X1DC0, 0X1C80, 0XDC41,
            0X1400, 0XD4C1, 0XD581, 0X1540, 0XD701, 0X17C0, 0X1680, 0XD641,
            0XD201, 0X12C0, 0X1380, 0XD341, 0X1100, 0XD1C1, 0XD081, 0X1040,
            0XF001, 0X30C0, 0X3180, 0XF141, 0X3300, 0XF3C1, 0XF281, 0X3240,
            0X3600, 0XF6C1, 0XF781, 0X3740, 0XF501, 0X35C0, 0X3480, 0XF441,
            0X3C00, 0XFCC1, 0XFD81, 0X3D40, 0XFF01, 0X3FC0, 0X3E80, 0XFE41,
            0XFA01, 0X3AC0, 0X3B80, 0XFB41, 0X3900, 0XF9C1, 0XF881, 0X3840,
            0X2800, 0XE8C1, 0XE981, 0X2940, 0XEB01, 0X2BC0, 0X2A80, 0XEA41,
            0XEE01, 0X2EC0, 0X2F80, 0XEF41, 0X2D00, 0XEDC1, 0XEC81, 0X2C40,
            0XE401, 0X24C0, 0X2580, 0XE541, 0X2700, 0XE7C1, 0XE681, 0X2640,
            0X2200, 0XE2C1, 0XE381, 0X2340, 0XE101, 0X21C0, 0X2080, 0XE041,
            0XA001, 0X60C0, 0X6180, 0XA141, 0X6300, 0XA3C1, 0XA281, 0X6240,
            0X6600, 0XA6C1, 0XA781, 0X6740, 0XA501, 0X65C0, 0X6480, 0XA441,
            0X6C00, 0XACC1, 0XAD81, 0X6D40, 0XAF01, 0X6FC0, 0X6E80, 0XAE41,
            0XAA01, 0X6AC0, 0X6B80, 0XAB41, 0X6900, 0XA9C1, 0XA881, 0X6840,
            0X7800, 0XB8C1, 0XB981, 0X7940, 0XBB01, 0X7BC0, 0X7A80, 0XBA41,
            0XBE01, 0X7EC0, 0X7F80, 0XBF41, 0X7D00, 0XBDC1, 0XBC81, 0X7C40,
            0XB401, 0X74C0, 0X7580, 0XB541, 0X7700, 0XB7C1, 0XB681, 0X7640,
            0X7200, 0XB2C1, 0XB381, 0X7340, 0XB101, 0X71C0, 0X7080, 0XB041,
            0X5000, 0X90C1, 0X9181, 0X5140, 0X9301, 0X53C0, 0X5280, 0X9241,
            0X9601, 0X56C0, 0X5780, 0X9741, 0X5500, 0X95C1, 0X9481, 0X5440,
            0X9C01, 0X5CC0, 0X5D80, 0X9D41, 0X5F00, 0X9FC1, 0X9E81, 0X5E40,
            0X5A00, 0X9AC1, 0X9B81, 0X5B40, 0X9901, 0X59C0, 0X5880, 0X9841,
            0X8801, 0X48C0, 0X4980, 0X8941, 0X4B00, 0X8BC1, 0X8A81, 0X4A40,
            0X4E00, 0X8EC1, 0X8F81, 0X4F40, 0X8D01, 0X4DC0, 0X4C80, 0X8C41,
            0X4400, 0X84C1, 0X8581, 0X4540, 0X8701, 0X47C0, 0X4680, 0X8641,
            0X8201, 0X42C0, 0X4380, 0X8341, 0X4100, 0X81C1, 0X8081, 0X4040
        };

        public static void AddSorted<T>(this IList<T> list, T item, IComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;

            var i = 0;
            while (i < list.Count && comparer.Compare(list[i], item) < 0)
                i++;

            list.Insert(i, item);
        }

        public static void Sort<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector, bool descending = false)
        {
            var sortedList = descending ? source.OrderByDescending(keySelector).ToList() : source.OrderBy(keySelector).ToList();

            source.Clear();
            foreach (var sortedItem in sortedList)
                source.Add(sortedItem);
        }

        public static void SortWith2Keys<TSource, TKeyFirst, TKeySecond>(this Collection<TSource> source, Func<TSource, TKeyFirst> keySelector, Func<TSource, TKeySecond> secondKeySelector,
                                                                         bool descendingFirst, bool descendingSecond)
        {
            var sortedList = descendingFirst ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
            if (secondKeySelector != null)
            {
                sortedList = descendingSecond ? sortedList.ThenByDescending(secondKeySelector) : sortedList.ThenBy(secondKeySelector);
            }

            var tempCollection = sortedList.ToList();
            source.Clear();
            foreach (var sortedItem in tempCollection)
                source.Add(sortedItem);
        }

        /// <summary>
        ///     Strips all non alphanumeric chars
        /// </summary>
        /// <param name="rawName"></param>
        /// <returns></returns>
        public static string GetHumanReadableFileName(string rawName)
        {
            var rgx = new Regex("[^a-zA-Z0-9 _]");
            var humanReadableName = rgx.Replace(rawName, "");
            return humanReadableName;
        }

        public static string GetHumanReadableRandomPassword()
        {
            return GetHumanReadableRandomString(8);
        }

        public static string GetHumanReadableRandomString(int digits, bool useWords = false, int maxWordLength = 8)
        {
            var password = "";

            var random = new Random((int)DateTime.Now.Ticks);

            for (var i = 0; i < digits; i++)
            {
                byte character = 33; //"!"

                if (useWords && i != 0 && i % maxWordLength == 0)
                {
                    password += " ";
                    continue;
                }

                switch (random.Next(1000) % 3)
                {
                    case 0:
                        character = (byte)(48 + random.Next(0, 9));
                        break;
                    case 1:
                        character = (byte)(65 + random.Next(0, 25));
                        break;
                    case 2:
                        character = (byte)(97 + random.Next(0, 25));
                        break;
                }

                password += ByteToString(character);
            }
            return password;
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            if (val.CompareTo(max) > 0) return max;
            return val;
        }

        public static bool IsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                   && Comparer<T>.Default.Compare(item, end) <= 0;
        }

        public static string AddOrRemoveStarFromLabel(bool isModify, string labelText)
        {
            if (string.IsNullOrEmpty(labelText))
                return labelText;

            if (!isModify && labelText.EndsWith("*"))
            {
                labelText = labelText.Substring(0, labelText.Length - 1);
            }
            else if (isModify)
            {
                labelText = labelText + "*";
            }

            return labelText;
        }


        public static string GetOnlyAsciiSuportedString(string value)
        {
            var allInputedChars = value.ToCharArray();
            var newValue = "";


            foreach (var tmpChar in allInputedChars)
            {
                var valueByte = Encoding.UTF8.GetBytes(tmpChar.ToString());
                if (valueByte.Length == 1 || GetAsciiExtendedKey(tmpChar.ToString()) != null)
                {
                    newValue += tmpChar.ToString();
                }
            }

            return newValue;
        }

        public static int? GetAsciiExtendedKey(string value)
        {
            if (AsciiExtendedKeys.ContainsKey(value))
            {
                return AsciiExtendedKeys[value];
            }

            return null;
        }

        public static string GetAsciiExtendedValue(int key)
        {
            if (AsciiExtendedKeys.ContainsValue(key))
            {
                return AsciiExtendedKeys.FirstOrDefault(k => k.Value == key).Key;
            }

            return "";
        }

        public static string EscapeHtml(string text)
        {
            return string.IsNullOrEmpty(text) ? "" : text.Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public static string BytesToString(byte[] bytes)
        {
            var result = "";
            foreach (var tmpByte in bytes)
            {
                result += ByteToString(tmpByte);
            }

            return result.Replace("\0", "");
        }

        private static string ByteToString(byte value)
        {
            var asciiExt = GetAsciiExtendedValue(value);
            if (asciiExt != string.Empty)
            {
                return asciiExt;
            }
            return Encoding.UTF8.GetString(new[] { value }, 0, 1);
        }

        public static byte[] StringToBytes(string value)
        {
            var allInputedChars = value.ToCharArray();
            var newValue = new List<byte>();

            foreach (var tmpChar in allInputedChars)
            {
                var valueByte = Encoding.UTF8.GetBytes(tmpChar.ToString());
                if (valueByte.Length == 1)
                {
                    newValue.Add(valueByte[0]);
                }
                else
                {
                    var extAscii = GetAsciiExtendedKey(tmpChar.ToString());

                    if (extAscii != null)
                        newValue.Add((byte)extAscii);
                }
            }

            return newValue.ToArray(); //Encoding.UTF8.GetBytes(value);
        }

        public static byte SetBit(byte data, byte bitIndex)
        {
            if (bitIndex < 8)
            {
                data |= (byte)(0x01 << bitIndex);
            }
            return data;
        }

        public static byte ResetBit(byte data, byte bitIndex)
        {
            if (bitIndex < 8)
            {
                data &= (byte)~(0x01 << bitIndex);
            }
            return data;
        }

        public static byte ToggleBit(byte data, byte bitIndex)
        {
            if (bitIndex < 8)
            {
                data ^= (byte)~(0x01 << bitIndex);
            }
            return data;
        }

        public static bool IsBitSet(byte data, byte bitIndex)
        {
            return (data & (0x1 << bitIndex)) != 0;
        }

        public static uint SetBit(uint data, byte bitIndex)
        {
            if (bitIndex < 32)
            {
                data |= (byte)(0x01 << bitIndex);
            }
            return data;
        }

        public static uint ResetBit(uint data, byte bitIndex)
        {
            if (bitIndex < 32)
            {
                data &= (byte)~(0x01 << bitIndex);
            }
            return data;
        }

        public static uint ToggleBit(uint data, byte bitIndex)
        {
            if (bitIndex < 32)
            {
                data ^= (byte)~(0x01 << bitIndex);
            }
            return data;
        }

        public static bool IsBitSet(uint data, byte bitIndex)
        {
            return (data & (0x1 << bitIndex)) != 0;
        }

        public static ushort calcCRC16(byte[] b, int length = 0)
        {
            ushort wCRCWord = 0xFFFF;
            var i = 0;
            while (i < (length == 0 ? b.Length : length))
            {
                var nTemp = (byte)(b[i++] ^ wCRCWord);
                wCRCWord >>= 8;
                wCRCWord ^= _crc16_table[nTemp];
            }
            return wCRCWord;
        }

        public static TimeSpan ConvertPirValueToTimeSpan(ushort value)
        {
            //infinity
            if (value == 0xFFFF)
            {
                return TimeSpan.MaxValue;
            }

            if (value <= 3600)
            {
                return TimeSpan.FromSeconds(value);
            }

            return TimeSpan.FromSeconds(3600 + (value - 3600) * 10);
        }

        public static ushort ConvertTimeSpanToPirValue(TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.MaxValue)
            {
                return 0xFFFF;
            }

            if (timeSpan.Hours == 0)
            {
                return (ushort)timeSpan.TotalSeconds;
            }

            timeSpan = timeSpan - TimeSpan.FromSeconds(3600);
            return (ushort)(3600 + ((int)timeSpan.TotalSeconds / 10));
        }

        public static Color GetColorForTemperature(int temperature, int maxTemperature, int minTemperature, double alpha = 1)
        {
            var deltaTemperature = maxTemperature - minTemperature;
            var minChannelLevel = 0.2;
            var maxChannelLevel = 0.8;
            var deltaChannelColor = maxChannelLevel - minChannelLevel;
            var ratio = deltaChannelColor / deltaTemperature;

            var channelVariation = (temperature - minTemperature) * ratio;

            var redChannel = minChannelLevel + channelVariation;
            var blueChannel = maxChannelLevel - channelVariation;

            return Color.FromRgba(redChannel, 0.6, blueChannel, alpha);
        }

        public static Color GetColorForTemperature2(int temperature, int maxTemperature, int minTemperature, double alpha = 1)
        {
            var deltaTemperature = maxTemperature - minTemperature;
            var halfRange = (deltaTemperature / 2);
            var GreenMinValue = 0.4;
            var greenVariationSpeed = (1.0 - GreenMinValue) / halfRange;
            var otherColorVariationSpeed = (1.0) / halfRange;

            var relativeTemp = temperature - minTemperature;

            double r, g, b;
            if (relativeTemp < halfRange)
            {
                r = 1.0;
                g = GreenMinValue + greenVariationSpeed * (relativeTemp);
                b = otherColorVariationSpeed * (relativeTemp);
            }
            else
            {
                r = 1.0 - otherColorVariationSpeed * (relativeTemp - halfRange);
                g = 1.0 - greenVariationSpeed * (relativeTemp - halfRange);
                b = 1.0;
            }
            return Color.FromRgba(r, g, b, alpha);
        }
    }
}