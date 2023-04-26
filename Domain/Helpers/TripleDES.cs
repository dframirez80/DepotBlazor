using System.Text;
using System.Security.Cryptography;

namespace Domain.Helpers
{
    public class TripleDES
    {

        private const string mysecurityKey = "dfr80@hotmail.com";

        public static string Encrypt(string TextToEncrypt)
        {
            var MyEncryptedArray = Encoding.UTF8
               .GetBytes(TextToEncrypt);

            var MyMD5CryptoService = new MD5CryptoServiceProvider();

            var MysecurityKeyArray = MyMD5CryptoService.ComputeHash
               (Encoding.UTF8.GetBytes(mysecurityKey));

            MyMD5CryptoService.Clear();

            var MyTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            MyTripleDESCryptoService.Key = MysecurityKeyArray;

            MyTripleDESCryptoService.Mode = CipherMode.ECB;

            MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var MyCrytpoTransform = MyTripleDESCryptoService
               .CreateEncryptor();

            var MyresultArray = MyCrytpoTransform
               .TransformFinalBlock(MyEncryptedArray, 0,
               MyEncryptedArray.Length);

            MyTripleDESCryptoService.Clear();

            return Convert.ToBase64String(MyresultArray, 0,
               MyresultArray.Length);
        }

        public static string Decrypt(string TextToDecrypt)
        {
            var MyDecryptArray = Convert.FromBase64String
               (TextToDecrypt);

            var MyMD5CryptoService = new
               MD5CryptoServiceProvider();

            var MysecurityKeyArray = MyMD5CryptoService.ComputeHash
               (Encoding.UTF8.GetBytes(mysecurityKey));

            MyMD5CryptoService.Clear();

            var MyTripleDESCryptoService = new
               TripleDESCryptoServiceProvider();

            MyTripleDESCryptoService.Key = MysecurityKeyArray;

            MyTripleDESCryptoService.Mode = CipherMode.ECB;

            MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var MyCrytpoTransform = MyTripleDESCryptoService
               .CreateDecryptor();

            var MyresultArray = MyCrytpoTransform
               .TransformFinalBlock(MyDecryptArray, 0,
               MyDecryptArray.Length);

            MyTripleDESCryptoService.Clear();

            return Encoding.UTF8.GetString(MyresultArray);
        }
    }
}
