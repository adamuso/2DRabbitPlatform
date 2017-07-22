using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform
{
    public class TestClass
    {
        Engine.Files.SerialBinary.SerialBinaryCompound SBC;
        System.IO.MemoryStream stream;
        bool b1;
        int i1;
        float[] fa1;
        List<short> ls1;
        string str1;
        List<string> ls2;

        public TestClass()
        {
            SBC = new Engine.Files.SerialBinary.SerialBinaryCompound(this);
            stream = new System.IO.MemoryStream();

            b1 = true;
            i1 = 10;
            fa1 = new float[] { 5.5f, 7.7f, 9.9f };
            ls1 = new List<short>(new short[] { 15, 20 });
            str1 = "Hello!";
            ls2 = new List<string>(new string[] { "World", "in", "danger" });

            SBC.addElement(() => b1);
            SBC.addElement(() => i1);
            SBC.addElement(() => fa1);
            SBC.addElement(() => ls1);
            SBC.addElement(() => str1);
            SBC.addElement(() => ls2);

            SBC.writeCompound(new System.IO.BinaryWriter(stream));

            b1 = false;
            i1 = 7;
            fa1 = new float[] { 3.0f};
            ls1 = new List<short>(new short[] { 56, 90 });
            str1 = "Byeeeeee!";
            ls2 = new List<string>(new string[] { "Nope so", "well", "in pocket" });
        }

        public void dosome()
        {
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            SBC.readCompound(new System.IO.BinaryReader(stream));
        }
    }
}
