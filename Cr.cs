using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace budilnik
{
  
    class Cr
    {
        
        public byte[] SHA_256(byte[] data) // генератор хэша на основе SHA256
        {
            byte[] res = new byte[1];

            SHA256 SHA = new SHA256Managed();

            res = SHA.ComputeHash(data);
            return res;
        }
    
    }
} 
