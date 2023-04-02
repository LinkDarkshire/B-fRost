using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bifrost;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bifrost
{
    internal class MidiToRock
    {
        public void OnEntry()
        {
            Console.WriteLine("Drag and drop your Midi file here and press enter: ");
            var filePath = Console.ReadLine().Replace("\"", "");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found.");
                Console.ReadLine();
                return;
            }
            Creater(filePath);
        }

        private void Creater(string filePath)
        {
            string outputFilePath = Path.Combine(Environment.CurrentDirectory + "/Output", Path.GetFileNameWithoutExtension(filePath) + ".dat");

            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory + "/Output")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory + "/Output"));
            }
            Console.WriteLine("Enter the BPM: ");
            int bpm = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Enter the Delay (if none is exisiting enter 0): ");
            int delay = Int32.Parse(Console.ReadLine());

            var rhythmMap = Convert(filePath, bpm, delay);

            File.WriteAllText(outputFilePath, rhythmMap.ToString());

            Console.WriteLine("Conversion complet");
            Console.WriteLine("File can befound under " + outputFilePath);
            Console.ReadLine();
        }

        private JObject Convert(string inputFilePath, int bpm, int delay)
        {
            var midiFile = MidiFile.Read(inputFilePath);
            var tempoMap = midiFile.GetTempoMap();
            double beatDurInSec = 60.0 / bpm;
            var notes = new List<JObject>();

            foreach (var note in midiFile.GetNotes())
            {
                if (note.NoteNumber == 42)
                    continue;

                //var time = note.TimeAs<MetricTimeSpan>(tempoMap).TotalSeconds;
                var time = note.Time;
                //time = time + delay;
                var lineIndex = note.NoteNumber % 4;
                var noteObj = new JObject(
                    new JProperty("_time", time),
                    new JProperty("_lineIndex", lineIndex),
                    new JProperty("_lineLayer", 1),
                    new JProperty("_type", 0),
                    new JProperty("_cutDirection", 1)
                );
                notes.Add(noteObj);
            }

            var rhythmMap = new JObject(
                new JProperty("_version", "1.0.0"),
                new JProperty("_BPMChanges", new JArray()),
                new JProperty("_customData", new JObject(
                    new JProperty("_customEvents", new JArray()),
                    new JProperty("_pointDefinitions", new JArray())
                )),
                new JProperty("_bookmarks", new JArray()),
                new JProperty("_events", new JArray()),
                new JProperty("_notes", new JArray(notes)),
                new JProperty("_obstacles", new JArray()),
                new JProperty("_waypoints", new JArray())
            );

            if (bpm != 0)
            {
                var bpmChange = new JObject(
                    new JProperty("_time", 0),
                    new JProperty("_bpm", bpm)
                );
                rhythmMap["_BPMChanges"] = new JArray(bpmChange);
            }

            return rhythmMap;
        }
    }
}
