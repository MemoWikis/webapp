using System;
using BDDish.English;
using NUnit.Framework;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Registration;

namespace TrueOrFalse.Tests
{
    public class User_creation_spec  : BaseTest
    {

        [Test]
        public void Test()
        {
            Features.Registration
                .Requirement("A user should be able to register")
                .Customer(Persona.UserWhoWantsToRegister).
                    AceptanceCriterion("correct behaviour").
                        When(the_user_registers).
                        Then(an_email_should_be_send).
                        Then(he_should_be_able_to_login).
                Execute(this);
        }

        EmptyContext the_user_registers = new EmptyContext(()=>
        {
            var user = new User();
            user.FirstName = "Firstname";
            user.LastName = "Lastname";
            user.UserName = "UserName";

            Resolve<RegisterUser>().Run(user);
        });

        Action an_email_should_be_send;
        private Action he_should_be_able_to_login;

    }
}
