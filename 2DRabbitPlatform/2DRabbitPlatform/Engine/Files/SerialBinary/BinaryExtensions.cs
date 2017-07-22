using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Files.SerialBinary
{
    public static class BinaryExtensions
    {
        static Dictionary<Type, SerialBinaryCompound> compounds = new Dictionary<Type,SerialBinaryCompound>();

        public static SerialBinaryCompound createSBCompound(this IBinarySerializable bs)
        {
            if (!compounds.ContainsKey(bs.GetType()))
            {
                SerialBinaryCompound c = new SerialBinaryCompound(bs);
                compounds.Add(bs.GetType(), c);
                return c;
            }

            throw new Exception("Compound already created!");
        }

        public static SerialBinaryCompound getSBCompound(this IBinarySerializable bs)
        {
            if (compounds.ContainsKey(bs.GetType()))
                return compounds[bs.GetType()].setReferenced(bs);

            return SerialBinaryCompound.Empty;
        }

        public static bool hasSBCompound(this IBinarySerializable bs)
        {
            return compounds.ContainsKey(bs.GetType());
        }
    }
}
