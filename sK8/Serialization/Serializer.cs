using System.Numerics;

namespace sK8.Serialization
{
    public static class Serializer
    {
        internal static byte[] Serialize(params object[] objects)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                using (BufferWriter writer = new BufferWriter(buffer))
                {
                    foreach (object obj in objects)
                    {
                        switch (obj)
                        {
                            case null:
                                throw new ArgumentNullException("Object to be serialized must not be null!");

                            case Boolean bl:
                                writer.Write(bl);
                                break;

                            case Byte b:
                                writer.Write(b);
                                break;

                            case SByte sb:
                                writer.Write(sb);
                                break;

                            case UInt16 u16:
                                writer.WriteBE(u16);
                                break;

                            case Int16 s16:
                                writer.WriteBE(s16);
                                break;

                            case UInt32 u32:
                                writer.WriteBE(u32);
                                break;

                            case Int32 s32:
                                writer.WriteBE(s32);
                                break;

                            case UInt64 u64:
                                writer.WriteBE(u64);
                                break;

                            case Int64 s64:
                                writer.WriteBE(s64);
                                break;

                            case Half f16:
                                writer.WriteBE(f16);
                                break;

                            case Single f32:
                                writer.WriteBE(f32);
                                break;

                            case Double f64:
                                writer.WriteBE(f64);
                                break;

                            case String str:
                                writer.WriteNTString(str);
                                break;

                            case Vector4 v4:
                                writer.WriteBE(v4.X);
                                writer.WriteBE(v4.Y);
                                writer.WriteBE(v4.Z);
                                writer.WriteBE(v4.W);
                                break;

                            case ISerializable serializable:;
                                writer.Write(serializable.Serialize());
                                break;

                            default:
                                throw new NotSupportedException($"Type {obj.GetType().ToString()} is not serializable.");
                        }
                    }

                    buffer.Flush();
                }
                
                return buffer.ToArray();
            }
        }
        
        internal static object[] Deserialize(byte[] data, params object[] objects)
        {   
            List<object> result = new List<object>();

            using (MemoryStream buffer = new MemoryStream(data))
            {
                using (BufferReader reader = new BufferReader(buffer))
                {
                    foreach (object obj in objects)
                    {
                        switch (obj)
                        {
                            case null:
                                throw new ArgumentNullException("Object cannot be null!");

                            case UInt16 u16:
                                result.Add(reader.ReadUInt16BE());
                                break;

                            case Int16 s16:
                                result.Add(reader.ReadInt16BE());
                                break;

                            case UInt32 u32:
                                result.Add(reader.ReadUInt32BE());
                                break;

                            case Int32 s32:
                                result.Add(reader.ReadInt32BE());
                                break;

                            case UInt64 u64:
                                result.Add(reader.ReadUInt64BE());
                                break;

                            case Int64 s64:
                                result.Add(reader.ReadInt64BE());
                                break;

                            case Half f16:
                                result.Add(reader.ReadFloat16BE());
                                break;

                            case Single f32:
                                result.Add(reader.ReadFloat32BE());
                                break;

                            case Double f64:
                                result.Add(reader.ReadFloat64BE());
                                break;

                            case String str:
                                result.Add(reader.ReadNTString());
                                break;

                            case Vector4 v4:
                                result.Add(new Vector4(reader.ReadFloat32BE(), reader.ReadFloat32BE(), reader.ReadFloat32BE(), reader.ReadFloat32BE()));
                                break;

                            case ISerializable serializable:
                                ISerializable Object = (ISerializable)obj;
                                Object.Deserialize(reader.ReadBytes((int)Object.GetBufferSize()));
                                result.Add(Object);
                                break;

                            default:
                                throw new NotSupportedException($"Type {obj.GetType().ToString()} is not deserializable.");
                        }
                    }
                }
            }

            return result.ToArray();
        }
        
    }
}
