﻿
//using Shared;
using System;
using System.IO;
using System.Reflection;

namespace Fenix
{
    class Program
    {
        static void Main(string[] args)
        {
#if !UNITY_5_3_OR_NEWER

            var rootFolder = Directory.GetCurrentDirectory();
            //var clientDll = File.ReadAllBytes(Path.Combine(rootFolder, "../../../../../src/Client.App/bin/Debug/netcoreapp3.1/Client.App.dll"));
            //var serverDll = File.ReadAllBytes(Path.Combine(rootFolder, "../../../../../bin/netcoreapp3.1/Server.App.dll"));
            Assembly asmClientApp = Assembly.LoadFrom(Path.Combine(rootFolder, "../../../../../src/Client.App/bin/Debug/netcoreapp3.1/Client.App.dll"));
            Assembly asmServerApp = Assembly.LoadFrom(Path.Combine(rootFolder, "../../../../../bin/netcoreapp3.1/Server.App.dll"));
            //Assembly asmServerApp = typeof(Server.UModule.Avatar).Assembly;// Assembly.Load(serverDll);

            Assembly asmRuntime = Assembly.LoadFrom(Path.Combine(rootFolder, "../../../../../bin/netcoreapp3.1/Fenix.Runtime.dll"));
            //Assembly asmServerApp = typeof(Server.UModule.Avatar).Assembly;
            //Assembly asmClientApp = typeof(Client.Avatar).Assembly;

            //Assembly asmApp = typeof(ErrCode).Assembly;

            //string sharedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Shared/Gen");
            string clientPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Client.App");
            string serverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Server.App");

            string sharedClientPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Client.App/Gen"));
            string sharedServerPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Server.App/Gen"));

            Gen.AutogenHost(asmRuntime, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Fenix.Runtime/Common"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Client.App"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Server.App"));

            Gen.AutogenActor(asmServerApp, true, sharedClientPath, sharedServerPath, clientPath, serverPath); 
            Gen.AutogenActor(asmClientApp, false, sharedClientPath, sharedServerPath, clientPath, serverPath);

            //var p = new ProtocolCode();
            //p.Validate();

#else
            var rootFolder = Directory.GetCurrentDirectory();

            string rootPath = Path.Combine(Path.Combine(rootFolder, "../../../../../../../"));
            string unityPath = Path.Combine(rootPath, "Unity");

            var asmClientApp = Assembly.LoadFrom(Path.Combine(unityPath, "Library/ScriptAssemblies/Client.App.dll"));
            var asmServerApp = Assembly.LoadFrom(Path.Combine(rootPath, "bin/netcoreapp3.1/Server.App.dll"));
            var asmRuntime = Assembly.LoadFrom(Path.Combine(rootPath, "bin/netcoreapp3.1/Fenix.Runtime.dll"));

            string sharedClientPath = Path.GetFullPath(Path.Combine(unityPath, "Assets/Scripts/Client/Gen"));
            string sharedServerPath = Path.GetFullPath(Path.Combine(rootPath, "src/Server.App/Gen"));

            string clientPath = Path.GetFullPath(Path.Combine(unityPath, "Assets/Scripts/Client"));
            string serverPath = Path.GetFullPath(Path.Combine(rootPath, "src/Server.App"));

            Gen.AutogenHost(asmRuntime, Path.GetFullPath(Path.Combine(rootPath, "src/Fenix.Runtime/Common")), clientPath, serverPath);
             
            Gen.AutogenActor(asmServerApp, true, sharedClientPath, sharedServerPath, clientPath, serverPath);

            Assembly.LoadFrom(Path.Combine(unityPath, "Assets/Plugins/Fenix/Fenix.Runtime.Unity.dll"));
            Assembly.LoadFrom(Path.Combine(unityPath, "Assets/Plugins/Fenix/Shared.Unity.dll"));

            Gen.AutogenActor(asmClientApp, false, sharedClientPath, sharedServerPath, clientPath, serverPath);
            
            //var p = new ProtocolCode();
            //p.Validate();
#endif
        }
    }
}
