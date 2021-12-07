using System;
using System.Text;

namespace ChatCoreTest
{
  internal class Program
  {
    private static byte[] m_PacketData;
    private static uint m_Pos;
    public static void Main(string[] args)
    {
      m_PacketData = new byte[1024];
      m_Pos = 0;

            Write(109);
            Write(109.99f);
            Write("Hello!");

            UnPack(m_PacketData);
            Console.ReadLine();
      //Console.Write($"Output Byte array(length:{m_Pos}): ");
      //for (var i = 0; i < m_Pos; i++)
      //{
      //  Console.Write(m_PacketData[i] + ", ");
      //}
    }


        private static void UnPack(byte[] u)
        {
            byte[] typeData = new byte[2];
            byte[] intData = new byte[4];
            byte[] floatData = new byte[4];
            int uPos = 0;
            int uLen = 0;
            while(uPos!=m_Pos)
            {
                uLen = 0;
                for(int i =uPos;uPos<i+2;uPos++)
                {
                    typeData[uLen] = u[uPos];
                    uLen++;
                }
                switch (tRead(typeData)) 
                {
                    case 'I':
                        uLen = 0;
                            for (int i =uPos;i<uPos+4;i++)
                        {
                            intData[uLen] = u[i];
                            uLen++;
                        }
                        uPos += 4;
                        Console.WriteLine(iRead(intData));
                        break;
                    case 'F':
                        uLen = 0;
                        for (int i = uPos; i < uPos + 4; i++)
                        {
                            floatData[uLen] = u[i];
                            uLen++;
                        }
                        uPos += 4;
                        Console.WriteLine(fRead(floatData));
                        break;
                    case 'S':
                        uLen = 0;
                        for (int i = uPos; i < uPos + 4; i++)
                        {
                            intData[uLen] = u[i];
                            uLen++;
                        }
                        uPos += 4;
                        uLen = 0;
                        int sLen = iRead(intData);
                        var rString = new byte[sLen];
                        for(int i =uPos;i<uPos+sLen;i++)
                        {
                            rString[uLen] = u[i];
                            uLen++;
                        }
                        Console.WriteLine(sRead(rString));
                        break;
                }
            }
            

        }
    // write an integer into a byte array
    private static bool Write(int i)
    {
            // convert int to byte array
            _Write(BitConverter.GetBytes('I'));
            _Write(BitConverter.GetBytes(i));
            return true;
    }

    // write a float into a byte array
    private static bool Write(float f)
    {
            // convert int to byte array
            _Write(BitConverter.GetBytes('F'));
            _Write(BitConverter.GetBytes(f));
      return true;
    }

    // write a string into a byte array
    private static bool Write(string s)
    {
            // convert string to byte array
            // write byte array length to packet's byte array
            //if (Write(Encoding.Unicode.GetBytes(s).Length) == false)
            //{
            //  return false;
            //}
            _Write(BitConverter.GetBytes('S'));
            var sb = Encoding.Unicode.GetBytes(s);
            _Write(BitConverter.GetBytes(sb.Length));
            _Write(sb);
            return true;
    }

    // write a byte array into packet's byte array
    private static void _Write(byte[] byteData)
    {
      // converter little-endian to network's big-endian
      if (BitConverter.IsLittleEndian)
      {
        Array.Reverse(byteData);
      }

      byteData.CopyTo(m_PacketData, m_Pos);
      m_Pos +=(uint)byteData.Length;
    }
            private static char tRead(byte[] t)
            {
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(t);
                }
                Char c = BitConverter.ToChar(t, 0);
                return c;
            }
        private static int iRead(byte[] i)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(i);
            }
            int r = BitConverter.ToInt32(i, 0);
            return r;
        }
        private static float fRead(byte[] f)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(f);
            }
            float r = BitConverter.ToSingle(f, 0);
            return r;
        }
        private static string sRead(byte[] s)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(s);
            }
            string r = Encoding.Unicode.GetString(s);
            return r;
        }
    }
}
