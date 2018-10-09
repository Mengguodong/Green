using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Common;


public class SignatureUtil
{
    public SignatureUtil()
    {

    }

    private static int GetIntegerSize(BinaryReader binr)
    {
        byte bt = 0;
        byte lowbyte = 0x00;
        byte highbyte = 0x00;
        int count = 0;
        bt = binr.ReadByte();
        if (bt != 0x02)
            return 0;
        bt = binr.ReadByte();

        if (bt == 0x81)
            count = binr.ReadByte();
        else
            if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }
        while (binr.ReadByte() == 0x00)
        {
            count -= 1;
        }
        binr.BaseStream.Seek(-1, SeekOrigin.Current);
        return count;
    }

    private static bool CompareBytearrays(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            return false;
        int i = 0;
        foreach (byte c in a)
        {
            if (c != b[i])
                return false;
            i++;
        }
        return true;
    }

    public byte[] GetPkcs8PrivateKey(String privateKeyFile)
    {
        const String pemp8header = "-----BEGIN CERTIFICATE-----";
        const String pemp8footer = "-----END CERTIFICATE-----";
        StreamReader sr = System.IO.File.OpenText(privateKeyFile);
        String pemstr = sr.ReadToEnd().Trim();
        sr.Close();
        byte[] privateBinKey = null;
        if (pemstr.StartsWith(pemp8header) && pemstr.EndsWith(pemp8footer))
        {
            StringBuilder sb = new StringBuilder(pemstr);
            sb.Replace(pemp8header, "");
            sb.Replace(pemp8footer, "");
            String privateKey = sb.ToString().Trim();
            try
            {
                privateBinKey = Convert.FromBase64String(privateKey);
            }
            catch (System.FormatException)
            {
                return null;
            }
        }
        return privateBinKey;
    }

    public RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
    {
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
            if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                binr.ReadByte();	//advance 1 byte
            else if (twobytes == 0x8230)
                binr.ReadInt16();	//advance 2 bytes
            else
                return null;

            twobytes = binr.ReadUInt16();
            if (twobytes != 0x0102)	//version number
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

            /*
            if (verbose)
            {
                showBytes("\nModulus", MODULUS);
                showBytes("\nExponent", E);
                showBytes("\nD", D);
                showBytes("\nP", P);
                showBytes("\nQ", Q);
                showBytes("\nDP", DP);
                showBytes("\nDQ", DQ);
                showBytes("\nIQ", IQ);
            }*/

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
        finally { binr.Close(); }
    }

    public RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
    {
        // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
        // this byte[] includes the sequence byte and terminal encoded null 
        byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
        byte[] seq = new byte[15];
        // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
        MemoryStream mem = new MemoryStream(pkcs8);
        int lenstream = (int)mem.Length;
        BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
        byte bt = 0;
        ushort twobytes = 0;

        try
        {
            twobytes = binr.ReadUInt16();
            if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                binr.ReadByte();	//advance 1 byte
            else if (twobytes == 0x8230)
                binr.ReadInt16();	//advance 2 bytes
            else
                return null;


            bt = binr.ReadByte();
            if (bt != 0x02)
                return null;

            twobytes = binr.ReadUInt16();

            if (twobytes != 0x0001)
                return null;

            seq = binr.ReadBytes(15);		//read the Sequence OID
            if (!CompareBytearrays(seq, SeqOID))	//make sure Sequence for OID is correct
                return null;

            bt = binr.ReadByte();
            if (bt != 0x04)	//expect an Octet string 
                return null;

            bt = binr.ReadByte();		//read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
            if (bt == 0x81)
                binr.ReadByte();
            else
                if (bt == 0x82)
                    binr.ReadUInt16();
            //------ at this stage, the remaining sequence should be the RSA private key

            byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
            RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
            return rsacsp;
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

    public String Sign(String privateKeyFile, byte[] dataBytes)
    {
        byte[] pkcs8privatekey = GetPkcs8PrivateKey(privateKeyFile);
        if (pkcs8privatekey != null)
        {
            RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8privatekey);
            if (rsa != null)
            {
                byte[] signatureBytes = rsa.SignData(dataBytes, "MD5");
                return Convert.ToBase64String(signatureBytes);
            }
        }
        return null;
    }

    public bool verifySign(String publicKeyFile, byte[] dataBytes, String sign)
    {
        X509Store store = new X509Store(StoreName.Root);
        store.Open(OpenFlags.ReadWrite);
        try
        {
            //for .Net Framework2.0(VS .NET 2005)
            X509Certificate2 certificate = new X509Certificate2(publicKeyFile);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(certificate.PublicKey.Key.ToXmlString(false));
            //if (rsa.VerifyData(dataBytes, "MD5", Convert.FromBase64String(sign)))
            if (rsa.VerifyData(dataBytes, "MD5", Base64Helper.FromBase64String(sign)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

}
