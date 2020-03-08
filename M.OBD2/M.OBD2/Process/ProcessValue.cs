using System;
using System.Collections.Generic;
using System.Text;

namespace M.OBD2
{
    public class ProcessValue
    {
        public DateTime dtNext { get; set; }
        public StringBuilder sbResponse { get; set; }
        public string Response { get; set; }
        public ulong tx_fail { get; set; }
        public ulong tx_good { get; set; }
        public ulong rx_fail { get; set; }
        public ulong rx_good { get; set; }
        
        public int rxvalue { get; set; }  // Received raw value
        public double value { get; set; }       // Processed value
        public bool isValid { get; set; }

        public ProcessValue()
        {
            sbResponse = new StringBuilder();
        }
    }
}
