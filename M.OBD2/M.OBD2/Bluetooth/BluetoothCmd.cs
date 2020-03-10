using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

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
        public string sCommand_Types { get; set; } // Command type comma delimited format ex. "2" or  "2,3,4" ...
        public bool isSelected { get; set; }                // If user selected
        
        // Non DB Values
        public byte[] CmdBytes { get; set; }    // Pre generated command bytes for a given Cmd string

        // Associated Command_Type - multiple indicates multi values used in a given Expression string 
        // Ex: Expression = "(a*b*1740.572)/(3600*c/100)", Command_Types = { COMMAND_TYPE.MPG, COMMAND_TYPE.VSS, COMMAND_TYPE.MAF }
        public BlueToothCmds.COMMAND_TYPE[] Command_Types { get; set; }
        public BlueToothCmds.SELECTION_TYPE Selection_Type { get; set; } // Selection state Ex. User, Process ...

        public BluetoothCmd()
        {
        }
    }

    public class BlueToothCmds : List<BluetoothCmd>
    {
        public const int MAX_COMMANDS = 10; // Maximum user process selections
        public const string SCOMMAND_SPECIFIER = ","; // Duplicated specifier types for faster iteration
        public const char COMMAND_SPECIFIER = ',';

        public enum COMMAND_TYPE // Process command type 
        {
            DEFAULT,
            AFR,
            VSS,
            MAF,
            MPG,
            TPS
        }

        public enum SELECTION_TYPE // Current selection state of a process
        {
            NONE,   // No selection
            USER,   // User selected
            PROCESS,    // Process selected
            USER_PROCESS // User and Process selected
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

        public static string CommandTypesToString(COMMAND_TYPE[] Command_Types) // String generation method for handling db arrays
        {
            StringBuilder sb = new StringBuilder();

            if (Command_Types == null || Command_Types.Length == 0)
                return null;

            if (Command_Types.Length == 1)
                return ((int)Command_Types[0]).ToString();

            for (int i = 0; i < Command_Types.Length; i++)
            {
                sb.Append(((int)Command_Types[i]) + ((i < Command_Types.Length - 1) ? SCOMMAND_SPECIFIER : ""));
            }

            return sb.ToString();
        }

        public static COMMAND_TYPE[] GetCommandTypes(string sCommand_Types) // String parsing method for handling db arrays
        {
            if (!string.IsNullOrEmpty(sCommand_Types))
            {
                COMMAND_TYPE[] Command_Types;

                if (sCommand_Types.IndexOf(SCOMMAND_SPECIFIER, StringComparison.OrdinalIgnoreCase) > 0) // If specifier found
                {
                    string[] sValues = sCommand_Types.Split(COMMAND_SPECIFIER);
                    if (sValues.Length != 0)
                    {
                        Command_Types = new COMMAND_TYPE[sValues.Length];

                        for (int idx = 0; idx < sValues.Length; idx++)
                        {
                            if (int.TryParse(sValues[idx], out int value))
                                Command_Types[idx] = (COMMAND_TYPE)value;
                            else
                                return new[] { COMMAND_TYPE.DEFAULT };
                        }
                        return Command_Types;
                    }
                }
                else if (int.TryParse(sCommand_Types, out int value))
                {
                    Command_Types = new COMMAND_TYPE[1];
                    Command_Types[0] = (COMMAND_TYPE)value;
                    return Command_Types;
                }
            }
            return new[] { COMMAND_TYPE.DEFAULT };
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
            //    Id = 5,
            //    Name = "MPG",
            //    Units = "",
            //    isImperial = true,
            //    Cmd = null,
            //    Rate = 5000,
            //    Decimals = 1,
            //    Expression = "(a*b*1740.572)/(3600*c/100)",
            //    Command_Types = new[] { COMMAND_TYPE.MPG, COMMAND_TYPE.VSS, COMMAND_TYPE.MAF }
            //});

            InitCommandBytes();

            foreach (BluetoothCmd b in this)
            {
                b.InitExpression(b.Expression, b.Command_Types);
            }
        }
    }
}
