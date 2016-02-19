using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Internal;
using System.IO;
using System.Linq;
using System.Reflection;
using Lux.IO;

namespace Lux.Config
{
    public class LuxConfigHost : DelegatingConfigHost//, IInternalConfigClientHost
    {
        public LuxConfigHost()
            : this((IInternalConfigHost)ConfigSystemProxy._internalConfigClientHost)
        {
            
        }

        public LuxConfigHost(IInternalConfigHost internalConfigHost)
        {
            FileSystem = new FileSystem();
            if (ConfigSystemProxy.FileSystem != null)
                FileSystem = ConfigSystemProxy.FileSystem;
            Host = internalConfigHost;
        }
        
        public IFileSystem FileSystem { get; set; }


        public override bool IsTrustedConfigPath(string configPath)
        {
            bool res;
            res = configPath == LuxConfigurationManager.MachineConfigPath;
            //res = base.IsTrustedConfigPath(configPath);
            return res;
        }

        public override bool PrefetchSection(string sectionGroupName, string sectionName)
        {
            return base.PrefetchSection(sectionGroupName, sectionName);
        }

        public override bool PrefetchAll(string configPath, string streamName)
        {
            return base.PrefetchAll(configPath, streamName);
        }

        public override object StartMonitoringStreamForChanges(string streamName, StreamChangeCallback callback)
        {
            return base.StartMonitoringStreamForChanges(streamName, callback);
        }

        public override void StopMonitoringStreamForChanges(string streamName, StreamChangeCallback callback)
        {
            base.StopMonitoringStreamForChanges(streamName, callback);
        }

        public override object CreateConfigurationContext(string configPath, string locationSubPath)
        {
            return base.CreateConfigurationContext(configPath, locationSubPath);
        }

        public override object CreateDeprecatedConfigContext(string configPath)
        {
            return base.CreateDeprecatedConfigContext(configPath);
        }

        public override string DecryptSection(string encryptedXml, ProtectedConfigurationProvider protectionProvider,
            ProtectedConfigurationSection protectedConfigSection)
        {
            return base.DecryptSection(encryptedXml, protectionProvider, protectedConfigSection);
        }

        public override string EncryptSection(string clearTextXml, ProtectedConfigurationProvider protectionProvider,
            ProtectedConfigurationSection protectedConfigSection)
        {
            return base.EncryptSection(clearTextXml, protectionProvider, protectedConfigSection);
        }

        public override Type GetConfigType(string typeName, bool throwOnError)
        {
            return base.GetConfigType(typeName, throwOnError);
        }

        public override string GetConfigPathFromLocationSubPath(string configPath, string locationSubPath)
        {
            return base.GetConfigPathFromLocationSubPath(configPath, locationSubPath);
        }

        public override string GetConfigTypeName(Type t)
        {
            return base.GetConfigTypeName(t);
        }

        public override void DeleteStream(string streamName)
        {
            base.DeleteStream(streamName);
        }

        public override string GetStreamName(string configPath)
        {
            return base.GetStreamName(configPath);
        }

        public override string GetStreamNameForConfigSource(string streamName, string configSource)
        {
            return base.GetStreamNameForConfigSource(streamName, configSource);
        }

        public override object GetStreamVersion(string streamName)
        {
            return base.GetStreamVersion(streamName);
        }

        public override void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath,
            IInternalConfigRoot configRoot, params object[] hostInitConfigurationParams)
        {
            base.InitForConfiguration(ref locationSubPath, out configPath, out locationConfigPath, configRoot, hostInitConfigurationParams);
        }

        public override void Init(IInternalConfigRoot configRoot, params object[] hostInitParams)
        {
            base.Init(configRoot, hostInitParams);
        }

        public override bool IsFile(string streamName)
        {
            return base.IsFile(streamName);
        }

        public override Stream OpenStreamForRead(string streamName)
        {
            var stream = OpenStreamForRead(streamName, false);
            return stream;
        }

        public override Stream OpenStreamForRead(string streamName, bool assertPermissions)
        {
            Stream stream = null;
            //stream = base.OpenStreamForRead(streamName, assertPermissions);

            var fileName = streamName;
            if (FileSystem.FileExists(fileName))
            {
                var mode = FileMode.Open;
                var access = FileAccess.Read;
                var share = FileShare.ReadWrite;
                stream = FileSystem.OpenFile(fileName, mode, access, share);
            }

#if DEBUG
            if (stream != null)
            {
                var streamReader = new StreamReader(stream);
                var xml = streamReader.ReadToEnd();
                stream.Position = 0;
            }
#endif

            return stream;
        }

        public override Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext)
        {
            var stream = OpenStreamForWrite(streamName, templateStreamName, ref writeContext, false);
            return stream;
        }

        public override Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext, bool assertPermissions)
        {
            Stream stream = null;
            //stream = base.OpenStreamForWrite(streamName, templateStreamName, ref writeContext, assertPermissions);

            writeContext = null;

            var fileName = streamName;
            var writeContextType = Type.GetType("System.Configuration.Internal.WriteFileContext, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
            if (writeContextType != null)
            {
                var flags = BindingFlags.NonPublic | BindingFlags.CreateInstance;
                writeContext = Activator.CreateInstance(writeContextType, flags, null, new[] { fileName }, null);
            }

            if (FileSystem.FileExists(fileName))
            {
                var mode = FileMode.Open;
                var access = FileAccess.ReadWrite;
                var share = FileShare.ReadWrite;
                stream = FileSystem.OpenFile(fileName, mode, access, share);
            }

            return stream;

        }

            
        public bool IsExeConfig(string configPath)
        {
            throw new NotImplementedException();
        }

        public bool IsRoamingUserConfig(string configPath)
        {
            throw new NotImplementedException();
        }

        public bool IsLocalUserConfig(string configPath)
        {
            throw new NotImplementedException();
        }

        public string GetExeConfigPath()
        {
            throw new NotImplementedException();
        }

        public string GetRoamingUserConfigPath()
        {
            throw new NotImplementedException();
        }

        public string GetLocalUserConfigPath()
        {
            throw new NotImplementedException();
        }
    }
}
