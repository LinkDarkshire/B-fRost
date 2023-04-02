using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bifrost
{
    public class RagnarokRhythmMap
    {
        public string _version { get; set; } = "1.0.0";
        public List<object> _customData { get; set; } = new List<object>();
        public List<object> _BPMChanges { get; set; } = new List<object>();
        public List<object> _bookmarks { get; set; } = new List<object>();
        public List<RagnarokRhythmNote> _notes { get; set; } = new List<RagnarokRhythmNote>();
        public List<object> _obstacles { get; set; } = new List<object>();
        public List<object> _waypoints { get; set; } = new List<object>();
    }
}
