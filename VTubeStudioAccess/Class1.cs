using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using VTS.Models;

namespace VTubeStudioAccess
{
    public class VTubeStudioAccess
    {
        // // 文字列単体：引数
        // [DllExport]
        // static int GetStringLength([MarshalAs(UnmanagedType.LPStr)]string s)
        // {
        //     return s.Length;
        // }
        //
        // // 文字列：返値
        // [DllExport][return: MarshalAs(UnmanagedType.LPStr)]
        // static string GetCodeString()
        // {
        //     return "abcd".Replace("a","A");
        // }

        // [StructLayout(LayoutKind.Sequential)]
        // struct Parameter
        // {
        //     [MarshalAs(UnmanagedType.LPStr)]
        //     public string id;
        //
        //     public float value;
        //     public float max;
        //     public float min;
        // }
        
        private static VTSAccess access = new VTSAccess();
        
        // 構造体単体：返値
        // [DllExport]
        // static IntPtr GetParameter(int index)
        // {
        //     VTSParameter parameter = access.GetParameter(index);
        //     Parameter ret;
        //     ret.id = parameter.name;
        //     ret.value = parameter.value;
        //     ret.max = parameter.max;
        //     ret.min = parameter.min;
        //
        //     IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Parameter)));
        //     Marshal.StructureToPtr(ret, ptr, false);
        //     return ptr;
        // }
        //
        // [DllExport]
        // static int GetParameterCount()
        // {
        //     return access.getParameterCount();
        // }

        [DllExport]
        static void StockParameterFromVTS(string logBaseDirPath, float fps)
        {
            access.StockParameterFromVTS(logBaseDirPath,  fps);
        }

        [DllExport]
        static void StopRecord()
        {
            access.ResetRecord();
        }
    }
}