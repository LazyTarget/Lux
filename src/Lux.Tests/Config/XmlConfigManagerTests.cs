﻿using System;
using System.IO;
using System.Xml.Linq;
using Lux.Config;
using Lux.IO;
using Lux.Serialization.Xml;
using Lux.Xml;
using NUnit.Framework;

namespace Lux.Tests.Config
{
    [TestFixture]
    public class XmlConfigManagerTests : TestBase
    {
        private TestableXmlConfigManager GetSUT()
        {
            return new TestableXmlConfigManager();
        }


        #region Tests

        [TestCase]
        public void Load()
        {
            var sut = GetSUT();

            var source = new XmlConfigSource
            {
                Uri = new Uri("C:/app.config"),
                RootElementName = "Root",
            };
            var expected = new MyAppConfig
            {
                AppName = "MyAppName",
                AppVersion = "1.0.0.0",
            };
            SaveToFile(source, sut.FileSystem, expected);

            var canLoad = sut.CanLoad<MyAppConfig>(source);
            Assert.IsTrue(canLoad);

            var actual = sut.Load<MyAppConfig>(source);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AppName, actual.AppName);
            Assert.AreEqual(expected.AppVersion, actual.AppVersion);
        }

        #endregion


        #region Helpers

        private void SaveToFile(XmlConfigSource target, IFileSystem fileSystem, IXmlExportable exportable)
        {
            var xdocument = new XDocument();
            var rootElement = xdocument.GetOrCreateElement(target.RootElementName);
            exportable.Export(rootElement);

            var fileName = target.Uri.LocalPath;
            var dirPath = PathHelper.GetParent(fileName);
            if (!fileSystem.DirExists(dirPath))
                fileSystem.CreateDir(dirPath);
            using (var stream = fileSystem.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                xdocument.Save(stream);
                stream.Flush();
            }
        }

        #endregion


        #region Classes

        public class MyAppConfig : IConfig, IXmlConfigurable, IXmlExportable
        {
            public ConfigSource Source { get; set; }

            public string AppName { get; set; }
            public string AppVersion { get; set; }

            public void Configure(XElement element)
            {
                var elem = element.Element(nameof(AppName));
                if (elem != null)
                {
                    AppName = elem.Value;
                }

                elem = element.Element(nameof(AppVersion));
                if (elem != null)
                {
                    AppVersion = elem.Value;
                }
            }

            public void Export(XElement element)
            {
                element.GetOrCreateElement(nameof(AppName)).Value = AppName;
                element.GetOrCreateElement(nameof(AppVersion)).Value = AppVersion;
            }
        }


        public class TestableXmlConfigManager : XmlConfigManager
        {
            public readonly IFileSystem FileSystem = new MemoryFileSystem();


            protected override Stream GetStreamFromSource(ConfigSource source)
            {
                var xmlConfigSource = (XmlConfigSource) source;
                var fileName = xmlConfigSource.Uri.LocalPath;
                var exists = FileSystem.FileExists(fileName);
                Stream stream = null;
                if (exists)
                {
                    // todo: truncate?
                    stream = FileSystem.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                else
                {
                    stream = FileSystem.OpenFile(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                }
                //stream = base.GetStreamFromSource(source);
                return stream;
            }
        }

        #endregion

    }
}
