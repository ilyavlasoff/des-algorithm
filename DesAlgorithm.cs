using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace des_algorithm
{
    class DesAlgorithm
    {
        const int ulongMaxLength = 64;
        private List<ulong> keys;
        public enum Operation { encrypt, decrypt }

        public DesAlgorithm(bool useCustomKey, ulong customKey = 0)
        {
            keys = GenerateKeys(useCustomKey, customKey);
        }

        public List<Byte> Des(List<Byte> byteData, Operation operation)
        {
            if (byteData.Count % 8 != 0)
            {
                int addBytesCount = 8 - (byteData.Count % 8);
                for (int i=0; i!= addBytesCount; ++i)
                {
                    byteData.Add((byte)0);
                }
            }

            List<ulong> transposedDataChunks = new List<ulong>();
            for (var i =0; i!= byteData.Count / 8; ++i)
            {
                ulong u64chunk = 0;
                List<Byte> chunk = byteData.GetRange(i * 8, 8);
                for (var j =0; j!= chunk.Count; ++j)
                {
                    u64chunk = u64chunk | (ulong)chunk[j] << (56 - j * 8);
                }
                transposedDataChunks.Add(u64chunk);
            }
            
            List<ulong> result;
            if (operation == Operation.encrypt)
            {
               result = DriveDes(transposedDataChunks, this.keys, false);
            }
            else
            {
                List<ulong> reversedKeys = this.keys;
                reversedKeys.Reverse();
                result = DriveDes(transposedDataChunks, reversedKeys, true);
            }
      
            List<byte> encodedBytes = new List<byte>();
            foreach (ulong value in result)
            {
                List<byte> byteBuffer = new List<byte>();
                for (int i = 0; i != 8; ++i)
                {
                    byteBuffer.Add((byte)(value >> (i * 8) & 0xFF));
                }
                byteBuffer.Reverse();
                encodedBytes.AddRange(byteBuffer);
            }

            return encodedBytes;
        }

        private static List<ulong> DriveDes(List<ulong> data, List<ulong> keys, bool reverse)
        {
            List<ulong> encryptedData = new List<ulong>();

            for (int i = 0; i != data.Count; ++i)
            {
                ulong startPermutedValue = TransformByteListWithRule(data[i], 63, DesAlgorithmConstants.PTranspositionMatrix, 63);
                ulong desEncryptedValue = DesFunction(startPermutedValue, keys, reverse);
                ulong endPermutedValue = TransformByteListWithRule(desEncryptedValue, 63, DesAlgorithmConstants.P1TranspositionMatrix, 63);
                encryptedData.Add(endPermutedValue);
            }

            return encryptedData;
        }

        private static ulong TransformByteListWithRule(ulong data, int startSourceShift, List<int> transposeRule, int startResultShift)
        {
            if (transposeRule.Max() > ulongMaxLength)
            {
                throw new Exception("Wrong transpose rule");
            }

            ulong transposedData = 0;

            for(int i =0; i!= transposeRule.Count; ++i)
            {
                ulong startSourceBit = (ulong)0x01 << startSourceShift;

                ulong coeff = (startSourceBit >> (transposeRule[i] - 1)) & data;

                int shift = (startResultShift - i) - (startSourceShift - (transposeRule[i] - 1));
                if (shift == 0) 
                {
                    transposedData = coeff | transposedData;
                }
                else if (shift < 0)
                {
                    transposedData = (coeff >> -shift) | transposedData;
                }
                else
                {
                    transposedData = (coeff << shift) | transposedData;
                }

            }

            return transposedData;
        }

        private static ulong DesFunction(ulong data, List<ulong> roundKeys, bool reverse)
        {
            ulong leftPart = data >> 32;
            ulong rightPart = data & (ulong.MaxValue >> 32);

            for (int i = 0; i != roundKeys.Count; ++i)
            {
                if(reverse)
                {
                    rightPart = rightPart ^ PerformEncryptionRound(leftPart, roundKeys[i]);
                } 
                else
                {
                    leftPart = leftPart ^ PerformEncryptionRound(rightPart, roundKeys[i]);
                }
                
                ulong tmp = rightPart;
                rightPart = leftPart;
                leftPart = tmp;
            }

            return leftPart << 32 | rightPart;
        }

        private static ulong PExtensionBox(ulong data)
        {
            ulong pExtendedValue = 0;
            int maskShift = 27;
            for (var i = 0; i != 8; ++i)
            {
                ulong mask = (ulong)0x3F;
                if (maskShift > 0)
                {
                    mask = mask << maskShift;
                }
                else
                {
                    mask = mask >> -maskShift;
                }
                mask = mask & 0xFFFFFFFF;
                ulong pBoxRow = data & mask;
                int extendedValueShift = 42 - (i * 6);
                int shift = extendedValueShift - maskShift;

                if (shift == 0)
                {
                    pExtendedValue = pExtendedValue | pBoxRow;
                }
                if (shift > 0)
                {
                    pExtendedValue = pExtendedValue | pBoxRow << shift;
                }
                else
                {
                    pExtendedValue = pExtendedValue | pBoxRow >> -shift;
                }

                maskShift -= 4;
            }

            pExtendedValue = pExtendedValue | (pExtendedValue >> 46) | ((pExtendedValue << 46) & 0xFFFFFFFFFFFF);

            return pExtendedValue;
            //return TransformByteListWithRule(data, 31, DesAlgorithmConstants.PExtension, 47);
        }

        private static ulong SBox(ulong data)
        {
            ulong SBoxedValue = 0;
            for(var i = 0; i!= 8; ++i)
            {
                int shift = 42 - i * 6;
                ulong sBoxData = (((ulong)0x3F << shift) & data) >> shift;
                uint sBoxRowNo = (uint)(((sBoxData >> 4) & 0x02) | (sBoxData & (ulong)0x01));
                uint sBoxColNo = (uint)((sBoxData >> 1) & 0x0F);
                ulong SBoxRes = Convert.ToUInt64(DesAlgorithmConstants.SBox[i][(int)sBoxRowNo][(int)sBoxColNo]) & 0x0F;

                SBoxedValue = SBoxedValue | (SBoxRes << (28 - i * 4));
            }

            return SBoxedValue;
        }

        private static ulong PerformEncryptionRound(ulong data, ulong key)
        {
            ulong pExtendedData = PExtensionBox(data);
            ulong pXExtendedData = pExtendedData ^ key;
            ulong SBoxData = SBox(pXExtendedData);
            return TransformByteListWithRule(SBoxData, 31, DesAlgorithmConstants.PBox, 31);
        }

        private static List<ulong> GenerateKeys(bool useCustomKeys, ulong customKey = 0)
        {
            ulong salt;
            if(useCustomKeys)
            {
                salt = customKey;
            }
            else
            {
                Random rnd = new Random();
                salt = (ulong)rnd.Next();
            }
           
            ulong c0KeyPart = TransformByteListWithRule(salt, 63, DesAlgorithmConstants.ExtendedKeyReplacement[0], 27);
            ulong d0KeyPart = TransformByteListWithRule(salt, 63, DesAlgorithmConstants.ExtendedKeyReplacement[1], 27);

            const int totalRoundCount = 16;
            List<int> requiresHalfShifting = new List<int> { 0, 1, 8, 15 };

            List<ulong> roundKeys = new List<ulong>();
            for(int i = 0; i!= totalRoundCount; ++i)
            {
                int totalShift = requiresHalfShifting.Contains(i) ? 1 : 2;

                c0KeyPart = CircularShiftLeft(c0KeyPart, totalShift, 27);
                d0KeyPart = CircularShiftLeft(d0KeyPart, totalShift, 27);

                ulong raw56Key = c0KeyPart << 28 | d0KeyPart;

                ulong roundKey = TransformByteListWithRule(raw56Key, 55, DesAlgorithmConstants.KeyCompressionReplacement, 47);

                roundKeys.Add(roundKey);
            }

            return roundKeys;
        }

        private static ulong CircularShiftLeft(ulong data, int shiftAmount, int borderValue)
        {
            if (shiftAmount >= borderValue)
            {
                throw new Exception("Too big shift value"); 
            }
            return (data << shiftAmount | data >> (borderValue - shiftAmount + 1)) & (ulong.MaxValue >> (64 - borderValue - 1));
        }
    }
}
