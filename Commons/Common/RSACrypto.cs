using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class RSACrypto
    {
        private static readonly Encoding Encoder = Encoding.UTF8;
        private static string keyPath = Auxiliary.ConfigKey("DFsignFile");

        private static string privatePath = Auxiliary.ConfigKey("privateFile");

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static String Encrypt(this String plaintext)
        {
            X509Certificate2 _X509Certificate2 = RSACrypto.RetrieveX509Certificate(keyPath);
            using (RSACryptoServiceProvider RSACryptography = _X509Certificate2.PublicKey.Key as RSACryptoServiceProvider)
            {
                Byte[] PlaintextData = RSACrypto.Encoder.GetBytes(plaintext);
                int MaxBlockSize = RSACryptography.KeySize / 8 - 11;    //加密块最大长度限制

                if (PlaintextData.Length <= MaxBlockSize)
                    return Convert.ToBase64String(RSACryptography.Encrypt(PlaintextData, false));

                using (MemoryStream PlaiStream = new MemoryStream(PlaintextData))
                using (MemoryStream CrypStream = new MemoryStream())
                {
                    Byte[] Buffer = new Byte[MaxBlockSize];
                    int BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);

                    while (BlockSize > 0)
                    {
                        Byte[] ToEncrypt = new Byte[BlockSize];
                        Array.Copy(Buffer, 0, ToEncrypt, 0, BlockSize);

                        Byte[] Cryptograph = RSACryptography.Encrypt(ToEncrypt, false);
                        CrypStream.Write(Cryptograph, 0, Cryptograph.Length);

                        BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
                    }

                    return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(Convert.ToBase64String(CrypStream.ToArray(), Base64FormattingOptions.None)));
                }
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public static String Decrypt(this String ciphertext)
        {
            X509Certificate2 _X509Certificate2 = RSACrypto.RetrieveX509Certificate(privatePath);
            using (RSACryptoServiceProvider RSACryptography = _X509Certificate2.PrivateKey as RSACryptoServiceProvider)
            {
                Byte[] CiphertextData = Convert.FromBase64String(ciphertext);
                int MaxBlockSize = RSACryptography.KeySize / 8;    //解密块最大长度限制

                if (CiphertextData.Length <= MaxBlockSize)
                    return RSACrypto.Encoder.GetString(RSACryptography.Decrypt(CiphertextData, false));

                using (MemoryStream CrypStream = new MemoryStream(CiphertextData))
                using (MemoryStream PlaiStream = new MemoryStream())
                {
                    Byte[] Buffer = new Byte[MaxBlockSize];
                    int BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);

                    while (BlockSize > 0)
                    {
                        Byte[] ToDecrypt = new Byte[BlockSize];
                        Array.Copy(Buffer, 0, ToDecrypt, 0, BlockSize);

                        Byte[] Plaintext = RSACryptography.Decrypt(ToDecrypt, false);
                        PlaiStream.Write(Plaintext, 0, Plaintext.Length);

                        BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                    }

                    return RSACrypto.Encoder.GetString(PlaiStream.ToArray());
                }
            }
        }

        private static X509Certificate2 RetrieveX509Certificate(string keyPath)
        {
            return new X509Certificate2(keyPath);    //检索用于 RSA 加密的 X509Certificate2 证书
        }

        //
         public static RSACryptoServiceProvider DecodeRSAPrivateKey(string priKey)
        {
            var privkey = Convert.FromBase64String(priKey);
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();        //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();       //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)        //expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();    // data size in next byte
            else
                if (bt == 0x82)
                {
                    highbyte = binr.ReadByte();    // data size in next 2 bytes
                    lowbyte = binr.ReadByte();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    count = BitConverter.ToInt32(modint, 0);
                }
                else
                {
                    count = bt;        // we already have the data size
                }

            while (binr.ReadByte() == 0x00)
            {    //remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);        //last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
    
    }
}
