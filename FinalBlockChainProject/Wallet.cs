using System;
using System.Security.Cryptography;
using System.Text;

public class Wallet
{
    public string PublicKey { get; }
    public string PrivateKey { get; }

    public Wallet()
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            PublicKey = rsa.ToXmlString(false); 
            PrivateKey = rsa.ToXmlString(true); 
        }
    }

    public string SignData(string data)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(PrivateKey); 
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] signatureBytes = rsa.SignData(dataBytes, SHA256.Create());
            return Convert.ToBase64String(signatureBytes);
        }
    }

    public bool VerifySignature(string data, string signature, string publicKey)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKey); 
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] signatureBytes = Convert.FromBase64String(signature);
            return rsa.VerifyData(dataBytes, SHA256.Create(), signatureBytes);
        }
    }
}