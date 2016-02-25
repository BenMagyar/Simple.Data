namespace Simple.Data.UnitTest
{
    using Plurally;
    using System.Globalization;
    using Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class PluralizationTest
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            Database.SetPluralizer(new PlurallyPluralizer());
        }

        /// <summary>
        ///A test for IsPlural
        ///</summary>
        [Test]
        public void IsPluralLowercaseUsersShouldReturnTrue()
        {
            Assert.IsTrue("Users".IsPlural());
        }

        /// <summary>
        ///A test for IsPlural
        ///</summary>
        [Test]
        public void IsPluralUppercaseUsersShouldReturnTrue()
        {
            Assert.IsTrue("USERS".IsPlural());
        }

        /// <summary>
        ///A test for IsPlural
        ///</summary>
        [Test]
        public void IsPluralLowercaseUserShouldReturnFalse()
        {
            Assert.IsFalse("User".IsPlural());
        }

        /// <summary>
        ///A test for IsPlural
        ///</summary>
        [Test]
        public void IsPluralUppercaseUserShouldReturnFalse()
        {
            Assert.IsFalse("USER".IsPlural());
        }

        /// <summary>
        ///A test for Pluralize
        ///</summary>
        [Test()]
        public void PluralizeUserShouldReturnUsers()
        {
            Assert.AreEqual("Users", "User".Pluralize());
        }

        /// <summary>
        ///A test for Pluralize
        ///</summary>
        [Test()]
        // ReSharper disable InconsistentNaming
        public void PluralizeUSERShouldReturnUSERS()
        // ReSharper restore InconsistentNaming
        {
            Assert.AreEqual("USERS", "USER".Pluralize());
        }

        /// <summary>
        ///A test for Singularize
        ///</summary>
        [Test()]
        public void SingularizeUsersShouldReturnUser()
        {
            Assert.AreEqual("User", "Users".Singularize());
        }

        /// <summary>
        ///A test for Singularize
        ///</summary>
        [Test()]
        public void SingularizeUserShouldReturnUser()
        {
            Assert.AreEqual("User", "User".Singularize());
        }

        /// <summary>
        ///A test for Singularize
        ///</summary>
        [Test()]
        // ReSharper disable InconsistentNaming
        public void SingularizeUSERSShouldReturnUSER()
        // ReSharper restore InconsistentNaming
        {
            Assert.AreEqual("USER", "USERS".Singularize());
        }

        [Test]
        public void PluralizeCompanyShouldReturnCompanies()
        {
            Assert.AreEqual("Companies", "Company".Pluralize());
        }

        [Test]
        public void SingularizeCompaniesShouldReturnCompany()
        {
            Assert.AreEqual("Company", "Companies".Singularize());
        }
    }

    class PlurallyPluralizer : IPluralizer
    {
        private readonly Pluralizer _pluralizationService =
            new Pluralizer(new CultureInfo("en"));

        public bool IsPlural(string word)
        {
            return _pluralizationService.IsPlural(word);
        }

        public bool IsSingular(string word)
        {
            return _pluralizationService.IsSingular(word);
        }

        public string Pluralize(string word)
        {
            return _pluralizationService.Pluralize(word);
        }

        public string Singularize(string word)
        {
            return _pluralizationService.Singularize(word);
        }
    }
}