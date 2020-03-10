using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using M.OBD2;
using Newtonsoft.Json;

namespace M.OBD2.TEST
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCommandParsing()
        {
            Assert.AreEqual(BlueToothCmds.CommandTypesToString(new[] { BlueToothCmds.COMMAND_TYPE.DEFAULT }), (((int) BlueToothCmds.COMMAND_TYPE.DEFAULT)).ToString());
            Assert.AreEqual(BlueToothCmds.CommandTypesToString(new[] { BlueToothCmds.COMMAND_TYPE.TPS }), (((int)BlueToothCmds.COMMAND_TYPE.TPS)).ToString());

            BlueToothCmds.COMMAND_TYPE[] Command_Types1 = new[]
                {BlueToothCmds.COMMAND_TYPE.TPS, BlueToothCmds.COMMAND_TYPE.MAF, BlueToothCmds.COMMAND_TYPE.MPG};
            string scommands = BlueToothCmds.CommandTypesToString(new[] { BlueToothCmds.COMMAND_TYPE.TPS, BlueToothCmds.COMMAND_TYPE.MAF, BlueToothCmds.COMMAND_TYPE.MPG});
            BlueToothCmds.COMMAND_TYPE[] Command_Types2 = BlueToothCmds.GetCommandTypes(scommands);
            Assert.AreEqual(JsonConvert.SerializeObject(Command_Types1), JsonConvert.SerializeObject(Command_Types2));

            Assert.AreEqual(BlueToothCmds.CommandTypesToString(null), null);
            Assert.AreEqual(BlueToothCmds.CommandTypesToString(new BlueToothCmds.COMMAND_TYPE[]{}), null);

            var result1 = JsonConvert.SerializeObject(BlueToothCmds.GetCommandTypes(null));
            var result2 = JsonConvert.SerializeObject(new[] { BlueToothCmds.COMMAND_TYPE.DEFAULT });

            Assert.AreEqual(result1, result2);

        }
    }
}
