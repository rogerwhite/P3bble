﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace P3bble.Core.Helper
{
    internal static class Util
    {
        private static DateTime _epoch = new DateTime(1970, 1, 1);
        
        public static DateTime AsDateTime(this int ts)
        {
            return _epoch.AddSeconds(ts);
        }

        public static int AsEpoch(this DateTime time)
        {
            return Convert.ToInt32((time - _epoch).TotalSeconds);
        }

        public static T AsStruct<T>(this Stream fs, out byte[] buffer) where T : struct
        {
            // Borrowed from http://stackoverflow.com/a/1936208 because BitConverter-ing all of this would be a pain
            buffer = new byte[Marshal.SizeOf(typeof(T))];
            fs.Read(buffer, 0, buffer.Length);
            return AsStruct<T>(buffer);
        }

        public static T AsStruct<T>(this byte[] bytes) where T : struct
        {
            if (bytes.Count() != Marshal.SizeOf(typeof(T)))
            {
                throw new ArgumentException("Byte array does not match size of target type.");
            }

            T ret;
            GCHandle hdl = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                ret = (T)Marshal.PtrToStructure(hdl.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                hdl.Free();
            }

            return ret;
        }

        public static Version AsVersion(this string version)
        {
            return new Version(version.Remove(0, 1));
        }
    }
}
