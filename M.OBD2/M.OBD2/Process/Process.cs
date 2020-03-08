using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace M.OBD2
{
    public class Process
    {
        private readonly BlueToothCmds oBlueToothCmds;
        private Bluetooth oBluetooth;

        public Process(Bluetooth oBluetooth, BlueToothCmds oBlueToothCmds)
        {
            this.oBluetooth = oBluetooth;
            this.oBlueToothCmds = oBlueToothCmds;
        }

        public async Task RunProcesses()
        {
            await RunProcess();
            //await Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        foreach (BluetoothCmd bcmd in oBlueToothCmds)
            //        {
            //            dtCurrent = DateTime.UtcNow;

            //            if (dtCurrent >= bcmd.dtNext)
            //            {
            //                if (bcmd.CmdBytes != null && bcmd.CmdBytes.Length != 0)
            //                {
            //                    bool result = oBluetooth.SendCommandSync(bcmd.CmdBytes);

            //                    if (!result)
            //                    {
            //                        Debug.WriteLine("Process: {0} {1}", bcmd.Name, Bluetooth.GetStatusMessage());
            //                    }
            //                }

            //                bcmd.dtNext = dtCurrent.AddMilliseconds(bcmd.Rate);
            //                Debug.WriteLine("Process:" + bcmd.Name);
            //            }
            //        }
            //    }
            //}).ConfigureAwait(false);
        }

        public async Task RunProcess()
        {
            DateTime dtCurrent = DateTime.UtcNow;

            foreach (BluetoothCmd bcmd in oBlueToothCmds)
            {
                bcmd.dtNext = dtCurrent.AddMilliseconds(bcmd.Rate);
            }

            while (true)
            {
                foreach (BluetoothCmd bcmd in oBlueToothCmds)
                {
                    dtCurrent = DateTime.UtcNow;

                    if (dtCurrent >= bcmd.dtNext)
                    {
                        if (bcmd.CmdBytes != null && bcmd.CmdBytes.Length != 0)
                        {
                            bool result = await oBluetooth.SendCommandAsync(bcmd);

                            if (result)
                                Debug.WriteLine("Process: {0} Rx: {1} Value: {2}", bcmd.Name,  bcmd.Response,  (bcmd.isRxBytes) ? bcmd.rxvalue : -1);
                            else
                                Debug.WriteLine("Process: {0} {1}", bcmd.Name, Bluetooth.GetStatusMessage());
                        }
                        
                        bcmd.dtNext = dtCurrent.AddMilliseconds(bcmd.Rate);
                        Debug.WriteLine("Process:" + bcmd.Name);
                    }
                }
            }
        }
    }
}
