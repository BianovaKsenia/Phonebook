using TelephoneBook;

internal class Program
{
    private static void Main(string[] args)
    {
        
        Phonebook phonebook = Phonebook.GetInstance();
        phonebook.GetMenu();
    }
}