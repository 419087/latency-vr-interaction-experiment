using UnityEngine;
using System;

internal static class DataConverter
{
    /// <summary>
    /// Convert Byte array to ushort array
    /// </summary>
    /// <param name="bytes"> input byte array</param>
    /// <param name="array"> result array with your ushort type</param>
    internal static void ByteToUshortArray (byte[] bytes, ref ushort[] convertedArray) 
    {
        // byte length of T
        int typeSize = sizeof(ushort);

        // if the array is not the multiple of T byte size, throw exception
        if(bytes.Length % typeSize != 0){
            Debug.LogError("The array is not the multiple of T byte size");
            return;
        }

        // Change type of array by shifting the bytes
        convertedArray = new ushort[bytes.Length/typeSize];
        for (int i = 0; i < bytes.Length/typeSize; ++i)
        {
            byte[] temp = new byte[typeSize];
            for(int j = 0; j < typeSize; ++j){
                temp[j] = (bytes[i * typeSize + j]);
            }
            convertedArray[i] = BitConverter.ToUInt16(temp, 0);
        }
    }
    
    /// <summary>
    /// Convert Byte array to float array
    /// </summary>
    /// <param name="bytes"> input byte array</param>
    /// <param name="array"> result array with float</param>
    internal static void ToFloatArray (this byte[] bytes, ref float[] convertedArray) 
    {
        // byte length of T
        int typeSize = sizeof(float);

        // if the array is not the multiple of T byte size, throw exception
        if(bytes.Length % typeSize != 0){
            Debug.LogError("The array is not the multiple of T byte size");
            return;
        }

        // Change type of array by shifting the bytes
        convertedArray = new float[bytes.Length/typeSize];
        for (int i = 0; i < bytes.Length/typeSize; ++i)
        {
            byte[] temp = new byte[typeSize];
            for(int j = 0; j < typeSize; ++j){
                temp[j] = (bytes[i * typeSize + j]);
            }
            convertedArray[i] = BitConverter.ToSingle(temp, 0);
        }
    }

    /// <summary>
    /// convert float array to byte array 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="convertedArray"></param>
    internal static void FloatToByteArray(float[] data, ref byte[] convertedArray)
    {
        // byte length of T
        int typeSize = sizeof(float);

        // if the array is not the multiple of T byte size, throw exception
        if(data.Length % typeSize != 0){
            Debug.LogError("The array is not the multiple of T byte size");
            return;
        }

        // Change type of array by shifting the bytes
        convertedArray = new byte[data.Length*typeSize];
        for (int i = 0; i < data.Length; ++i)
        {
            byte[] temp = BitConverter.GetBytes(data[i]);
            for(int j = 0; j < typeSize; ++j){
                convertedArray[i * typeSize + j] = temp[j];
            }
        }
    }
}

