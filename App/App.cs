using WoWCombatLogTools.CombatLogHandler;
using WoWCombatLogTools.ConsoleIO;

namespace WoWCombatLogTools.App
{
  public class App
  {
    private TimeSpan TimeDif = DateTime.Now - DateTime.Now;
    private int Choice = -1;
    private int UseDefaultEvents = 0;
    private int AmountOfEncounters;
    private int TotalSplitsDone = 0;
    private string PathToLogFile = @"";
    private string PathToSplitFolder = @"";
    bool KeepGoing = true;
    bool FileExists;
    bool FolderExists;
    List<string> AllowedEvents = new();
    public void Run()
    {
      (TimeSpan, int) returnedValues;
      do
      {
        Console.Clear();
        if (TimeDif.TotalSeconds < 60)
        {
          Console.WriteLine($"Completed writing to file! It took {TimeDif.TotalSeconds} seconds!");
        }
        FileExists = File.Exists(PathToLogFile);
        FolderExists = Directory.Exists(PathToSplitFolder);


        if (!FileExists || !FolderExists)
        {
          Choice = PromptUser.MenuChoice("What would you like to do?\n1.) Enter the path to your CombatLog.txt\n2.) Enter the path to where you want your Split CombatLogs to go\n5.) Quit");
          switch (Choice)
          {
            case 1:
              PathToLogFile = PromptUser.GetFilePathToLog("Please enter the full path to your CombatLog.txt file.");
              break;
            case 2:
              PathToSplitFolder = PromptUser.GetFolderPathToSplitLog("Please enter the full path to the folder you want your split CombatLogs should go.");
              break;
            case 5:
              KeepGoing = false;
              break;
          }
        }
        else
        {
          Choice = PromptUser.MenuChoice("What would you like to do?\n1.) Enter the path to your CombatLog.txt\n2.) Enter the path to where you want your Split CombatLogs to go \n3.) Select Specific COMBAT_LOG_EVENTS to include in the split (default = from ENCOUNTER_START to ENCOUNTER_END)\n4.) Split the CombatLog\n5.) Quit");
          switch (Choice)
          {
            case 1:
              PathToLogFile = PromptUser.GetFilePathToLog("Please enter the full path to your CombatLog.txt file.");
              break;
            case 2:
              PathToSplitFolder = PromptUser.GetFolderPathToSplitLog("Please enter the full path to the folder you want your split CombatLogs should go.");
              break;
            case 3:
              AllowedEvents = PromptUser.GetSpecificEvents("", PathToLogFile, PathToSplitFolder);
              break;
            case 4:
              int maximumEncounters = PromptUser.GetAmountOfEncounters("How many encounters would you like to to include (0 to include all).");
              returnedValues = SplitCombatLog.SplitCombatLogs("Do you want to split the CombatLog (Y)es or (N)o.", PathToLogFile, PathToSplitFolder, AllowedEvents, maximumEncounters, UseDefaultEvents);
              if (TimeDif < returnedValues.Item1)
              {
                TimeDif = returnedValues.Item1;
              }
              if (AmountOfEncounters < returnedValues.Item2)
              {
                AmountOfEncounters = returnedValues.Item2;
              }
              TotalSplitsDone++;
              break;
            case 5:
              KeepGoing = false;
              break;
          }

        }
      } while (KeepGoing);
      if (TotalSplitsDone > 0)
      {
        Console.WriteLine($"You did a total of {TotalSplitsDone} splits.\nYour maximum amount of Encounters were {AmountOfEncounters}.\nYour longest time spent spliting a file was {TimeDif.TotalSeconds} seconds.");
        Console.WriteLine("\nPress any key to quit...");
        Console.ReadKey();
      }
      else
      {
        Console.WriteLine("Goodbye!");
      }
    }
  }
}
