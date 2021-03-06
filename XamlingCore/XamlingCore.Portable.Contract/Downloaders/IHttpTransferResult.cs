﻿using System;
using System.Net;

namespace XamlingCore.Portable.Contract.Downloaders
{
    public interface IHttpTransferResult
    {
        string Result { get; set; }
        HttpStatusCode HttpStatusCode { get; set; }
        Exception DownloadException { get; set; }
        bool IsSuccessCode { get; }
    }
}