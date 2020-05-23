using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public class CreateUser : ICommand
{
	public string Email { get; set; }
	public string Password { get; set; }
	public string Name { get; set; }

}
