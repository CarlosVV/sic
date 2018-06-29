using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nagnoi.SiC.Domain.Core.Model;
using Nagnoi.SiC.Infrastructure.Core.Dependencies;
using Nagnoi.SiC.Infrastructure.Data;

namespace Application.Core.Test
{
    //[TestClass]
    public class Settings {
        public Settings()  {
            //IoC.InitializeWith(new FactoryDependencyManager());
        }
       // [TestMethod]
        public void TestMethodInsert() {
            var settingRepository = new SettingRepository();
            var setting = new Setting{
                Description = "test2",
                Name="test2",
                Value = "test2"
            };
            settingRepository.Insert(setting);
        }
        //[TestMethod]
        public void TestMethodUpdate() {

            var settingRepository = new SettingRepository();
            var setting = new Setting {
                SettingId = 3,
                Description = "test2",
                Name = "test2",
                Value = "test2"
            };
            settingRepository.Update(setting);
        }
    }
}
