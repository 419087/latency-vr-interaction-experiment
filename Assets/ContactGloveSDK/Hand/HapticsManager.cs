using System;
using System.Collections.Generic;
using UnityEngine;

namespace ContactGloveSDK
{
    public struct VibrationType
    {
        public float amplitude;
        public float frequency;

        public VibrationType(float amplitude, float frequency)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
        }
    }

    public class VibrationData
    {
        public readonly HandSides Hand;
        public readonly float Frequency, Amplitude, Duration;

        public VibrationData(HandSides hand, float amplitude, float frequency, float duration)
        {
            this.Hand = hand;
            this.Amplitude = amplitude;
            this.Frequency = frequency;
            this.Duration = duration;
        }
    }

    // There is only one instance of HapticsManager for each instance of ISerialCommunicator.
    public class HapticsManager
    {

        private EachFingerHaptics fingerHaptics_l = new EachFingerHaptics();
        private EachFingerHaptics fingerHaptics_r = new EachFingerHaptics();

        private float vibrationAmplitude_l = 0, vibrationAmplitude_r = 0;
        private float vibrationFrequency_l = 0, vibrationFrequency_r = 0;

        /// <summary>
        /// With reference to design, compose and return haptics signal in bytes.
        /// When design is changed, fix this function.
        /// </summary>
        /// <returns>haptics signal in bytes</returns>
        private byte[] ComposeAllHapticsBytes()
        {   
            return ConvertBitsToBytes(
                ComposeBits(
                    fingerHaptics_l.GetBits(),
                    fingerHaptics_r.GetBits()
                )
            );
        }

        public void SetHapticsOn(HandSides hand, ColliderFinger_e section, HapticModules collider)
        {
            if (hand == HandSides.Left)
                fingerHaptics_l.SetHapticsOn(section, collider);
            else 
                fingerHaptics_r.SetHapticsOn(section, collider);
        }

        public void SetHapticsOn(HandSides hand, ColliderFinger_e section, HapticModules collider, int strength)
        {
            if (hand == HandSides.Left)
                fingerHaptics_l.SetHapticsOn(section, collider, strength);
            else
                fingerHaptics_r.SetHapticsOn(section, collider, strength);
        }

        public void SetHapticsOff(HandSides hand, ColliderFinger_e section, HapticModules collider)
        {
            if(hand == HandSides.Left)
                fingerHaptics_l.SetHapticsOff(section, collider);
            else
                fingerHaptics_r.SetHapticsOff(section, collider);
        }

        public void SetWholeHapticsOn(HandSides hand, ColliderFinger_e section)
        {
            if(hand == HandSides.Left)
                fingerHaptics_l.SetWholeHapticsOn(section);
            else
                fingerHaptics_r.SetWholeHapticsOn(section);
        }

        public void SetWholeHapticsOff(HandSides hand, ColliderFinger_e section)
        {
            if(hand == HandSides.Left)
                fingerHaptics_l.SetWholeHapticsOff(section);
            else
                fingerHaptics_r.SetWholeHapticsOff(section);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hand">Left or Right</param>
        /// <param name="amplitude">0.0 ~ 1.0</param>
        /// <param name="frequency">Hz</param>
        public void SetVibration (HandSides hand, float amplitude, float frequency)
        {
            if (hand == HandSides.Left)
            {
                // fingerHaptics_l.SetBackHapticsOn(hapticsType);
                this.vibrationAmplitude_l = amplitude;
                this.vibrationFrequency_l = frequency;
            }
            else
            {
                // fingerHaptics_r.SetBackHapticsOn(hapticsType);
                this.vibrationAmplitude_r = amplitude;
                this.vibrationFrequency_r = frequency;
            }
        }

        public void SetBothBackHapticsOn (int leftBackHapticsType, int rightBackHapticsType)
        {
            fingerHaptics_l.SetBackHapticsOn(leftBackHapticsType );
            fingerHaptics_r.SetBackHapticsOn(rightBackHapticsType);
        }

        public void SetBackHapticsOff (HandSides hand)
        {
            if(hand == HandSides.Left)
                fingerHaptics_l.SetBackHapticsOff();
            else
                fingerHaptics_r.SetBackHapticsOff();
        }

        public void SetBothBackHapticsOff ()
        {
            fingerHaptics_l.SetBackHapticsOff();
            fingerHaptics_r.SetBackHapticsOff();
        }

        public void SetFingerHapticsStrength(HandSides hand, int strength)
        {
            if(hand == HandSides.Left)
                fingerHaptics_l.SetFingerHapticsStrength(strength);
            else
                fingerHaptics_r.SetFingerHapticsStrength(strength);
        }

        public void SetAllHapticsOff(HandSides hand)
        {
            if(hand == HandSides.Left)
                fingerHaptics_l.SetAllHapticsOff();
            else
                fingerHaptics_r.SetAllHapticsOff();
        }
        
        public void SetAllHapticsOff()
        {
            fingerHaptics_l.SetAllHapticsOff();
            fingerHaptics_r.SetAllHapticsOff();
        }

        public void SetSMAHapticsInBytes (byte[] bytes)
        {
            SetSMAHapticsInBits(ConvertBytesToBits(bytes));
        }

        public void SetSMAHapticsInBits (bool[] bits)
        {
            fingerHaptics_l.SetSMAHapticsInBits(ExtractBits(bits, 0, DataLength.CollisionNum / 2));
            fingerHaptics_r.SetSMAHapticsInBits(ExtractBits(bits, DataLength.CollisionNum / 2, DataLength.CollisionNum));
        }

        public bool GetHaptics(HandSides hand, ColliderFinger_e section, HapticModules collider)
        {
            if (hand == HandSides.Left)
                return fingerHaptics_l.GetHaptics(section, collider);
            else
                return fingerHaptics_r.GetHaptics(section, collider);
        }

        public VibrationType GetVibration(HandSides hand)
        {
            if (hand == HandSides.Left)
            {
                VibrationType tmp = new VibrationType(
                    this.vibrationAmplitude_l, 
                    this.vibrationFrequency_l
                );
                return tmp;
            }
            else
            {
                VibrationType tmp = new VibrationType(
                    this.vibrationAmplitude_r,
                    this.vibrationFrequency_r
                );
                return tmp;
            }
        }

        public int GetFingerHapticsStrength(HandSides hand)
        {
            if(hand == HandSides.Left)
                return fingerHaptics_l.GetFingerHapticsStrength();
            else
                return fingerHaptics_r.GetFingerHapticsStrength();
        }

        public byte[] GetHapticsBytes()
        {
            return ComposeAllHapticsBytes();
        }

        public byte[] GetVibrationBytes()
        {
            // amplitude converted
            byte amp_l_c = (byte) (this.vibrationAmplitude_l * 255);
            byte amp_r_c = (byte) (this.vibrationAmplitude_r * 255);
            
            // frequency converted
            byte freq_l_c = (byte)Mathf.Clamp(this.vibrationFrequency_l, 0.0f, 255.0f);
            byte freq_r_c = (byte)Mathf.Clamp(this.vibrationFrequency_r, 0.0f, 255.0f);

            byte[] sendData = { amp_l_c, freq_l_c, amp_r_c, freq_r_c };
            return sendData;
        }

        public static T[] ComposeArray<T>(params T[][] argsArr)
        {
            int sumLength = 0;
            foreach(T[] ts in argsArr)
            {
                sumLength += ts.Length;
            }
            
            T[] ret = new T[sumLength];
            int index = 0;

            foreach(T[] ts in argsArr)
            {
                foreach(T t in ts)
                {
                    ret[index] = t;
                    index ++;
                }
            }
            return ret;
        }      

        public static byte[] ComposeBytes(params byte[][] bytesArr)
        {
            return ComposeArray<byte>(bytesArr);
        }

        public static bool[] ComposeBits(params bool[][] bitsArr)
        {
            return ComposeArray<bool>(bitsArr);
        }

        public static bool[] ConvertBytesToBits(byte[] bytes)
        {
            bool[] ret = new bool[bytes.Length * 8];
            for(int i = 0; i < bytes.Length; i++) 
            {
                byte tmp = bytes[i];
                for(int j = 0; j < 8; j++)
                {
                    ret[i*8 + j] = (tmp & 1) == 1;
                    tmp = (byte)(tmp >> 1);
                }
            }
            return ret;
        }

        public static byte[] ConvertBitsToBytes(bool[] bits)
        {
            if(bits.Length % 8 != 0)
            {
                Debug.Log("ConvertBitsToBytes(): Length of bits is not a multiple of 8. Blanks are filled with 0.");
            }

            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            int bitIndex = 0;
            for (int byteIndex = 0; byteIndex < ret.Length; byteIndex ++)
            {
                byte data = 0;
                for (int i = 0; i < 8; i++)
                {
                    if(bitIndex >= bits.Length)
                        break;
                    data += (byte)(Convert.ToByte(bits[bitIndex]) << i);
                    bitIndex ++;
                }
                ret[byteIndex] = data;
            }
            //DebugTool.ShowArrayLog(ret);
            return ret;
        }


        // for example when num is 23 (10111) and bitLength is 8, return {1, 1, 1, 0, 1, 0, 0, 0}
        public static bool[] ConvertIntToBits(int num, int bitLength)
        {
            if(num < 0 || num >= (1 << bitLength))
            {
                Debug.LogError("The length of bit is out of range.");
                return new bool[]{};
            }
            bool[] ret = new bool[bitLength];
            for(int i = 0; i < bitLength; i ++)
            {
                ret[i] = (num & 1) == 1;
                num = num >> 1;
            }
            return ret;
        }

        // for example when ConvertBitsToInt({1, 1, 1, 0, 1, 0, 0, 0}, 1, 6) is called, return 11 (01011)
        public static int ConvertBitsToInt(bool[] bits, int start, int end)
        {
            if(start < 0 || end > bits.Length || start > end || end - start > 31)
            {
                Debug.LogError("The range of bits is illegal.");
                return -1;
            }
            int ret = 0;
            int digit = 0;
            for(int i = start; i < end; i ++)
            {
                ret += ((bits[i] ? 1 : 0) << digit);
                digit ++;
            }
            return ret;
        }

        public static T[] ExtractArray<T>(T[] array, int start, int end)
        {
            if(start < 0 || end > array.Length || start > end)
            {
                // Debug.LogError("The range is illegal.");
                return new T[]{};
            }
            T[] ret = new T[end - start];
            for(int i = start; i < end; i ++)
            {
                ret[i - start] = array[i];
            }
            return ret;
        }

        public static bool[] ExtractBits(bool[] bits, int start, int end)
        {
            return ExtractArray<bool>(bits, start, end);
        }

        public static byte[] ExtractBytes(byte[] bytes, int start, int end)
        {
            return ExtractArray<byte>(bytes, start, end);
        }
    }
}