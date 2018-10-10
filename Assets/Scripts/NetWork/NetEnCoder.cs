using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetEnCoder
{

    static int secretKeyIndex = 0;

    public static byte[] EnCode(byte[] bytes)
    {
        var index = secretKeyIndex % NetSecretKey.enCodeSecretKey.Length;
        var password = BitConverter.GetBytes(NetSecretKey.enCodeSecretKey[index]);

        var length = bytes.Length;
        byte originalByte;
        byte encodedByte;
        byte ucByte = 0;
        var keyLength = password.Length;
        var keyIndex=0;
        for (var i = 0; i < length; i++)
        {
            keyIndex = i % keyLength;
            originalByte = bytes[i];
            encodedByte = (byte)(originalByte ^ password[keyIndex]);
            encodedByte ^= ucByte;
            ucByte += originalByte;
            encodedByte ^= (byte)i;
            encodedByte ^= 0x78;
            encodedByte ^= 0x05;
            encodedByte ^= 0x27;

            bytes[i] = encodedByte;
        }

        return bytes;
    }

    public static void ResetSecretKeyIndex()
    {
        secretKeyIndex = 0;
    }

}
