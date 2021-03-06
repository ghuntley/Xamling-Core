﻿using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Contract.Services;

namespace XamlingCore.Portable.Net.Downloaders
{
    public class SimpleHttpHttpTransfer : ISimpleHttpTranferrer
    {
        private readonly IDeviceNetworkStatus _networkInformation;

        public SimpleHttpHttpTransfer(IDeviceNetworkStatus networkInformation)
        {
            _networkInformation = networkInformation;
        }

        public async Task<bool> UploadBytes(string url, byte[] data)
        {
            try
            {
                if (_networkInformation.QuickNetworkCheck())
                {
                    var httpClient = new HttpClient();
                    var message = new HttpRequestMessage(HttpMethod.Post, url);

                    var content = new ByteArrayContent(data);

                    message.Content = content;

                    var result = await httpClient.SendAsync(message);
                    return result.IsSuccessStatusCode;
                }
            }
            catch
            {
                Debug.WriteLine("WARNING: Could not upload: {0}", url);
            }

            return false;
        }


        public async Task<byte[]> DownloadBytes(string url)
        {
            try
            {
                if (_networkInformation.QuickNetworkCheck())
                {
                    var httpClient = new HttpClient();
                    var message = new HttpRequestMessage(HttpMethod.Get, url);

                    var result = await httpClient.SendAsync(message);

                    if (result.IsSuccessStatusCode)
                    {
                        var content = result.Content;
                        using (var s = await content.ReadAsStreamAsync())
                        {
                            if (s != null)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    await s.CopyToAsync(ms);
                                    return ms.ToArray();
                                }
                            }
                        }
                    }

                 
                }
            }
            catch
            {
                Debug.WriteLine("WARNING: Could not download: {0}", url);
            }

            return null;
        }
    }
}
