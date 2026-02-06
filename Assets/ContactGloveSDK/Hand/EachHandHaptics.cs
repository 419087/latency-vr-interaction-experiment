using System;
using System.Collections.Generic;
using UnityEngine;

namespace ContactGloveSDK
{
    internal class EachFingerHaptics
    {
        private static readonly int DEFAULT_HAPTICS_STRENGTH = 15;
        private static readonly int SMABitsLength = DataLength.CollisionNum / 2;
        private bool[] hapticsDataInBits = new bool[DataLength.EachHandHapticsDataBytes * 8];
   	    private int[] setHapticsOnCount = new int[DataLength.EachHandHapticsDataBytes * 8];
   	    
        //private int currentThumbHapticsType = 0; // 0 ~ 15 (0: No Haptic)
        private int currentBackHapticsType = 0; // 0 ~ 15 (0: No Haptic)
        private int fingerHapticsStrength = DEFAULT_HAPTICS_STRENGTH;

        /// <summary>
        /// Return the bit index of the finger's haptic modules
        /// </summary>
        /// <param name="section">Finger section's name defined in enum FingerSectionNames</param>
        /// <param name="collider">Collider's name defined in enum ColliderNames</param>
        /// <returns></returns>
        private static int GetFirstBitIndexOfHapticModule(ColliderFinger_e section)
        {
            if(!HapticModulesExistIn(section))
            {
                return -1;
            }
            int index = 0;
            foreach (ColliderFinger_e fingerSection in Enum.GetValues(typeof(ColliderFinger_e)))
            {
                if (fingerSection == section)
                {
                    break;
                }
                index += fingerSection.ColliderNum();
            }
            return index;
        }

        private static int GetBitIndexOfHapticModule(ColliderFinger_e section, HapticModules collider)
        {
            if(!HapticModulesExistIn(section))
            {
                return -1;
            }
            if(!IsAvailableHapticModule(section, collider))
            {
                return -1;
            }
            int index = 0;
            foreach (ColliderFinger_e fingerSection in Enum.GetValues(typeof(ColliderFinger_e)))
            {
                if (fingerSection == section)
                {
                    index += GetLocalIndexOfHapticModule(section, collider);
                    break;
                }
                index += fingerSection.ColliderNum();
            }
            return index;
        }

        private static bool HapticModulesExistIn(ColliderFinger_e section)
        {
            // Haptic Modules are available only for Index, Middle, and Ring Distal.
            // return section.ColliderNum() == 4 && section != ColliderFinger_e.ThumbDistal;
            return section == ColliderFinger_e.ThumbDistal ||
                   section == ColliderFinger_e.IndexDistal ||
                   section == ColliderFinger_e.MiddleDistal ||
                   section == ColliderFinger_e.RingDistal;
        }

        private static bool IsAvailableHapticModule(ColliderFinger_e section, HapticModules collider)
        {
            /*
            return  collider != ColliderNames.intermediateLeft && 
                    collider != ColliderNames.intermediateRight;
            */
            return GetLocalIndexOfHapticModule(section, collider) >= 0;
        } 

