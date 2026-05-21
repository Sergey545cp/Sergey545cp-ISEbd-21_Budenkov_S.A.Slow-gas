using NUnit.Framework;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Infrastructure.PostConfigurations;

namespace SlowGas.Tests.DataModelsTests
{
    [TestFixture]
    public class PostDataModelTests
    {
        [Test]
        public void Post_ValidData_ShouldPass()
        {
            var config = new CashierPostConfiguration { Rate = 1000, SalePercent = 0.1, BonusForExtraSales = 0.5 };
            var post = new PostDataModel(Guid.NewGuid().ToString(), "Кассир", PostType.CashierConsultant, config);

            Assert.That(() => post.Validate(), Throws.Nothing);
        }

        [Test]
        public void Post_ConfigurationNull_ShouldThrowException()
        {
            var post = new PostDataModel(Guid.NewGuid().ToString(), "Должность", PostType.Assistant, (PostConfiguration)null);

            Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Post_RateZero_ShouldThrowException()
        {
            var config = new PostConfiguration { Rate = 0 };
            var post = new PostDataModel(Guid.NewGuid().ToString(), "Должность", PostType.Assistant, config);

            Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Post_RateNegative_ShouldThrowException()
        {
            var config = new PostConfiguration { Rate = -100 };
            var post = new PostDataModel(Guid.NewGuid().ToString(), "Должность", PostType.Assistant, config);

            Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Post_IdEmpty_ShouldThrowException()
        {
            var config = new PostConfiguration { Rate = 1000 };
            var post = new PostDataModel("", "Должность", PostType.Assistant, config);

            Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Post_IdNotGuid_ShouldThrowException()
        {
            var config = new PostConfiguration { Rate = 1000 };
            var post = new PostDataModel("not-a-guid", "Должность", PostType.Assistant, config);

            Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Post_PostNameEmpty_ShouldThrowException()
        {
            var config = new PostConfiguration { Rate = 1000 };
            var post = new PostDataModel(Guid.NewGuid().ToString(), "", PostType.Assistant, config);

            Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Post_PostTypeNone_ShouldThrowException()
        {
            var config = new PostConfiguration { Rate = 1000 };
            var post = new PostDataModel(Guid.NewGuid().ToString(), "Должность", PostType.None, config);

            Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Post_FromJsonCashier_ShouldDeserializeCorrectly()
        {
            var json = "{\"Type\":\"CashierPostConfiguration\",\"Rate\":1000,\"SalePercent\":0.1,\"BonusForExtraSales\":0.5}";
            var post = new PostDataModel(Guid.NewGuid().ToString(), "Кассир", PostType.CashierConsultant, json);

            post.Validate();
            Assert.That(post.ConfigurationModel, Is.InstanceOf<CashierPostConfiguration>());
            var config = post.ConfigurationModel as CashierPostConfiguration;
            Assert.That(config.Rate, Is.EqualTo(1000));
            Assert.That(config.SalePercent, Is.EqualTo(0.1));
            Assert.That(config.BonusForExtraSales, Is.EqualTo(0.5));
        }

        [Test]
        public void Post_FromJsonSupervisor_ShouldDeserializeCorrectly()
        {
            var json = "{\"Type\":\"SupervisorPostConfiguration\",\"Rate\":1500,\"PersonalCountTrendPremium\":50}";
            var post = new PostDataModel(Guid.NewGuid().ToString(), "Руководитель", PostType.Supervisor, json);

            post.Validate();
            Assert.That(post.ConfigurationModel, Is.InstanceOf<SupervisorPostConfiguration>());
            var config = post.ConfigurationModel as SupervisorPostConfiguration;
            Assert.That(config.Rate, Is.EqualTo(1500));
            Assert.That(config.PersonalCountTrendPremium, Is.EqualTo(50));
        }

        [Test]
        public void Post_FromJsonBasic_ShouldDeserializeCorrectly()
        {
            var json = "{\"Type\":\"PostConfiguration\",\"Rate\":2000}";
            var post = new PostDataModel(Guid.NewGuid().ToString(), "Сотрудник", PostType.Assistant, json);

            post.Validate();
            Assert.That(post.ConfigurationModel, Is.InstanceOf<PostConfiguration>());
            Assert.That(post.ConfigurationModel.Rate, Is.EqualTo(2000));
        }

        [Test]
        public void Post_EmptyJson_ShouldCreateDefaultConfig()
        {
            var post = new PostDataModel(Guid.NewGuid().ToString(), "Сотрудник", PostType.Assistant, "");

            // ИСПРАВЛЕНО: теперь ConfigurationModel не null, а дефолтный объект
            Assert.That(post.ConfigurationModel, Is.Not.Null);
            Assert.That(post.ConfigurationModel, Is.InstanceOf<PostConfiguration>());
            Assert.That(post.ConfigurationModel.Rate, Is.EqualTo(0));
        }
    }
}