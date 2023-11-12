using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimMilyonerOlmakIster
{

        public class Soru
        {
            public string soru_no { get; set; }
            public string zorluk_seviyesi { get; set; }
            public string soru { get; set; }
            public List<string> secenekler { get; set; }
            public string dogru_secenek { get; set; }
        }
    
}
