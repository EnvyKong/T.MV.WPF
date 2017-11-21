using System;
using System.Reflection;
using TopYoung.MV.Core;

namespace MV.Client.Model
{
    public static class VNAFactory
    {
        public static IVectorNetworkAnalyzer GetVNA(string ip, int portNum)
        {
            var vnaType = Type.GetType($"{MethodBase.GetCurrentMethod().DeclaringType.Namespace}.{AppConfigInfo.VNAType}");
            return Activator.CreateInstance(vnaType, ip, portNum) as IVectorNetworkAnalyzer;
        }
    }
}
