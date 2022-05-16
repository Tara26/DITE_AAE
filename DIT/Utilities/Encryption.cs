using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DIT.Utilities
{
	public class Encryption
	{
		public string RandomString(int size, bool lowerCase)
		{
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			char ch;
			for (int i = 0; i < size; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
				builder.Append(ch);
			}
			if (lowerCase)
				return builder.ToString().ToLower();
			return builder.ToString();
		}
		public static string CreateToken(string message, string secret)
		{
			secret = secret ?? "";
			//var sha256 = Crypto.SHA256(message);    
			//var salt = Crypto.GenerateSalt();
			//var hashedPassword = Crypto.HashPassword(message);
			// var verify = Crypto.VerifyHashedPassword("{hashedPassword}", message);     
			var encoding = new System.Text.ASCIIEncoding();
			byte[] keyByte = encoding.GetBytes(secret);
			byte[] messageBytes = encoding.GetBytes(message);
			using (var hmacsha256 = new HMACSHA256(keyByte))
			{
				byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
				return Convert.ToBase64String(hashmessage);
			}
		}
		public static byte[] Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return plainTextBytes;
		}
		public static string DecryptStringAES(string cipherText)
		{
			//cipherText = cipherText.Replace(" ", "+");
			var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
			var iv = Encoding.UTF8.GetBytes("8080808080808080");

			var encrypted = Convert.FromBase64String(cipherText);
			var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
			return string.Format(decriptedFromJavascript);
		}
		private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
		{
			// Check arguments.
			if (cipherText == null || cipherText.Length <= 0)
			{
				throw new ArgumentNullException("cipherText");
			}
			if (key == null || key.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}
			if (iv == null || iv.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}

			// Declare the string used to hold
			// the decrypted text.
			string plaintext = null;

			// Create an RijndaelManaged object
			// with the specified key and IV.
			using (var rijAlg = new RijndaelManaged())
			{
				//Settings
				rijAlg.Mode = CipherMode.CBC;
				rijAlg.Padding = PaddingMode.PKCS7;
				rijAlg.FeedbackSize = 128;

				rijAlg.Key = key;
				rijAlg.IV = iv;

				// Create a decrytor to perform the stream transform.
				var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
				try
				{
					// Create the streams used for decryption.
					using (var msDecrypt = new MemoryStream(cipherText))
					{
						using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
						{

							using (var srDecrypt = new StreamReader(csDecrypt))
							{
								// Read the decrypted bytes from the decrypting stream
								// and place them in a string.
								plaintext = srDecrypt.ReadToEnd();

							}

						}
					}
				}
				catch (Exception ex)
				{
					plaintext = "keyError";
				}
			}

			return plaintext;
		}
		private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
		{
			// Check arguments.
			if (plainText == null || plainText.Length <= 0)
			{
				throw new ArgumentNullException("plainText");
			}
			if (key == null || key.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}
			if (iv == null || iv.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}
			byte[] encrypted;
			// Create a RijndaelManaged object
			// with the specified key and IV.
			using (var rijAlg = new RijndaelManaged())
			{
				rijAlg.Mode = CipherMode.CBC;
				rijAlg.Padding = PaddingMode.PKCS7;
				rijAlg.FeedbackSize = 128;

				rijAlg.Key = key;
				rijAlg.IV = iv;

				// Create a decrytor to perform the stream transform.
				var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

				// Create the streams used for encryption.
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (var swEncrypt = new StreamWriter(csEncrypt))
						{
							//Write all data to the stream.
							swEncrypt.Write(plainText);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}

			// Return the encrypted bytes from the memory stream.
			return encrypted;
		}
		public static string CreateHashWithoutSalt(string plainText)
		{
			string hash = "";
			using (SHA256 sha256Hash = SHA256.Create())
			{
				hash = GetHash(sha256Hash, plainText);


				if (VerifyHash(sha256Hash, plainText, hash))
				{
					return hash;
				}
				else
				{
					return "";
				}
			}

		}
		public static string CreateHashWithSalt(string plainText, string storedsalt)
		{
			string hash1 = "";
			string hash2 = "";
			using (SHA256 sha256Hash = SHA256.Create())
			{
				hash1 = GetHash(sha256Hash, plainText);

			var	concathashwithsalt = Convert.ToString("" + hash1 + "" + storedsalt + "");
				hash2 = GetHash(sha256Hash, concathashwithsalt);
				if (VerifyHash(sha256Hash, concathashwithsalt, hash2))
				{
					return hash2;
				}
				else
				{
					return "";
				}
			}

		}
		public static string GetHash(HashAlgorithm hashAlgorithm, string input)
		{

			// Convert the input string to a byte array and compute the hash.
			byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			var sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data
			// and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			// Return the hexadecimal string.
			return sBuilder.ToString();
		}

		// Verify a hash against a string.
		public static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
		{
			// Hash the input.
			var hashOfInput = GetHash(hashAlgorithm, input);

			// Create a StringComparer an compare the hashes.
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			return comparer.Compare(hashOfInput, hash) == 0;
		}

	}
}