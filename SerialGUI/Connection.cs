using System;                       
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
                        
namespace SerialGUI
{
    class Connection
    {
        private readonly int TIMER_PERIOD = 100;
        private readonly int TOLERANCE = 20;
        private readonly object monitor = new object();

        private SerialPort port;

        private int i, reforward;
        private OutMessage messageSent;
        private InMessage messageReceived;              
        private bool waitACK = false;
        private int position = 0;                                                               

        public Connection(string portName, int portBitRate)
        {
            port = new SerialPort(portName, portBitRate, Parity.None, 8, StopBits.One);
            port.Open();

            Thread readThread = new Thread(Read);
            readThread.Start();
        }

        public void Write(Instruction[] instructions)
        {       
            waitACK = true;

            for (i = 0; i < instructions.Length;)
            {                                   
                if (reforward++ >= TOLERANCE)
                    throw new Exception("Message not received.");

                byte[] tmp = instructions[i].GetInstruction();
                messageSent = new OutMessage((byte)(i % 2), tmp[0], tmp[1], tmp[2]);
                port.Write(messageSent.GetMessage(), 0, 5);   

                lock (monitor)
                    Monitor.Wait(monitor, TimeSpan.FromMilliseconds(TIMER_PERIOD));
            }

            waitACK = false;
                                
            Write(205);
        }

        public void Write(byte instruction)
        {
            port.Write(new byte[] { instruction }, 0, 1);     
        }

        public void SendInstructions()
        {

        }
        
        private void Read()
        {
            byte[] receiveBuffer;

            while (true)
                try
                {
                    // ACK received
                    if (waitACK)
                    {
                        receiveBuffer = new byte[1];

                        if (port.BytesToRead >= 1)
                        {
                            port.Read(receiveBuffer, 0, 1);
                            Check(receiveBuffer[0]);
                        }
                    }
                    // Message received
                    else
                    {
                        receiveBuffer = new byte[15];

                        if (port.BytesToRead >= 15)
                        {
                            port.Read(receiveBuffer, 0, 15);

                            try
                            {
                                // Check CRC (if incorrect, exception will be thrown)
                                InMessage inMessage = new InMessage(receiveBuffer);
                                // Return ACK
                                Write(receiveBuffer[0]);

                                // If record received
                                if (receiveBuffer[0] == 0 || receiveBuffer[0] == 1)
                                {
                                    // If message not previously received
                                    if (messageReceived == null ||
                                        inMessage.GetMessage()[0] != messageReceived.GetMessage()[0])
                                    {
                                        byte[] msg = inMessage.GetMessage();
                                        int tacho = messageReceived == null ? msg[1] :
                                                msg[1] + lap.Records[lap.Records.Count - 1].tacho;
                                        short XAcc = CRC8.ByteToShort(msg[3], msg[2]);
                                        short YAcc = CRC8.ByteToShort(msg[5], msg[4]);
                                        short ZAcc = CRC8.ByteToShort(msg[7], msg[6]);
                                        short XRot = CRC8.ByteToShort(msg[9], msg[8]);
                                        short YRot = CRC8.ByteToShort(msg[11], msg[10]);
                                        short ZRot = CRC8.ByteToShort(msg[13], msg[12]);

                                        Record record = new Record(tacho, XAcc, YAcc, ZAcc, XRot, YRot, ZRot);

                                        messageReceived = inMessage;
                                        lap.AddRecord(record);

                                        var form = Form.ActiveForm as Form1;
                                        form.Invoke(new MethodInvoker(delegate
                                        {
                                            form.AddPoint(record);
                                            //form.Write(inMessage.ToString());
                                            //form.Write(record.ToString());
                                        }));
                                    }
                                }
                                else
                                {
                                    Console("All messages received");
                                    new Thread(SendCommands).Start();
                                }
                            }
                            catch (Exception) { }
                        }
                    }
                }
                catch (Exception)
                {
                    Environment.Exit(-1);
                }
        }     
        
        private void SendCommands()
        {
            messageReceived = null;

            race.AddLap(lap);
            position++;
            lap = new Lap();

            lap.AddInsructions(race.GetInstructions());

            Write(lap.GetInstructions().ToArray());
            Write(201);

            var form = Form.ActiveForm as Form1;
            form.Invoke(new MethodInvoker(delegate
            {
                form.Clear();
            }));

            Thread.CurrentThread.Abort();
        }       

        private void Console(string text)
        {
            var form = Form.ActiveForm as Form1;
            form.Invoke(new MethodInvoker(delegate
            {
                form.Write(text);
            }));
        }

        private void Check(byte seqNum)
        {
            if (messageSent.GetMessage()[0] == seqNum)
            {
                reforward = 0;
                i++;

                lock (monitor)
                    Monitor.Pulse(monitor);
            }
        }            

        public override string ToString()
        {
            string toReturn = string.Format("{0}, {1}bpm, parity {2}, {3}bits, stop bit - {4}",
                port.PortName, port.BaudRate, port.Parity, port.DataBits, port.StopBits);
            return toReturn;
        }
    }
}