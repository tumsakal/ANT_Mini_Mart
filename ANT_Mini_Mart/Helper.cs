using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANT_Mini_Mart
{
    public static class Helper
    {
        public static byte[] ConvertImageToBytes(Image img)
        {
            MemoryStream memory = new MemoryStream();
            img.Save(memory, img.RawFormat);
            return memory.ToArray();
        }
        public static Image ConvertBytesToImage(byte[] raw)
        {
            MemoryStream memory = new MemoryStream(raw);
            Image img = Image.FromStream(memory);
            return img;
        }
    }
}
