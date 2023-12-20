using TestConsole;

var p = new Person();
Console.WriteLine("The generated string type is "+p.MyTypeName);
p.FirstName = "Andrei";
//set last name via prop
p.SetPropValue(ePerson_Properties.LastName, "Ignat");
Console.WriteLine("called directly first name : " + p.FirstName);
Console.WriteLine("called via enum of prop first name : " + p.GetPropValue(ePerson_Properties.FirstName));
Console.WriteLine("called get property :" + p.GetPropValue(ePerson_Properties.Name));

Console.WriteLine("this will throw error because Name has not set ");
try
{
    p.SetPropValue(ePerson_Properties.Name, "asd");
}
catch (Exception)
{
    Console.WriteLine("this is good!");
}

IPerson personInterface = p;

//Console.ReadLine();