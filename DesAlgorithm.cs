using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace des_algorithm
{
    class DesAlgorithm
    {
        private List<List<byte>> keys;

        public DesAlgorithm()
        {
            keys = GenerateKeys();
        }

        public void Encrypt(String dataString)
        {
            List<Byte> byteData = ConvertStringToByteList(dataString);
            if (byteData.Count % 8 != 0)
            {
                int addBytesCount = 8 - (byteData.Count % 8);
                for (int i=0; i!= addBytesCount; ++i)
                {
                    byteData.Add((byte)0);
                }
            }

            int chunks64count = byteData.Count / 8;
            List<List<byte>> transposedDataChunks = new List<List<byte>>();
            for(int i=0; i!=chunks64count; ++i)
            {
                List<byte> chunkRange = byteData.GetRange(i, 8);
                transposedDataChunks.Add(TransformByteListWithRule(chunkRange, DesAlgorithmConstants.PTranspositionMatrix));
            }
        }

        private static List<byte> TransformByteListWithRule(List<byte> data, List<int> transposeRule, int transposedDataSize = 0)
        {
            if (transposeRule.Max() > 8 * data.Count)
            {
                throw new Exception("Wrong transpose rule");
            }

            if (transposedDataSize <= 0)
            {
                transposedDataSize = data.Count;
            }
            List<byte> transposedData = new List<byte>();
            transposedData.AddRange(Enumerable.Repeat((byte)0x00, transposedDataSize).ToList<byte>());

            for(int i =0; i!= transposeRule.Count; ++i)
            {
                int bitNo = transposeRule[i] - 1;
                int byteNo = bitNo / 8;
                byte startShiftValue = (byte)(8 - bitNo % 8);
                byte endShiftValue = (byte)(8 - i % 8);
                byte targetByte = (byte)(i / 8);

                if(byteNo >= data.Count)
                {
                    throw new Exception("Data length is small");
                }

                byte shiftedData = GetShiftedBitValue(data[byteNo], startShiftValue, endShiftValue);

                transposedData[targetByte] = (byte)(transposedData[targetByte] | shiftedData);
            }

            return transposedData;
        }

        private static byte GetShiftedBitValue(byte data, byte startShift, byte finalShift)
        {
            if (finalShift > startShift)
            {
                data = unchecked((byte)((data << (finalShift - startShift)) & 0xFF));
            } 
            else if (finalShift < startShift)
            {
                data = unchecked((byte)((data >> (startShift - finalShift)) & 0xFF));
            }

            byte bitMask = (byte)((0x01 << finalShift) & 0xFF);
            data = (byte)(data & bitMask); 

            return data;
        }

        private static List<byte> CircularShiftLeft(List<byte> data, byte shift, int endShiftPass)
        {
            if (shift > 7)
            {
                throw new Exception("Too big shift value for single byte");
            }

            // учет нулевых битов в конце

            List<byte> shiftedData = new List<byte>();
            shiftedData.AddRange(Enumerable.Repeat((byte)0x00, data.Count).ToList<byte>());

            for (int i=0; i!= shiftedData.Count; ++i)
            {
                int previousCycleByte = i != shiftedData.Count - 1 ? i + 1 : 0;
                shiftedData[i] = (byte)(data[i] << shift | previousCycleByte >> (8 - shift));
            }

            return shiftedData;
        }

        private List<Byte> ConvertStringToByteList(String sourceString)
        {
            return Encoding.Unicode.GetBytes(sourceString).ToList<Byte>();
        }

        private void DesFunction(List<byte> data, List<List<byte>> roundKeys)
        {
            if (data.Count != 8)
            {
                throw new Exception("Data length is wrong");
            }

            for (int i = 0; i != roundKeys.Count; ++i)
            {
                List<byte> leftPart = data.GetRange(0, 4);
                List<byte> rightPart = data.GetRange(4, 4);
                List<byte> encryptedRightPart = PerformEncryptionRound(rightPart, roundKeys[i]);

                for(var j = 0; j!= encryptedRightPart.Count; ++j)
                {
                    leftPart[j] = (byte)(leftPart[j] ^ encryptedRightPart[j]);
                }

                List<byte> roundResult = new List<byte>();
                roundResult.AddRange(rightPart);
                roundResult.AddRange(leftPart);
                data = roundResult.Select(b => b).ToList<byte>();
            }
        }

        private List<byte> PerformEncryptionRound(List<byte> data, List<byte> key)
        {
            if (data.Count != 4)
            {
                throw new Exception("Wrong key length");
            }

            const int extendedKeysCount = 8;
            List<byte> pBoxValues = new List<byte>();

            int readingByteNo = 0;
            int shift = 6;
            byte previuosReadByte = (byte)((data[extendedKeysCount - 1] << 1 | data[0] >> 7) & 0x03);
            for (int i=0; i!= extendedKeysCount; ++i)
            {
                shift -= 4;
                if (shift > 0)
                {
                    previuosReadByte = (byte)((data[readingByteNo] >> shift | previuosReadByte << 4) & 0x3F);
                }
                else
                {
                    int carryByte = readingByteNo + 1;
                    if (readingByteNo + 1 == extendedKeysCount)
                    {
                        carryByte = 0;
                    }
                    previuosReadByte = (byte)(((data[readingByteNo] << (-shift) & data[carryByte] >> (8 - shift)) | previuosReadByte << 4) & 0x3F);
                    readingByteNo++;
                }
                pBoxValues.Add(previuosReadByte);
            }

            // 48 бит 

            if (pBoxValues.Count != key.Count)
            {
                throw new Exception("Wrong data length");
            }

            for(int i=0; i!= pBoxValues.Count; ++i)
            {
                pBoxValues[i] = (byte)(pBoxValues[i] ^ key[i]);
            }

            List<byte> sBoxedByteList = new List<byte>();
            sBoxedByteList.AddRange(Enumerable.Repeat((byte)0x00, 4));

            const int vectorsCount = 8;

            // в 64 бита

            for(int i = 0; i!= vectorsCount; ++i)
            {
                int targetByteNo = i / 2;
                int targetBitShift = 4 * (1 - i % 2);
                
                byte sBoxRow = (byte)((pBoxValues[i] << 1 | pBoxValues[i] >> 7) & 0x03);
                byte sBoxColumn = (byte)((pBoxValues[i] >> 1) & 0xF);

                byte sBoxData = (byte)DesAlgorithmConstants.SBox[i][sBoxRow][sBoxColumn];
                sBoxedByteList[targetByteNo] = (byte)(sBoxedByteList[targetByteNo] & (sBoxData << targetBitShift));

            }

            return sBoxedByteList;
        }

        private static List<List<byte>> GenerateKeys()
        {
            Random rnd = new Random();
            Byte[] bArray = new Byte[64];
            rnd.NextBytes(bArray);
            List<byte> keySource = bArray.ToList<byte>();

            const int halfKeyLength = 28;
            const int fullKeyByteLength = 8;
            int shiftBlockPass = 8 - halfKeyLength % 8;

            List<byte> c0KeyPart = TransformByteListWithRule(keySource, DesAlgorithmConstants.ExtendedKeyReplacement[0], halfKeyLength);
            List<byte> d0KeyPart = TransformByteListWithRule(keySource, DesAlgorithmConstants.ExtendedKeyReplacement[1], halfKeyLength);

            const int totalRoundCount = 16;
            List<int> requiresHalfShifting = new List<int> { 0, 1, 8, 15 };

            List<List<byte>> roundKeys = new List<List<byte>>();
            for(byte i = 0, totalShift = 0; i!= totalRoundCount; ++i)
            {
                if (requiresHalfShifting.Contains(i))
                {
                    totalShift += 1;
                } 
                else
                {
                    totalShift += 2;
                }

                List<byte> c0ShiftedKeyPart = CircularShiftLeft(c0KeyPart, totalShift, shiftBlockPass);
                List<byte> d0ShiftedKeyPart = CircularShiftLeft(d0KeyPart, totalShift, shiftBlockPass);

                List<byte> rawRoundKey = new List<byte>(fullKeyByteLength);
                int notShiftedBytesCount = halfKeyLength / 8;
                for (int j =0; j!= notShiftedBytesCount; ++j)
                {
                    rawRoundKey[j] = c0ShiftedKeyPart[i];
                }

                rawRoundKey[notShiftedBytesCount] = (byte)(c0ShiftedKeyPart[notShiftedBytesCount] | d0ShiftedKeyPart[0] << 4);

                for (int j = 0; j != d0ShiftedKeyPart.Count - 1; ++j)
                {
                    rawRoundKey[notShiftedBytesCount + j] = (byte)(d0ShiftedKeyPart[j] << 4 | d0ShiftedKeyPart[j + 1] >> 7);
                }

                rawRoundKey[fullKeyByteLength - 1] = (byte)(d0ShiftedKeyPart[d0ShiftedKeyPart.Count - 1] << 4);

                List<byte> compressedRawKey = TransformByteListWithRule(rawRoundKey, DesAlgorithmConstants.KeyCompressionReplacement);

                roundKeys.Add(compressedRawKey);
            }

            return roundKeys;
        }
    }
}
