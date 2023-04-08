using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using VTS;
using VTS.Models;
using VTS.Models.Impl;
using VTS.Networking;
using VTS.Networking.Impl;

namespace VTubeStudioAccess
{
    public class VTSAccess: VTSPlugin
    {
        private static VTSAccess instance = null;
        public static VTSAccess Instance
        {
            get
            {
                try
                {
                    if (instance == null)
                    {
                        instance = new VTSAccess();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }

                return instance;
            }
        }

        private CancellationTokenSource cancelSource;
        
        private VTSAccess()
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string settingsFolder = Path.Combine(appDataFolder, "VTSPlugins");
            
            this._pluginAuthor = "Ganeesya";
            this._pluginName = "parameterLogger";
            this.Initialize(
                new VTSWebSocket(),
                new WebSocketImpl(),
                new JsonUtilityImpl(),
                new TokenStorageImpl(settingsFolder),
                () =>
                {
                    DebugPrint("VTSAccess@ Connect");
                },
                () =>
                {
                    DebugPrint("VTSAccess@ DisConnect");
                },
                () =>
                {
                    DebugPrint("VTSAccess@ ConnectError");
                });
            cancelSource = new CancellationTokenSource();
            Task.Run(SocketUpdate, cancelSource.Token);
        }

        ~VTSAccess()
        {
            Close();
        }

        private VTSLive2DParameterListData old = null;
        private string currentLogFileName = "";
        private long tickCount = 0L;
        private float remainMsTime = 0f;
        private bool isInquiring = false;
        private bool isRecording = false;

        public void StockParameterFromVTS( string logBaseDirPath, float fps )
        {
            isRecording = true;
            if (isInquiring) return;

            isInquiring = true;
            GetLive2DParameterList(data =>
            {
                if (!isRecording) return;
                
                if (old?.data.modelID != data.data.modelID)
                {
                    DebugPrint(old?.data.modelID + " != " + data.data.modelID);
                    StartLogFile(logBaseDirPath,fps,data);
                }

                SaveParam(currentLogFileName, 1000f / fps, data);
                isInquiring = false;
            }, error =>
            {
                isInquiring = false;
            });
        }

        // todo クラス化
        private void StartLogFile(string logBaseDirPath, float fps,VTSLive2DParameterListData newData)
        {
            string dirPath = Path.Combine(logBaseDirPath, newData.data.modelName);
            if (!File.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string stumpString = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            currentLogFileName = Path.Combine(dirPath, stumpString + ".mtb");
            tickCount = 0L;
            remainMsTime =  - (1000f / fps);
            old = DeepCopy(newData);
            SaveHeader(currentLogFileName, fps, newData);
        }

        private void SaveHeader(string filePath, float fps,VTSLive2DParameterListData newData)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            var buffFile = new BinaryWriter(fs);
            buffFile.Write('M');
            buffFile.Write('T');
            buffFile.Write('B');
            buffFile.Write( (char)0 );
            buffFile.Write(fps);
            buffFile.Write(0L);
            
            buffFile.Write(newData.data.parameters.Length);
            buffFile.Write(0);
            
            foreach (var ele in newData.data.parameters)
            {
                buffFile.Write(ele.name);
            }

            buffFile.Close();
            fs.Close();
        }

        private void SaveParam(string filePath, float tickStepTime, VTSLive2DParameterListData newData)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write);
            var buffFile = new BinaryWriter(fs);
            buffFile.Seek(0,SeekOrigin.End);
            TimeSpan  totalTime = TimeSpan.FromMilliseconds(tickCount * tickStepTime);
            DebugPrint(totalTime.ToString("hh\\:mm\\:ss"));
            DebugPrint("stampDiff:" + (newData.timestamp - old.timestamp) + " tick:" + tickCount + " remain:" + remainMsTime);
            bool update = false;
            float diffTime = (float) (newData.timestamp - old.timestamp);
            while ( remainMsTime + tickStepTime <= diffTime )
            {
                float t = remainMsTime / diffTime;
                if (diffTime == 0L) t = 1f;
                foreach (var i in Enumerable.Range(0,newData.data.parameters.Length))
                {
                    float value = old.data.parameters[i].value * (1f-t) + newData.data.parameters[i].value * t; 
                    buffFile.Write(value);
                }
                
                DebugPrint( " t:" + t + " tick:" + tickCount + " remain:" + remainMsTime);

                remainMsTime += tickStepTime;
                tickCount++;
                update = true;
            }

            if (update)
            {
                DebugPrint( "updateTick!!!");
                remainMsTime -= diffTime;
                buffFile.Seek(1 * 4 + sizeof(float), SeekOrigin.Begin);
                buffFile.Write(tickCount);
                DebugPrint( "save tick:" + tickCount);
                old = DeepCopy(newData);
            }

            buffFile.Close();
            fs.Close();
        }

        public void ResetRecord()
        {
            isRecording = false;
            old = null;
            currentLogFileName = "";
            tickCount = 0L;
            remainMsTime = 0f;
        }

        private void DebugPrint(string str )
        {
            //Debug.Print(str);
        }
        
        public static T DeepCopy<T>(T obj) {
            using (var stream = new MemoryStream()) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
        
        private async Task SocketUpdate()
        {
            try
            {
                while (true)
                {
                    Socket.Update();
                    await Task.Delay(2);
                }
            }
            catch (Exception e)
            {
                DebugPrint(e.Message);
            }
        }

        public void Close()
        {
            cancelSource.Cancel();
            this.Socket.Close();
        }
    }
}