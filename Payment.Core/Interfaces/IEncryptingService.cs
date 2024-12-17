namespace Payment.Core.Interfaces
{
    public interface IEncryptingService
    {
        string AES_decrypt(string encrypted);
        string AES_Encrypt(string plainText);
    }
}
