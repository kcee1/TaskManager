using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.AzureBlobService.Helpers
{
    public class BlobHelper
    {
        public string GetBlockId(int blockNumber)
        {
            return Convert.ToBase64String(BitConverter.GetBytes(blockNumber));
        }

        public string[] GetBlockList(int blockCount)
        {
            string[] blockList = new string[blockCount];
            for (int i = 0; i < blockCount; i++)
            {
                blockList[i] = GetBlockId(i);
            }
            return blockList;
        }
    }
}
