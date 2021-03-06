﻿using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

/// --------------------------------------------------------------------------------
// Methods in order to encrypt/decrypt stored game data
//
// Easiest way to start using this is to call following code on app startup:
// if(!CyrptoUtil.ValidKeys()) {
// 	 CryptoUtil.ResetKeys();
// }
// Proceed with game
//
// This will create your key/IV, and a validity test that runs to ensure it hasn't been tampered with
//
// (Note: custom Flipt code used for VALID_TEXT and at the bottom of CreateKeys())
// (This should be changed when ported elsewhere)
/// --------------------------------------------------------------------------------
public class CryptoUtil {

	private const string VALID_TEXT = "Flipt";

	// Checks key validity
	public static bool ValidKeys() {
		if(!PlayerPrefs.HasKey("ValidTest")) {
			CreateKeys();
		}
		else {
			bool validKeys = TestKeys();
			if(!validKeys) {
				return false;
			}
		}
		return true;
	}

	// Resets keys (perhaps if validity test fails)
	public static void ResetKeys() {
		PlayerPrefs.DeleteKey("AuthKey");
		PlayerPrefs.DeleteKey("AuthIV");
		PlayerPrefs.DeleteKey("ValidTest");
		CreateKeys();
	}

	// Decrypts input string
	public static string Decrypt(string encryptedText) {
		byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
		byte[] keyBytes = Convert.FromBase64String(PlayerPrefs.GetString("AuthKey"));
		byte[] IVBytes = Convert.FromBase64String(PlayerPrefs.GetString("AuthIV"));
		return DecryptStringFromBytes(encryptedBytes, keyBytes, IVBytes);
	}
	// Encrypts input string
	public static string Encrypt(string unencryptedText) {
		byte[] keyBytes = Convert.FromBase64String(PlayerPrefs.GetString("AuthKey"));
		byte[] IVBytes = Convert.FromBase64String(PlayerPrefs.GetString("AuthIV"));
		return Convert.ToBase64String(EncryptStringToBytes(unencryptedText, keyBytes, IVBytes));
	}


	/// Private base methods -------------------------------------------------------
	// Creates keys and saves validity test
	private static void CreateKeys() {
		using(RijndaelManaged rij = new RijndaelManaged()) {
			rij.GenerateKey();
			rij.GenerateIV();
			byte[] keyBytes = rij.Key;
			byte[] IVBytes = rij.IV;
			string authKey = Convert.ToBase64String(keyBytes);
			string authIV = Convert.ToBase64String(IVBytes);

			string validTest = Convert.ToBase64String(EncryptStringToBytes(VALID_TEXT, keyBytes, IVBytes));
			PlayerPrefs.SetString("ValidTest", validTest);

			PlayerPrefs.SetString("AuthKey", authKey);
			PlayerPrefs.SetString("AuthIV", authIV);
		}
		// Encrypt progress data with new keys
		StaticLevelData.ResetLevelData();
	}

	// Tests decryption using given keys
	private static bool TestKeys() {
		byte[] validBytes = Convert.FromBase64String(PlayerPrefs.GetString("ValidTest"));
		byte[] keyBytes = Convert.FromBase64String(PlayerPrefs.GetString("AuthKey"));
		byte[] IVBytes = Convert.FromBase64String(PlayerPrefs.GetString("AuthIV"));

		string decrypted = DecryptStringFromBytes(validBytes, keyBytes, IVBytes);
		bool success = decrypted == VALID_TEXT;
		return success;
	}

	// Encrypts string using saved auth keys
	private static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV) {
		// Check arguments.
		if (plainText == null || plainText.Length <= 0) {
			Debug.Log("ArgumentNullException(plainText)");
			return new byte[0];
		}
		if (Key == null || Key.Length <= 0) {
			Debug.Log("ArgumentNullException(Key)");
			return new byte[0];
		}
		if (IV == null || IV.Length <= 0) {
			Debug.Log("ArgumentNullException(IV)");
			return new byte[0];
		}
		byte[] encrypted;
		// Create an RijndaelManaged object
		// with the specified key and IV.
		using (RijndaelManaged rijAlg = new RijndaelManaged()) {
			rijAlg.Key = Key;
			rijAlg.IV = IV;

			// Create an encryptor to perform the stream transform.
			ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

			// Create the streams used for encryption.
			using (MemoryStream msEncrypt = new MemoryStream()) {
				using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
					using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {
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

	// Decrypts string using saved auth keys
	private static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV) {
		// Check arguments.
		if (cipherText == null || cipherText.Length <= 0) {
			Debug.Log("ArgumentNullException(cipherText)");
			return "";
		}
		if (Key == null || Key.Length <= 0) {
			Debug.Log("ArgumentNullException(Key)");
			return "";
		}
		if (IV == null || IV.Length <= 0) {
			Debug.Log("ArgumentNullException(IV)");
			return "";
		}

		// Declare the string used to hold
		// the decrypted text.
		string plaintext = null;

		// Create an RijndaelManaged object
		// with the specified key and IV.
		using (RijndaelManaged rijAlg = new RijndaelManaged()) {
			rijAlg.Key = Key;
			rijAlg.IV = IV;

			// Create a decryptor to perform the stream transform.
			ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

			// Create the streams used for decryption.
			using (MemoryStream msDecrypt = new MemoryStream(cipherText)) {
				using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
					using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
						// Read the decrypted bytes from the decrypting stream
						// and place them in a string.
						plaintext = srDecrypt.ReadToEnd();
					}
				}
			}
		}
		// Return decrypted text
		return plaintext;
	}

}
