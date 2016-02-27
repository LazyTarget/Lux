using System;
using Lux.Model;
using Lux.Tests.Model.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Lux.Tests.Model.ModelTests
{
    [TestFixture]
    public class MirrorObjectModelTests : ObjectModelTestBase
    {
        protected override IObjectModel CreateObjectModel(object args)
        {
            var objectModel = new MirrorObjectModel(args);
            return objectModel;
        }
        
    }
}
