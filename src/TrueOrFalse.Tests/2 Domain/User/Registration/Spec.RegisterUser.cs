using System;
using BDDish.English;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    public class User_creation_spec : BaseTest
    {
        [Test]
        [Ignore("BDDish is outdated")]
        public void Test()
        {
            Features.Registration
                .Requirement("A user should be able to register")
                .Customer(Persona.UserWhoWantsToRegister).
                    AceptanceCriterion("").
                        When(the_user_registers).
                        Then(an_email_should_be_send).
                        Then(he_should_be_able_to_login).
                Execute(this);
        }

        private const string _password = "somePassword";
        private const string _name = "John Doe";
        private const string _emailAddress = "test@test.de";

        readonly EmptyContext the_user_registers = new EmptyContext(()=>
        {
            var user = new User();
            user.Name = _name;
            user.EmailAddress = _emailAddress;
            SetUserPassword.Run(_password, user);

            RegisterUser.Run(user);
        });

        private readonly Action an_email_should_be_send = () => Assert.That(OneEmailWasSend.IsTrue(), Is.True);
        private readonly Action he_should_be_able_to_login = () =>
        {
            Assert.That(Resolve<CredentialsAreValid>().Yes(_emailAddress, _password), Is.True);
            Assert.That(Resolve<CredentialsAreValid>().Yes("invalidUserNamer", _password), Is.False);
            Assert.That(Resolve<CredentialsAreValid>().Yes(_emailAddress, "invalidPassword"), Is.False);
        };

    }
}
