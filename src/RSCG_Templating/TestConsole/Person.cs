using RSCG_TemplatingCommon;

namespace TestConsole;

[IGenerateDataFromClass("ClassTypeName")]
[IGenerateDataFromClass("ClassPropByName")]
[IGenerateDataFromClass("ClassDebuggerDisplay")]
[IGenerateDataFromClass("ClassToInterface")]
public partial class Person
{
    private string Test { get { return this.GetType().Name; } }
    public string Name { get { return FullName(" "); } }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ICollection<Person> Children { get; set; }
    public DateTime? BirthDate { get; set; }
    public byte[] Picture { get; set; }
    public string FullName(string separator = " ")
    {
        return FirstName + separator + LastName;
    }
    public void DisplayNameOnConsole()
    {
        Console.WriteLine(FullName());
    }
    public async Task<string> GetName()
    {
        await Task.Delay(1000);
        return FirstName ?? "";
    }
    public Task<string> GetFullName()
    {
        return Task.FromResult(FullName());
    }
    public Task SaveId(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("this is an error because is <0 ");
        }
        return Task.CompletedTask;
    }
}
