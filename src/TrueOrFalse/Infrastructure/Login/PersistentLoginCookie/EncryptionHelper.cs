using System.Security.Cryptography;
using System.Text;

public class EncryptionHelper
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public EncryptionHelper()
    {
        using (var sha256 = SHA256.Create())
        {
            _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(Settings.AuthCookieEncryptionKey));
        }

        using (var aes = Aes.Create())
        {
            aes.GenerateIV();
            _iv = aes.IV;
        }
    }

    public string EncryptString(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                byte[] ivAndCipherText = new byte[_iv.Length + ms.ToArray().Length];
                Buffer.BlockCopy(_iv, 0, ivAndCipherText, 0, _iv.Length);
                Buffer.BlockCopy(ms.ToArray(), 0, ivAndCipherText, _iv.Length, ms.ToArray().Length);
                return Convert.ToBase64String(ivAndCipherText);
            }
        }
    }

    public string DecryptString(string cipherText)
    {
        byte[] ivAndCipherText = Convert.FromBase64String(cipherText);
        byte[] iv = new byte[16];
        byte[] actualCipherText = new byte[ivAndCipherText.Length - iv.Length];

        Buffer.BlockCopy(ivAndCipherText, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(ivAndCipherText, iv.Length, actualCipherText, 0, actualCipherText.Length);

        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream(actualCipherText))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}