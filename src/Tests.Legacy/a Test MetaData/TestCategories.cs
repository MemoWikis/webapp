public static class TestCategories
{
	/// <summary>
	/// Tests not exclusivly small units, but also bigger parts. 
	/// Could be also labeled as "IntegrationTest"
	/// </summary>
	public const string Programmer = "ProgrammersTest";

    public const string Integration = "Integration";

	/// <summary>
	/// Tests small decoupled, portions of the code
	/// </summary>
	public const string UnitTest = "UnitTest";

	/// <summary>
	/// Following the BDD ideas
	/// </summary>
	public const string BehaviourTest = "BehaviourTest";

    public const string IgnoreOnCI = "IgnoreOnCI";
}