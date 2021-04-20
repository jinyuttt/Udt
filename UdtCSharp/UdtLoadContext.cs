using IUdtSocket;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace UdtCSharp
{
    public   class UdtLoadContext : AssemblyLoadContext
    {
        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (assemblyName.Name == managedAssemblyName)
            {
                // Try framework-specific managed assembly path
                var path = Path.Combine(assemblyPath, "runtimes", "any", "lib",
#if NETSTANDARD1_5
                    "netstandard1.5"
#elif NETSTANDARD2_0
                    "netstandard2.0"
#elif NET5_0
                    "net5.0"
#else
#error "Unsupported framework?"
#endif
                    , managedAssemblyName + ".dll");
                if (!File.Exists(path))
                {
                    // Try the same directory
                    path = Path.Combine(assemblyPath, managedAssemblyName + ".dll");
                }
                return LoadFromAssemblyPath(path);
            }
            // Defer to default AssemblyLoadContext
            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            if (unmanagedDllName == "udt")
            {
#if NETSTANDARD2_0
                bool is64bit = Environment.Is64BitProcess;
#else
                bool is64bit = (IntPtr.Size == 8);
#endif
                string arch = string.Empty;
                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.Arm64: arch = "-arm64"; break;
                    case Architecture.Arm: arch = "-arm"; break;
                    default: arch = is64bit ? "-x64" : "-x86"; break;
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    var fullPath = Path.Combine(assemblyPath, "runtimes", "osx" + arch, "native", "libnng.dylib");
                    return LoadUnmanagedDllFromPath(fullPath);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    var fullPath = Path.Combine(assemblyPath, "runtimes", "linux" + arch, "native", "libnng.so");
                    return LoadUnmanagedDllFromPath(fullPath);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var fullPath = Path.Combine(assemblyPath, "runtimes", "win" + arch, "native", "nng.dll");
                    return LoadUnmanagedDllFromPath(fullPath);
                }
                else
                {
                    throw new Exception("Unexpected runtime OS platform: " + RuntimeInformation.OSDescription);
                }
            }
            return IntPtr.Zero;
        }

        internal static UdtFactory Init(UdtLoadContext loadContext)
        {
            var assem = loadContext.LoadFromAssemblyName(new AssemblyName(managedAssemblyName));
            var type = assem.GetType("IUdtSocket.UdtFactory");
            UdtFactory udt = Activator.CreateInstance<UdtFactory>();
            return udt;
        }

        const string managedAssemblyName = "udt.NetCore";
        readonly string assemblyPath;
        private AssemblyDependencyResolver _resolver;

        public UdtLoadContext(string managedAssemblyPath)
        {
            this.assemblyPath = managedAssemblyPath;
            this.ResolvingUnmanagedDll += PluginLoadContext_ResolvingUnmanagedDll;
            this.Resolving += PluginLoadContext_Resolving;
            //第1步,解析des.json文件,并调用Load和LoadUnmanagedDll函数
            _resolver = new AssemblyDependencyResolver(assemblyPath);
        }

        private Assembly PluginLoadContext_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
        {
            return null;
        }

        private IntPtr PluginLoadContext_ResolvingUnmanagedDll(Assembly arg1, string arg2)
        {
            return IntPtr.Zero;
        }
    }
}