        private static int GetLocalIndexOfHapticModule(ColliderFinger_e section, HapticModules collider)
        {
            switch(collider)
            {
                case HapticModules.topLeft:
                    return 0;
                case HapticModules.bottomLeft:
                    return 1;
                case HapticModules.topRight:
                    return 2;
                case HapticModules.bottomRight:
                    return 3;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// If the index specified by the argument is out of range, return false.
        /// </summary>
        /// <param name="index">the bit index of the collider</param>
        /// <returns></returns>
        private bool IsLegalBitIndex(int index)
        {
            return index >= 0 && index < SMABitsLength;
        }

        private static bool IsLegalBackHapticsType(int backHapticsType)
        {
            return backHapticsType >= 0 && backHapticsType < DataLength.BackHapticsTypeNum;
        }

        private static bool IsLegalFingerHapticsStrength(int strength)
        {
            return strength >= 0 && strength < DataLength.MaxFingerHapticsStength;
        }

        private bool SetHapticsOn(int index)
        {
            if(!IsLegalBitIndex(index))
            {
                return false;
            }

            bool update = false;
            if(setHapticsOnCount[index] == 0)
                update = true;
            setHapticsOnCount[index] ++;
            return update;
        }

        private bool SetHapticsOff(int index)
        {
            if(!IsLegalBitIndex(index))
            {
                return false;
            }

            setHapticsOnCount[index] --;
            if (setHapticsOnCount[index] > 0) 
                return false;
            if (setHapticsOnCount[index] == 0)
                return true;
            setHapticsOnCount[index] = 0;
            return false;
        }

        public bool SetHapticsOn(ColliderFinger_e section, HapticModules collider)
        {
            int index = GetBitIndexOfHapticModule(section, collider);
            return SetHapticsOn(index);
        }

        public bool SetHapticsOn(ColliderFinger_e section, HapticModules collider, int strength)
        {
            return SetHapticsOn(section, collider);
        }

        public bool SetHapticsOff(ColliderFinger_e section, HapticModules collider)
        {
            int index = GetBitIndexOfHapticModule(section, collider);
            return SetHapticsOff(index);
        }

        public bool SetWholeHapticsOn(ColliderFinger_e section)
        {
            if(!HapticModulesExistIn(section))
            {
                return false;
            }

            bool update = false;
            
            // Get the first bit index of the finger's haptic modules. Example: Thumb: 0
            int startIndex = GetFirstBitIndexOfHapticModule(section);
            for(int i = 0; i < section.ColliderNum(); i++)
            {
                int index = startIndex + i;
                bool localUpdate = SetHapticsOn(index);
                update = update || localUpdate;
            }
            return update;
        }

        public bool SetWholeHapticsOff(ColliderFinger_e section)
        {
            if(!HapticModulesExistIn(section))
            {
                return false;
            }
            bool update = false;
            int startIndex = GetFirstBitIndexOfHapticModule(section);
            for(int i = 0; i < section.ColliderNum(); i++)
            {
                int index = startIndex + i;
                bool localUpdate = SetHapticsOff(index);
                update = update || localUpdate;
            }
            return update;
        }

        public bool SetAllHapticsCountZero()
        {
            bool update = false;
            for(int i = 0; i < DataLength.EachHandHapticsDataBytes * 8; i++)
            {
                if(setHapticsOnCount[i] > 0)
                {
                    setHapticsOnCount[i] = 0;
                    update = true;
                }
            }
            return update;
        }

        public bool SetBackHapticsOn(int backHapticsType)
        {
            if (!IsLegalBackHapticsType(backHapticsType))
            {
                return false;
            }
            if (this.currentBackHapticsType == backHapticsType) 
            {
                return false;
            }
            this.currentBackHapticsType = backHapticsType;
            return true;
        }

        public bool SetBackHapticsOff()
        {
            return SetBackHapticsOn(0);
        }

        public bool SetFingerHapticsStrength(int strength)
        {
            if (!IsLegalFingerHapticsStrength(strength))
            {
                return false;
            }
            if(this.fingerHapticsStrength == strength)
            {
                return false;
            }
            this.fingerHapticsStrength = strength;
            return true;
        }

        public bool SetHapticsInBytes (byte[] hapticsData)
        {
            if(hapticsData.Length != DataLength.EachHandHapticsDataBytes)
            {
                return false;
            }
            return SetHapticsInBits(HapticsManager.ConvertBytesToBits(hapticsData));
        }

        public bool SetSMAHapticsInBits (bool[] bits)
        {
            if(bits.Length != SMABitsLength)
            {
                return false;
            }
            bool update = false;
            for(int i = 0; i < bits.Length; i++)
            {
                if(bits[i] != hapticsDataInBits[i])
                {
                    update = true;
                    hapticsDataInBits[i] = bits[i];
                }
            }
            return update;
        }

        public bool SetHapticsInBits (bool[] bits)
        {
            if(bits.Length != DataLength.EachHandHapticsDataBytes * 8)
            {
                return false;
            }
            bool update = false;
            for(int i = 0; i < bits.Length; i++)
            {
                if(bits[i] != hapticsDataInBits[i])
                {
                    update = true;
                    hapticsDataInBits[i] = bits[i];
                }
            }
            return update;
        }

        public bool SetHapticsInBytesClear ()
        {
            byte[] noHapticsInBytes = new byte[DataLength.EachHandHapticsDataBytes];
            //Array.Fill(noHapticsInBytes, 0);
            return SetHapticsInBytes(noHapticsInBytes);
        }

        public bool SetAllHapticsOff()
        {
            //bool thumbHapticsTypeUpdate = SetThumbHapticsOff();
            bool backHapticsTypeUpdate  = SetBackHapticsOff();
            bool hapticsInBytesUpdate   = SetHapticsInBytesClear();
            bool hapticsCountUpdate     = SetAllHapticsCountZero();
            return backHapticsTypeUpdate || hapticsInBytesUpdate || hapticsCountUpdate;
        }

        public byte[] GetBytes()
        {
            return HapticsManager.ConvertBitsToBytes(GetBits());
        }

        public bool[] GetBits()
        {
            return HapticsManager.ComposeBits(
                GetSMABits(),
                HapticsManager.ConvertIntToBits(GetBackHapticsType(), 4),
                HapticsManager.ConvertIntToBits(GetFingerHapticsStrength(), 4)
            );
        }

        private bool[] GetSMABits()
        {
            bool[] ret = new bool[SMABitsLength];
            for(int i = 0; i < ret.Length; i ++)
            {
                ret[i] = hapticsDataInBits[i] || setHapticsOnCount[i] > 0;
            }
            return ret;
        }

        public int GetBackHapticsType()
        {
            if(currentBackHapticsType > 0)
                return currentBackHapticsType;
            else
                return ExtractBackHapticType();
            
        }

        public bool GetHaptics(int index)
        {
            return hapticsDataInBits[index] || setHapticsOnCount[index] > 0;
        }

        public bool GetHaptics(ColliderFinger_e section, HapticModules collider)
        {
            int index = GetBitIndexOfHapticModule(section, collider);
            if(!IsLegalBitIndex(index))
            {
                return false;
            }
            return GetHaptics(index);
        }

        public int GetFingerHapticsStrength()
        {
            if(fingerHapticsStrength > 0)
                return fingerHapticsStrength;
            else
                return ExtractFingerHapticsStrength();
        }

        private int ExtractBackHapticType()
        {
            return HapticsManager.ConvertBitsToInt(hapticsDataInBits, SMABitsLength, SMABitsLength + 4);
        }

        private int ExtractFingerHapticsStrength()
        {
            return HapticsManager.ConvertBitsToInt(hapticsDataInBits, SMABitsLength + 4, SMABitsLength + 8);
        }
    }
}