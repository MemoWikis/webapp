using System;
using NUnit.Framework;
using TrueOrFalse.Core.Registration;
using IContextDescription = BDDish.Model.IContextDescription;


namespace TrueOrFalse.Tests
{
    public class Spec_IsUserNameAvailable : BaseTest
    {
        [Test]
        public void Test()
        {
            Features.Registration
                .Requirement("Username should be unique")
                .Customer(Persona.UserWhoWantsToRegister).
                    AceptanceCriterion("A used username should not be available twice").
                        Given(a_used_username).
                        Then(the_username_should_not_be_available_anymore).
                Execute(this);            
        }

        private static Context_RegisteredUser _context;

        private readonly Func<IContextDescription> a_used_username = () => _context = new Context_RegisteredUser().SetUserName("someUserName");
        private readonly Action the_username_should_not_be_available_anymore 
            = () => {
                       Assert.That(Resolve<IsEmailAddressNotInUse>().Yes(_context.Name), Is.False);
                       Assert.That(Resolve<IsEmailAddressNotInUse>().Yes("someOtherUserName"), Is.True);
                     };
    }
}
