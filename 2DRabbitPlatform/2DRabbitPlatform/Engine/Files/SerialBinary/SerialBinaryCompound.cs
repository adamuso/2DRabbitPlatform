using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IS = System.Runtime.InteropServices;
using Ref = System.Reflection;

namespace _2DRabbitPlatform.Engine.Files.SerialBinary
{
    public class SerialBinaryCompound
    {
        List<ValueInfo> binaries;
        object referenced;

        private SerialBinaryCompound()
        {
            binaries = new List<ValueInfo>();
        }

        public SerialBinaryCompound(object referencedObject)
            : this()
        {
            this.referenced = referencedObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">() => [var name]</param>
        public void addElement<T>(System.Linq.Expressions.Expression<Func<T>> value)
        {
            var body = ((System.Linq.Expressions.MemberExpression)value.Body);
            string fieldName = body.Member.Name;

            ValueInfo info = null;

            if (body.Member.MemberType == Ref.MemberTypes.Field)
                info = new ValueInfo(referenced.GetType().GetField(fieldName, Ref.BindingFlags.Instance | Ref.BindingFlags.NonPublic | Ref.BindingFlags.Public));
            else if (body.Member.MemberType == Ref.MemberTypes.Property)
                info = new ValueInfo(referenced.GetType().GetProperty(fieldName, Ref.BindingFlags.Instance | Ref.BindingFlags.Public | Ref.BindingFlags.NonPublic));

            if (info.IsValueType)
            {
                binaries.Add(info);
            }
            else if (info.IsList)
            {
                binaries.Add(info);
            }
            else if (info.IsString)
            {
                binaries.Add(info);
            }
            else
                throw new FormatException("Value needs to be a Value Type!");
        }

        public bool tryToSet(object obj, int parameter)
        {
            if (parameter < 0 || parameter > binaries.Count)
                return false;

            if(binaries[parameter].IsAssignableFrom(obj.GetType()))
            {
                binaries[parameter].SetValue(referenced, obj);
                return true;
            }

            return false;
        }

        public SerialBinaryCompound addElements<T>(params System.Linq.Expressions.Expression<Func<T>>[] values)
        {
            for (int i = 0; i < values.Length; i++)
                addElement(values[i]);

            return this;
        }

        private ValueInfo addElement(ValueInfo info)
        {
            binaries.Add(info);
            return info;
        }

        public T getElement<T>(int index)
        {
            return (T)binaries[index].GetValue(referenced);
        }

        public SerialBinaryCompound setReferenced(object referenced)
        {
            this.referenced = referenced;
            return this;
        }

        public void writeCompound(BinaryWriter writer, object referenced) 
        {
            object old = this.referenced;
            this.referenced = referenced;
            writeCompound(writer);
            this.referenced = old;
        }

        public void readCompound(BinaryReader reader, object referenced)
        {
            object old = this.referenced;
            this.referenced = referenced;
            readCompound(reader);
            this.referenced = old;
        }

        public void writeCompound(BinaryWriter writer)
        {
            for (int i = 0; i < binaries.Count; i++)
                writeVariable(writer, i);

        }

        public void readCompound(BinaryReader reader)
        {
            object o;

            for (int i = 0; i < binaries.Count; i++)
            {
                o = binaries[i].GetValue(referenced);

                if (o is bool)
                {
                    binaries[i].SetValue(referenced, reader.ReadBoolean());
                    continue;
                }

                if (binaries[i].IsList)
                {
                    readArray(reader, i);
                    continue;
                }

                if (binaries[i].IsString)
                {
                    binaries[i].SetValue(referenced, reader.ReadString());
                    continue;
                }

                binaries[i].SetValue(referenced, getFromBytes(i, reader.ReadBytes(getSize(o))));
            }
        }

        private byte[] getFromIndex(int index)
        {
            return getFromObject(binaries[index].GetValue(referenced));
        }

        private byte[] getFromObject(object o)
        {
            if (o.GetType().IsValueType)
            {
                // bool ----------------
                if (o is bool)
                    return BitConverter.GetBytes((bool)o);

                // value type -------------

                int size = getSize(o);

                IntPtr mem = IS.Marshal.AllocHGlobal(size);
                IS.Marshal.StructureToPtr(o, mem, true);
                byte[] ret = new byte[size];
                IS.Marshal.Copy(mem, ret, 0, ret.Length);
                IS.Marshal.FreeHGlobal(mem);

                return ret;
            }

            throw new Exception("Not value type!");
        }

        private object getFromBytes(int index, byte[] bytes)
        {
            if (binaries[index].IsValueType)
            {
                return getFromBytes(binaries[index].GetValue(referenced).GetType(), bytes);
            }

            throw new Exception("Not value type!");
        }

        private object getFromBytes(Type type, byte[] bytes)
        {
            if (type.IsValueType)
            {
                // value type and bool

                IntPtr mem = IS.Marshal.AllocHGlobal(bytes.Length);
                IS.Marshal.Copy(bytes, 0, mem, bytes.Length);
                object ret = IS.Marshal.PtrToStructure(mem, type);
                IS.Marshal.FreeHGlobal(mem);
                return ret;
            }

            throw new Exception("Not value type!");
        }

        private void writeVariable(BinaryWriter writer, int index)
        {
            if (!binaries[index].IsList)
            {
                if (binaries[index].IsString)
                {
                    writer.Write((string)binaries[index].GetValue(referenced));
                    return;
                }

                writer.Write(getFromIndex(index));
                return;
            }
            else
            {
                writeArray(writer, index);
                return;
            }
        }
               
        private void writeVariable(object o, BinaryWriter writer)
        {
            if (!(o is System.Collections.IList))
            {
                if (o is string)
                {
                    writer.Write((string)o);
                    return;
                }

                writer.Write(getFromObject(o));
                return;
            }

            throw new Exception();
        }

        private object readVariable(BinaryReader reader, Type type)
        {
            if (typeof(string).IsAssignableFrom(type))
            {
                return reader.ReadString();
            }
            else
            {
                return getFromBytes(type, reader.ReadBytes(getSize(type)));
            }
        }

        private void writeArray(BinaryWriter writer, int index)
        {
            if (binaries[index].IsList)
            {
                System.Collections.IList list = (System.Collections.IList)binaries[index].GetValue(referenced);

                writer.Write(list.Count);

                if (list.Count > 0)
                {
                    Type startType = list[0].GetType();
                    writer.Write(list[0].GetType().FullName);

                    for (int i = 0; i < list.Count; i++)
                    {
                        //writer.Write(getFromObject(list[i]));

                        writeVariable(list[i], writer); 

                        if (list[i].GetType() != startType)
                            throw new Exception("Multi type list!");
                    }
                }

                return;
            }

            throw new Exception();
        }

        private void readArray(BinaryReader reader, int index)
        {
            if (binaries[index].IsList)
            {
                int length = reader.ReadInt32();
                Type type = Type.GetType(reader.ReadString(), true, false);

                System.Collections.IList list = (System.Collections.IList)binaries[index].GetValue(referenced);

                if (list.IsFixedSize)
                {
                    if(binaries[index].GetValue(referenced) is Array)
                    {
                        binaries[index].SetValue(referenced, Array.CreateInstance(type, length));
                        list = (System.Collections.IList)binaries[index].GetValue(referenced);

                        for (int i = 0; i < length; i++)
                            list[i] = readVariable(reader, type);
                    }
                    else
                        throw new Exception();
                }
                else
                {
                    list.Clear();

                    for (int i = 0; i < length; i++)
                        list.Add(readVariable(reader, type));
                }
            }
            else
                throw new Exception();
        }

        private int getSize(Type o)
        {
            if (o.IsSubclassOf(typeof(bool)))
                return 1;

            return IS.Marshal.SizeOf(o);
        }

        private int getSize(object o)
        {
            return getSize(o.GetType());
        }

        public static SerialBinaryCompound Empty { get { return new SerialBinaryCompound(); } }

        private class ValueInfo
        {
            Ref.MemberInfo info;

            public ValueInfo(Ref.FieldInfo info)
            {
                this.info = info;
            }

            public ValueInfo(Ref.PropertyInfo info)
            {
                this.info = info;
            }

            public object GetValue(object referenced)
            {
                if (info.MemberType == Ref.MemberTypes.Field)
                    return ((Ref.FieldInfo)info).GetValue(referenced);
                else if (info.MemberType == Ref.MemberTypes.Property)
                    return ((Ref.PropertyInfo)info).GetValue(referenced, null);

                throw new Exception();
            }

            public void SetValue(object referenced, object value)
            {
                if (info.MemberType == Ref.MemberTypes.Field)
                    ((Ref.FieldInfo)info).SetValue(referenced, value);
                else if (info.MemberType == Ref.MemberTypes.Property)
                    ((Ref.PropertyInfo)info).SetValue(referenced, value, null);
            }

            public bool IsAssignableFrom(Type type)
            {
                if (info.MemberType == Ref.MemberTypes.Field)
                    return ((Ref.FieldInfo)info).FieldType.IsAssignableFrom(type);
                if (info.MemberType == Ref.MemberTypes.Property)
                    return ((Ref.PropertyInfo)info).PropertyType.IsAssignableFrom(type);

                return false;
            }

            public bool IsString { get { if (info.MemberType == Ref.MemberTypes.Field) return typeof(string).IsAssignableFrom(((Ref.FieldInfo)info).FieldType); else if (info.MemberType == Ref.MemberTypes.Property) return typeof(string).IsAssignableFrom(((Ref.PropertyInfo)info).PropertyType); else return false; } }
            public bool IsList { get { if (info.MemberType == Ref.MemberTypes.Field) return typeof(System.Collections.IList).IsAssignableFrom(((Ref.FieldInfo)info).FieldType); else if (info.MemberType == Ref.MemberTypes.Property) return typeof(System.Collections.IList).IsAssignableFrom(((Ref.PropertyInfo)info).PropertyType); else return false; } }
            public bool IsValueType { get { if (info.MemberType == Ref.MemberTypes.Field) return ((Ref.FieldInfo)info).FieldType.IsValueType; else if (info.MemberType == Ref.MemberTypes.Property) return ((Ref.PropertyInfo)info).PropertyType.IsValueType; else return false; } }
        }
    }
}
