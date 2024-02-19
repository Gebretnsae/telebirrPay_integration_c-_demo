using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.Text;

namespace telebirrpay
{
    public static class RSAHelper
    {
        /// <summary>
        /// public key encryption
        /// </summary>
        /// <param name="context"></param>
        /// <param name="publicKeyBase64String"></param>
        /// <returns></returns>
        public static string EncryptionByPublicKey(string context, string publicKey)
        {
            AsymmetricKeyParameter encryptionKey = ConvertBase64StringToPublicKey(publicKey);
            string ussd = Encryption(context, encryptionKey);
            return ussd;
        }
        /// <summary>
        /// public key decryption
        /// </summary>
        /// <param name="encryptionString"></param>
        /// <param name="publicKeyBase64String"></param>
        /// <returns></returns>
        public static string DecryptByPublicKey(string encryptionString, string publicKeyBase64String)
        {
            AsymmetricKeyParameter decryptKey = ConvertBase64StringToPublicKey(publicKeyBase64String);

            return Decrypt(encryptionString, decryptKey);
        }

        private static AsymmetricKeyParameter ConvertBase64StringToPublicKey(string publicKeyBase64String)
        {
            byte[] publicKeyBytes = Convert.FromBase64String(publicKeyBase64String);
            Asn1Object aobject = Asn1Object.FromByteArray(publicKeyBytes);
            SubjectPublicKeyInfo pubInfo = SubjectPublicKeyInfo.GetInstance(aobject);
            AsymmetricKeyParameter publicKey = (RsaKeyParameters)PublicKeyFactory.CreateKey(pubInfo);
            return publicKey;
        }

        private static string Decrypt(string encryptionContext, AsymmetricKeyParameter decryptKey)
        {
            int MAX_DECRYPT_BLOCK = 256;
            int offset = 0;
            int i = 0;
            MemoryStream aMS = new MemoryStream();
            byte[] encryptionBytes = Convert.FromBase64String(encryptionContext);
            var inputLength = encryptionBytes.Length;
            IBufferedCipher c = CipherUtilities.GetCipher("RSA/NONE/PKCS1PADDING");
            c.Init(false, decryptKey);
            byte[] cache;
            while (inputLength - offset > 0)
            {
                if (inputLength - offset > MAX_DECRYPT_BLOCK)
                {
                    cache = c.DoFinal(encryptionBytes, offset, MAX_DECRYPT_BLOCK);
                }
                else
                {
                    cache = c.DoFinal(encryptionBytes, offset, inputLength - offset);
                }
                aMS.Write(cache, 0, cache.Length);
                i++;
                offset = i * MAX_DECRYPT_BLOCK;
            }
            byte[] cipherbytes = aMS.ToArray();
            return Encoding.UTF8.GetString(cipherbytes);
        }

        private static string Encryption(string randomStr, AsymmetricKeyParameter encryptionKey)
        {
            var MAX_ENCRYPT_BLOCK = 245;
            int offset = 0;
            int i = 0;
            var outBytes = new List<byte>();
            IBufferedCipher c = CipherUtilities.GetCipher("RSA/NONE/PKCS1PADDING");
            c.Init(true, encryptionKey);
            var data = Encoding.UTF8.GetBytes(randomStr);
            var inputLength = data.Length;
            while (inputLength - offset > 0)
            {
                if (inputLength - offset > MAX_ENCRYPT_BLOCK)
                {
                    outBytes.AddRange(c.DoFinal(data, offset, MAX_ENCRYPT_BLOCK));
                }
                else
                {
                    outBytes.AddRange(c.DoFinal(data, offset, inputLength - offset));
                }
                i++;
                offset = i * MAX_ENCRYPT_BLOCK;
            }
            return Convert.ToBase64String(outBytes.ToArray());
        }


    }
}
