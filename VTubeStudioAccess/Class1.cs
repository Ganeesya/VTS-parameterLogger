using System.Text;

namespace VTubeStudioAccess
{
    public class VTubeStudioAccess
    {
        [DllExport]
        static void StockParameterFromVTS(string logBaseDirPath, float fps)
        {
            string encordedPath = Encoding.UTF8.GetString( Encoding.GetEncoding("Shift_JIS").GetBytes(logBaseDirPath) );
            // Debug.Print("preEncode path:"+logBaseDirPath);
            // Debug.Print("encode path:"+encordedPath);
            VTSAccess.Instance.StockParameterFromVTS(encordedPath,  fps);
        }

        [DllExport]
        static void StopRecord()
        {
            VTSAccess.Instance.ResetRecord();
        }
    }
}