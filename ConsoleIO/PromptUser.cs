namespace WoWCombatLogTools.ConsoleIO
{
  internal class PromptUser
  {
    public static int MenuChoice(string question)
    {
      Console.Clear();
      while (true)
      {

        Console.WriteLine(question + "\n");
        Console.Write("User: ");
        if (int.TryParse(Console.ReadLine(), out int menu))
        {
          if (menu >= 1 && menu <= 5)
          {
            return menu;
          }

          Console.WriteLine($"Please enter a valid menu choice (1-5).");
        }
        Console.WriteLine("Please enter a number between (1-5).");
      }
    }
    public static int GetAmountOfEncounters(string question)
    {
      Console.Clear();
      while (true)
      {
        Console.Write(question + "\n");
        Console.Write("User: ");
        if (int.TryParse(Console.ReadLine(), out int amountOfEncounters))
        {
          return amountOfEncounters;

        }
        Console.WriteLine("Please enter a positive number.");
      }
    }

    public static string GetFilePathToLog(string question)
    {
      Console.Clear();
      while (true)
      {
        Console.WriteLine(question + "\nOr type \"Back\" to go back.\n");
        Console.Write("User: ");
        string path = Console.ReadLine();
        if (path == "Back")
        {
          return "";
        }
        if (File.Exists(path))
        {
          return path;
        }
        Console.WriteLine("File do not exist. Please enter a valid path to your CombatLog.txt file.");
      }
    }

    public static string GetFolderPathToSplitLog(string question)
    {
      Console.Clear();
      while (true)
      {
        Console.WriteLine(question + "\nOr type \"Back\" to go back.\n");
        Console.Write("User: ");
        string path = Console.ReadLine();
        if (path == "Back")
        {
          return "";
        }
        if (Directory.Exists(path))
        {
          if (path.EndsWith('\\'))
          {
            return path;
          }
          path += '\\';
          return path;
        }
        Console.WriteLine("Path does not exist. Please enter a valid path to your output folder.");
      }
    }

    public static bool AskYesOrNo(string question)
    {
      Console.Clear();
      do
      {
        Console.WriteLine(question + "\n");
        Console.Write("User: ");
        string answer = Console.ReadLine().ToUpper();
        if (answer is "YES" or "Y")
        {
          return true;
        }
        if (answer is "NO" or "N")
        {
          return false;
        }
      } while (true);
    }
    public static List<string> GetSpecificEvents(string question, string pathToLog, string pathToSplit)
    {
      Console.Clear();
      var userEventList = new List<string>();
      var knownEventList = new List<string>();
      var stream = new FileStream(@"ExtractedEvents\ExtractedEventNames.txt", FileMode.Open);
      using (StreamReader sr = new StreamReader(stream))
      {
        string line;
        while ((line = sr.ReadLine()) != null)
        {
          knownEventList.Add(line);
        }
      }

      do
      {
        Console.WriteLine(question + "\nOr type \"Done\" to go back.\n");
        Console.Write("User: ");
        string userEvent = Console.ReadLine().ToUpper();

        if (userEvent is "DONE")
        {
          return userEventList;
        }

        if (!knownEventList.Contains(userEvent))
        {
          bool saveTheEvent = AskYesOrNo("This is a new event! Would you like to save it for later (Y)es or (N)o.");
          if (saveTheEvent)
          {
            knownEventList.Add(userEvent);
          }
          userEventList.Add(userEvent);
        }
      } while (true);
    }


  }
}
