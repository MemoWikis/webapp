using System;
using NUnit.Framework;
using IContextDescription = BDDish.Model.IContextDescription;

namespace TrueOrFalse.Tests;

public class Spec_IsEmailAdressNotInUse : BaseTest
{
    [Test]
    [Ignore("BDDish is outdated")]
    public void Test()
    {
        Features.Registration
            .Requirement("Email address should be unique")
            .Customer(Persona.UserWhoWantsToRegister).
            AceptanceCriterion("A used email address should not be usable again for registration").
            Given(an_email_address_in_use).
            Then(the_email_address_should_not_be_usable_anymore).
            Execute(this);            
    }

    private static ContextRegisteredUser _context;

    private readonly Func<IContextDescription> an_email_address_in_use
        = () => _context = ContextRegisteredUser.New().
            SetEmailAddress("some@emailAddress.com").
            Add().Persist();

    private readonly Action the_email_address_should_not_be_usable_anymore 
        = () => {
            Assert.That(IsEmailAddressAvailable.Yes(_context.EmailAddress), Is.False);
            Assert.That(IsEmailAddressAvailable.Yes("some@otherAddress.com"), Is.True);
        };
}