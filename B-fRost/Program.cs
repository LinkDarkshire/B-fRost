using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;

namespace Bifrost
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath;
            if (args.Length > 0)
            {
                filePath = args[0];
            }
            else
            {
                Console.WriteLine("Drag and drop your BeatSaber map file here and press enter: ");
                filePath = Console.ReadLine().Replace("\"", "");
            }
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found.");
                Console.ReadLine();
                return;
            }

            string outputFilePath = Path.Combine(Environment.CurrentDirectory + "/Output", Path.GetFileName(filePath));

            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory + "/Output")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory + "/Output"));
            }

            JObject map = JObject.Parse(File.ReadAllText(filePath));

            JObject customData = (JObject)map["_customData"];
            
            JArray bpm = (JArray)customData["_BPMChanges"];
            if (bpm != null)
            {
                if (bpm.Count > 0)
                    bpm.RemoveAll();
            }

            JArray bookmarks = (JArray)customData["_bookmarks"];
            if (bookmarks != null)
            {
                if (bookmarks.Count > 0)
                    bookmarks.RemoveAll();
            }

            JArray events = (JArray)map["_events"];
            if (events.Count > 0)
                events.RemoveAll();

            JArray obstacles = (JArray)map["_obstacles"];
            if (obstacles.Count > 0)
                obstacles.RemoveAll();
            //foreach (JObject item in obstacles.Children<JObject>())
            //{
            //    item.Remove();
            //}

            JArray notes = (JArray)map["_notes"];
            double currenttime = 0;
            byte timecount = 1;
            for (int i = 0; i < notes.Count; i++)
            {
                JObject note = (JObject)notes[i];

                double time = (double)note["_time"];
                int lineIndex = (int)note["_lineIndex"];
                int lineLayer = (int)note["_lineLayer"];
                int type = (int)note["_type"];
                int cutDirection = (int)note["_cutDirection"];

                time = (double)Math.Round(time * 4f) / 4f;
                if (time == currenttime)
                {
                    timecount++;
                    if (timecount > 2)
                    {
                        notes.RemoveAt(i);
                        Console.WriteLine($"Time {time} has more then 2 entrys. Deleting ...");
                        i--;
                        continue;
                    }
                }
                else
                {
                    currenttime = time;
                    timecount = 1;
                }
                int newLineIndex = lineIndex;
                bool lineIndexChanged = false;

                for (int j = 0; j < i; j++)
                {
                    JObject prevNote = (JObject)notes[j];
                    double prevTime = (double)prevNote["_time"];
                    int prevLineIndex = (int)prevNote["_lineIndex"];

                    if (prevTime == time && prevLineIndex == newLineIndex)
                    {
                        newLineIndex++;
                        if(newLineIndex > 3)
                            newLineIndex = 0;

                        lineIndexChanged = true;
                        j = -1; // start over
                    }
                }

                if (lineIndexChanged)
                {
                    Console.WriteLine($"Note at time {time} and line index {lineIndex} has been moved to line index {newLineIndex}.");
                    lineIndex = newLineIndex;
                }

                notes[i]["_time"] = time;
                notes[i]["_lineIndex"] = lineIndex;
                notes[i]["_lineLayer"] = 1;
                notes[i]["_type"] = 0;
                notes[i]["_cutDirection"] = 1;
            }

            map["_version"] = "1.0.0";

            File.WriteAllText(outputFilePath, map.ToString());
            Console.WriteLine($"Ragnarock map saved to {outputFilePath}");

            Console.ReadLine();
        }
    }
}