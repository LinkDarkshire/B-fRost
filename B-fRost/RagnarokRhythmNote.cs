using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bifrost
{
    public class RagnarokRhythmNote
    {
        public double _time { get; set; }
        public int _lineIndex { get; set; }
        public int _lineLayer { get; set; } = 1;
        public int _type { get; set; } = 0;
        public int _cutDirection { get; set; } = 1;
    }
}
