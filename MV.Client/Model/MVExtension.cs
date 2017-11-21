﻿using System;
using System.Threading;

namespace MV.Client.Model
{
    public static class MVExtension
    {
        public static void WaitCompleted(this string response, Func<string, string> func, string cmd)
        {
            while (true)
            {
                if (response.EndsWith("Vertex>"))
                {
                    break;
                }
                Thread.Sleep(1);
                response = func(cmd);
            }
        }
    }
}
