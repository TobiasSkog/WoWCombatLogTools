using System.Text;
using WoWCombatLogTools.ConsoleIO;

namespace WoWCombatLogTools.CombatLogHandler
{
  internal class SplitCombatLog
  {
    public static (TimeSpan, int) SplitCombatLogs(string question, string pathToLog, string pathToSplit, List<string> allowedEvents, int maxAmountOfEncounters = 0, int useDefaultEvents = 0)
    {
      if (!PromptUser.AskYesOrNo(question))
      {
        return ((DateTime.Now - DateTime.Now.AddYears(45)), 0);
      }

      var encounters = new List<string>();
      var defaultEvents = new List<string>()
      {
        "SPELL_CAST_START",
        "SPELL_CAST_SUCCESS",
        "SPELL_AURA_APPLIED",
        "SPELL_AURA_APPLIED_DOSE",
        "SPELL_AURA_REMOVED",
        "SPELL_AURA_REMOVED_DOSE",
        "SPELL_AURA_REFRESH",
        "SPELL_SUMMON",
        "UNIT_HEALTH",
        "GROUP_ROSTER_UPDATE",
      };
      if (useDefaultEvents == 1)
      {
        allowedEvents = defaultEvents;
      }
      var start = DateTime.Now;
      using (StreamReader sr = new StreamReader(pathToLog))
      {
        string line;
        int encounterCount = 0;
        bool isInEncounter = false;
        StringBuilder encounterBuilder = new StringBuilder();

        while (true)
        {
          line = sr.ReadLine();
          if (line == null)
          {
            string encounter = encounterBuilder.ToString();
            encounters.Add(encounter);
            break;
          }
          if (line.Contains("ENCOUNTER_START"))
          {
            isInEncounter = true;
            encounterBuilder.Clear();
            //encounterBuilder.AppendLine(line);
          }
          else if (line.Contains("ENCOUNTER_END"))
          {
            isInEncounter = false;
            encounterCount++;
            encounterBuilder.AppendLine(line);
            string encounter = encounterBuilder.ToString();
            encounters.Add(encounter);
          }
          if (allowedEvents.Count > 0)
          {
            if (isInEncounter && allowedEvents.Any(allowedWord => line.Contains(allowedWord)))
            {
              encounterBuilder.AppendLine(line);
            }
          }
          else
          {
            encounterBuilder.AppendLine(line);
          }
          if (maxAmountOfEncounters != 0 && encounterCount >= maxAmountOfEncounters)
          {
            break;
          }
        }
      }


      foreach (var encounter in encounters)
      {
        var startIndex = encounter.IndexOf('"', 0);
        var endIndex = encounter.IndexOf('"', startIndex + 1);
        if (startIndex >= 0 && endIndex > startIndex)
        {
          var counter = 1;
          var encounterName = encounter.Substring(startIndex + 1, endIndex - startIndex - 1);
          string outputPath = pathToSplit + encounterName + ".txt";
          do
          {
            if (File.Exists(outputPath))
            {
              outputPath = pathToSplit + encounterName + counter + ".txt";
              counter++;
            }
          } while (File.Exists(outputPath));
          FileStream stream = new(outputPath, FileMode.OpenOrCreate);
          using (StreamWriter sw = new(stream))
          {
            sw.Write(encounter);
          }
        }
      }

      var end = DateTime.Now;
      var timeDif = end - start;
      return (timeDif, encounters.Count);
    }
  }
}
