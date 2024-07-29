// Scanner.cs
//
// Property of the Kansas State University IT Help Desk
// Written by: Nathan York, Enzo Wollmeister-Kuschel, and Dean Blickenstaff
// 
// This class reads the data stream from the Scantron machine.

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScantronRevived.Data
{
    public class Scanner
    {

        private UIViewModel viewModel;

        // Serial port that reads in the data. A very basic implementation is used.
        private SerialPort serial_port = new SerialPort("COM3", 38400, Parity.Odd, 7, StopBits.Two);

        // Holds the data from the scanner as is.
        public string raw_scantron_output { get; set; }

        // Raw data split up at EndOfRecord symbol specified in the Scantron machine's configuration sheet.
        public List<string> raw_cards { get; set; }

        public List<string> raw_keys { get; set; }

        private CancellationTokenSource? t;

        private Thread? t1;

        /// <summary>
        /// Constructor for the scanner - we'll want to pass it a config file/use one in the future
        /// </summary>
        public Scanner(UIViewModel viewModel)
        {
            raw_scantron_output = "";
            raw_cards = new List<string>();
            this.viewModel = viewModel;
        }

        public void Scan()
        {
            raw_scantron_output = "";
            raw_cards = new List<string>();
            if (!serial_port.IsOpen)
            {
                serial_port.Open();
                serial_port.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            }
            t = new CancellationTokenSource();
            t1 = new Thread(() => ScanInOtherThread(serial_port, t.Token));
            t1.Start();
        }

        public void StopScanning()
        {
            if (t == null) throw new Exception();
            t.Cancel();
        }

        public static void ScanInOtherThread(SerialPort serial_port, CancellationToken t)
        {
            while (true)
            {
                try
                {
                    t.ThrowIfCancellationRequested();
                    serial_port.Write("\u0011");
                    ///Do not change the sleep value. This is the quickest the machine can scan each card and get the message to scan again.
                    System.Threading.Thread.Sleep(1200);
                }
                catch
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Event for when data is received by the serial port.
        /// </summary>
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            serial_port = (SerialPort)sender;
            raw_scantron_output += serial_port.ReadExisting();
        }

        /// <summary>
        /// Closes the serial port and puts the data into a list, split at EndOfRecord symbol.
        /// </summary>
        public void Stop(bool IsKeys)
        {
            t?.Dispose();
            serial_port.Close();
            string[] splitter = new string[] { "\r\n" };
            //raw_scantron_output.TrimEnd(new char[] { 'r', 'n', '\\' });
            if(!IsKeys)
            {
                raw_cards = raw_scantron_output.Split(splitter, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            }
            else
            {
                raw_keys = raw_scantron_output.Split(splitter, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            }

        }
    }
}
