using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M.OBD2
{
    public class BluetoothCmd : ProcessValue
    {
        // DB Values
        public long Id { get; set; }
        public string Name { get; set; }
        public string Units { get; set; }
        public bool isImperial { get; set; }
        public string Cmd { get; set; }
        public int Rate { get; set; }
        public int Decimals { get; set; }
        public int Bytes { get; set; }
        public bool isRxBytes { get; set; }
        public string Expression { get; set; }
        public BlueToothCmds.COMMAND_TYPE[] Command_Types { get; set; }

        // Non DB Values
        public byte[] CmdBytes { get; set; }

        public BluetoothCmd()
        { 
        }
    }

    public class BlueToothCmds : List<BluetoothCmd>
    {
        public enum COMMAND_TYPE
        {
            DEFAULT,
            AFR,
            VSS,
            MAF,
            MPG,
            TPS
        }

        public BlueToothCmds() // ToDo: load commands from Db
        {
        }

        public void InitCommandBytes()
        {
            foreach (BluetoothCmd bthcmd in this.Where(bthcmd => !string.IsNullOrEmpty(bthcmd.Cmd)))
            {
                bthcmd.CmdBytes = Encoding.ASCII.GetBytes(bthcmd.Cmd + Bluetooth.LINE_BREAK);
            }
        }

        public void CreateTestCommands()
        {
            // Format 01##01
            // 01 = Service
            // https://en.wikipedia.org/wiki/OBD-II_PIDs#Service_05

            Add(new BluetoothCmd()
            {
                Id = 2,
                Name = "IGN",
                Units = "",
                isImperial = true,
                Cmd = "ATIGN",
                Rate = 1000,
                Decimals = 0,
                isRxBytes = false,
                Expression = "",
                Command_Types = new[] { COMMAND_TYPE.DEFAULT }
            });

            Add(new BluetoothCmd()
            {
                Id = 3,
                Name = "TEMP",
                Units = "%",
                isImperial = true,
                Cmd = "01051",
                Rate = 2000,
                Decimals = 0,
                isRxBytes = true,
                Bytes =1,
                Expression = "(a * 100 / 255)",
                Command_Types = new[] { COMMAND_TYPE.DEFAULT }
            });

            Add(new BluetoothCmd()
            {
                Id = 3,
                Name = "VSS",
                Units = "Mph",
                isImperial = true,
                Cmd = "010D1",
                Rate = 1000,
                Decimals = 1,
                isRxBytes = true,
                Bytes = 1,
                Expression = "a*1",
                Command_Types = new[] { COMMAND_TYPE.VSS }
            });

            //Add(new BluetoothCmd()
            //{
            //    Id = 4,
            //    Name = "MAF",
            //    Units = "g/s",
            //    isImperial = true,
            //    Cmd = "",
            //    Rate = 500,
            //    Decimals = 2,
            //    Expression = "a*1",
            //    Command_Types = new[] { COMMAND_TYPE.MAF }
            //});

            //Add(new BluetoothCmd()
            //{
            //    Id = 5,
            //    Name = "MPG",
            //    Units = "",
            //    isImperial = true,
            //    Cmd = null,
            //    Rate = 5000,
            //    Decimals = 1,
            //    Expression = "(a*b*1740.572)/(3600*c/100",
            //    Command_Types = new[] { COMMAND_TYPE.MPG, COMMAND_TYPE.AFR, COMMAND_TYPE.VSS, COMMAND_TYPE.MAF }
            //});

            //Add(new BluetoothCmd()
            //{
            //    Id = 6,
            //    Name = "TPS",
            //    Units = "%",
            //    isImperial = true,
            //    Cmd = "",
            //    Rate = 1000,
            //    Decimals = 0,
            //    Expression = "(a * 100 / 255)",
            //    Command_Types = new[] { COMMAND_TYPE.TPS}
            //});

            InitCommandBytes();
        }
    }
}
