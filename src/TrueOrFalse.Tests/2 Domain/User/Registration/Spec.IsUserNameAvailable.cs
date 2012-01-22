using System;
using NUnit.Framework;
using TrueOrFalse.Core.Registration;
using IContextDescription = BDDish.Model.IContextDescription;


namespace TrueOrFalse.Tests
{
    public class Spec_IsEmailAdressNotInUse : BaseTest
    {
        [Test]
        public void Test()
        {
            Features.Registration
                .Requirement("Email address should be unique")
                .Customer(Persona.UserWhoWantsToRegister).
                    AceptanceCriterion("A used email address should not be usable again for registration").
                        Given(a_used_email_address).
                        Then(the_email_address_should_not_be_usable_anymore).
                Execute(this);            
        }

        private static ContextRegisteredUser _context;

        private readonly Func<IContextDescription> a_used_email_address = () => _context = ContextRegisteredUser.New().SetEmailAddress("some@emailAddress.com");
        private readonly Action the_email_address_should_not_be_usable_anymore 
            = () => {
                       Assert.That(Resolve<IsEmailAddressNotInUse>().Yes(_context.EmailAddress), Is.False);
                       Assert.That(Resolve<IsEmailAddressNotInUse>().Yes("some@otherAddress.com"), Is.True);
                     };
    }
}
