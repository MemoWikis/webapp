using System;
using NUnit.Framework;

[TestFixture]
public class Autofac_should_export_all_registered_services
{
    [Test]
    public void Write_to_console()
    {
        Console.Write(AutofacRegistrationWriter.Run()); ;
    }
}