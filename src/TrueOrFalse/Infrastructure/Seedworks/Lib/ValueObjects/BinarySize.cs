using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpeakFriend.Utilities.ValueObjects
{
    [Serializable]
    public class BinarySize
    {
        public long Bytes;

        public BinarySize(){}

        public BinarySize(long length)
        {
            Bytes = length;
        }

        public string Formatted
        {
            get
            {
                int count = 0;
                double size = Bytes;

                while (size >= 1024)
                {
                    size /= 1024;
                    count++;
                }

                string unit;

                switch (count)
                {
                    case 0:
                        unit = "Byte";
                        break;
                    case 1:
                        unit = "KB";
                        break;
                    case 2:
                        unit = "MB";
                        break;
                    case 3:
                        unit = "GB";
                        break;
                    case 4:
                        unit = "TB";
                        break;
                    case 5:
                        unit = "PB";
                        break;
                    case 6:
                        unit = "EB";
                        break;
                    default:
                        throw new OverflowException();
                }

                return size.ToString("N") + " " + unit;
            }
        }

        public static BinarySize operator +(BinarySize a, BinarySize b)
        {
            return new BinarySize(a.Bytes + b.Bytes);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            if (Bytes == ((BinarySize)obj).Bytes)
                return true;

            return false;            
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
