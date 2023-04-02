
using System;
using System.IO;


namespace Bifrost
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Select what Conversion do you wish: ");
            Console.WriteLine("Beatsaber to Ragnarock (1) ");
            Console.WriteLine("Midi to Ragnarock (2) (WIP)");
            string selcetedKey = Console.ReadLine();
            switch(selcetedKey)
            {
                case "1":
                    BeatToRock beat = new BeatToRock();
                    beat.OnEntry();
                    break;

                case "2":
                    MidiToRock midi = new MidiToRock();
                    midi.OnEntry();
                    break;

                default:
                    Console.WriteLine("No such Option!");
                    Console.ReadLine();
                    break;

            }           
        }
    }
}