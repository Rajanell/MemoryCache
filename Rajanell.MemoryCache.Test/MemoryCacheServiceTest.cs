using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rajanell.MemoryCache.Model;
using Rajanell.MemoryCache.Services;
using System;
using System.Threading.Tasks;

namespace Rajanell.Test
{
    [TestClass]
    public class MemoryCacheServiceTest
    {
        private IMemoryCacheService _configurationService;
        private IServiceProvider _serviceProvider;
        [TestInitialize]
        public void Initialize()
        {
            _serviceProvider = ServicesProvider.GetServiceProvider();
            _configurationService = _serviceProvider.GetService<IMemoryCacheService>();
        }
        [TestMethod]
        public void GetConfigurationTest()
        {
            var profileId = Guid.NewGuid();
            var data = new Profile
            {
                ProfileId = profileId,
                Username = "TestName"
            };

             _configurationService.AddRecord(profileId.ToString(), data);

            var result =  _configurationService.GetRecord<Profile>(profileId.ToString());
            Assert.AreEqual(result.ProfileId, profileId);
        }
    }
}
