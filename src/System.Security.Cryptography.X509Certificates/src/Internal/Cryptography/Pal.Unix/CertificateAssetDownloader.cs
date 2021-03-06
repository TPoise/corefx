using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Internal.Cryptography.Pal
{
    internal static class CertificateAssetDownloader
    {
        private static unsafe Interop.libcurl.curl_unsafe_write_callback s_writeCallback = CurlWriteCallback;

        internal static X509Certificate2 DownloadCertificate(string uri, ref TimeSpan remainingDownloadTime)
        {
            byte[] data = DownloadAsset(uri, ref remainingDownloadTime);

            if (data == null)
            {
                return null;
            }

            try
            {
                return new X509Certificate2(data);
            }
            catch (CryptographicException)
            {
                return null;
            }
        }

        internal static unsafe byte[] DownloadAsset(string uri, ref TimeSpan remainingDownloadTime)
        {
            if (remainingDownloadTime <= TimeSpan.Zero)
            {
                return null;
            }

            List<byte[]> dataPieces = new List<byte[]>();

            using (Interop.libcurl.SafeCurlHandle curlHandle = Interop.libcurl.curl_easy_init())
            {
                GCHandle gcHandle = GCHandle.Alloc(dataPieces);

                try
                {
                    IntPtr dataHandlePtr = GCHandle.ToIntPtr(gcHandle);
                    Interop.libcurl.curl_easy_setopt(curlHandle, Interop.libcurl.CURLoption.CURLOPT_URL, uri);
                    Interop.libcurl.curl_easy_setopt(curlHandle, Interop.libcurl.CURLoption.CURLOPT_WRITEDATA, dataHandlePtr);
                    Interop.libcurl.curl_easy_setopt(curlHandle, Interop.libcurl.CURLoption.CURLOPT_WRITEFUNCTION, s_writeCallback);
                    Interop.libcurl.curl_easy_setopt(curlHandle, Interop.libcurl.CURLoption.CURLOPT_FOLLOWLOCATION, 1L);

                    Stopwatch stopwatch = Stopwatch.StartNew();
                    int res = Interop.libcurl.curl_easy_perform(curlHandle);
                    stopwatch.Stop();

                    // TimeSpan.Zero isn't a worrisome value on the subtraction, it only
                    // means "no limit" on the original input.
                    remainingDownloadTime -= stopwatch.Elapsed;

                    if (res != Interop.libcurl.CURLcode.CURLE_OK)
                    {
                        return null;
                    }
                }
                finally
                {
                    gcHandle.Free();
                }
            }

            if (dataPieces.Count == 0)
            {
                return null;
            }

            if (dataPieces.Count == 1)
            {
                return dataPieces[0];
            }

            int dataLen = 0;

            for (int i = 0; i < dataPieces.Count; i++)
            {
                dataLen += dataPieces[i].Length;
            }

            byte[] data = new byte[dataLen];
            int offset = 0;

            for (int i = 0; i < dataPieces.Count; i++)
            {
                byte[] piece = dataPieces[i];

                Buffer.BlockCopy(piece, 0, data, offset, piece.Length);
                offset += piece.Length;
            }

            return data;
        }

        private static unsafe ulong CurlWriteCallback(byte* buffer, ulong size, ulong nitems, IntPtr context)
        {
            ulong totalSize = size * nitems;

            if (totalSize == 0)
            {
                return 0;
            }

            GCHandle gcHandle = GCHandle.FromIntPtr(context);
            List<byte[]> dataPieces = (List<byte[]>)gcHandle.Target;
            byte[] piece = new byte[totalSize];

            Marshal.Copy((IntPtr)buffer, piece, 0, (int)totalSize);
            dataPieces.Add(piece);

            return totalSize;
        }
    }
}
