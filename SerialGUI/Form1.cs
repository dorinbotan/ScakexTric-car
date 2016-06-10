using System;
using System.Windows.Forms;


namespace SerialGUI
{
    public partial class Form1 : Form
    {
        private Connection connection;

        public Form1()
        {
            InitializeComponent();

            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            Acceleration.Series["XAcc"].Points.AddXY(0, 0);      
            Rotation.Series["XRot"].Points.AddXY(0, 0); 
        }
            
        public void AddPoint(Record record)
        {                        
            Acceleration.Series["XAcc"].Points.AddXY(record.tacho, record.XAcc);
            Acceleration.Series["YAcc"].Points.AddXY(record.tacho, record.YAcc);
            Acceleration.Series["ZAcc"].Points.AddXY(record.tacho, record.ZAcc);

            Rotation.Series["XRot"].Points.AddXY(record.tacho, record.XRot);
            Rotation.Series["YRot"].Points.AddXY(record.tacho, record.YRot);
            Rotation.Series["ZRot"].Points.AddXY(record.tacho, record.ZRot);                                       
        }   
        
        public void Draw(Record[] record)
        {
            Clear();

            for (int i = 0; i < record.Length; i++)
                AddPoint(record[i]);
        }             

        public void Clear()
        {                                
            Acceleration.Series["XAcc"].Points.Clear();
            Acceleration.Series["YAcc"].Points.Clear();
            Acceleration.Series["ZAcc"].Points.Clear();

            Rotation.Series["XRot"].Points.Clear();
            Rotation.Series["YRot"].Points.Clear();
            Rotation.Series["ZRot"].Points.Clear();

            Acceleration.Series["XAcc"].Points.AddXY(0, 0);
            Rotation.Series["XRot"].Points.AddXY(0, 0);
        }
        
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                int wordEndPorition = Console.SelectionStart;
                int currentPosition = wordEndPorition;

                while (currentPosition > 0 && Console.Text[currentPosition - 1] != '\n')
                    currentPosition--;

                Console.SelectionColor = System.Drawing.Color.Green;

                WriteInstruction(Console.Text.Substring(currentPosition,
                    wordEndPorition - currentPosition));     

                Console.SelectionColor = Console.ForeColor;
            }
        }

        private void WriteInstruction(string instruction)
        {
            if (instruction.Length > 0)
            {
                string help = System.IO.File.ReadAllText(@"C:\Users\dorin\Documents\Visual Studio 2015\Projects\SerialGUI\SerialGUI\help.txt");

                var form = Form.ActiveForm as Form1;
                string[] words = instruction.ToLower().Split(' ');

                try
                {
                    switch (words[0])
                    {
                        case "connect":
                        case "con":
                            connection = new Connection("COM4", 57600);
                            //connection = new Connection(words[1], int.Parse(words[2]));
                            Console.AppendText("\n" + connection.ToString());
                            break;
                        case "200":
                            connection.Write(200);
                            break;
                        case "201":
                            connection.Write(201);         
                            break;
                        case "205":
                            connection.Write(205);
                            break;       
                        case "help":
                            form.Console.AppendText("\n" + help);
                            break;
                        case "cls":
                            form.Console.Clear();
                            break;
                        case "exit":
                            Environment.Exit(0);
                            break;
                        default:
                            if (int.Parse(words[0]) >= 70 && int.Parse(words[0]) <= 107)
                                connection.Write(byte.Parse(words[0]));                 
                            else
                                Console.AppendText("\n'" + words[0] +
                                    "' is not recognized as a command.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    form.Console.AppendText("\n" + ex.Message);
                }
            }
        }

        public void Write(string text)
        {
            Console.SelectionColor = System.Drawing.Color.Blue;

            Console.AppendText(text + "\n");

            Console.SelectionColor = Console.ForeColor;      
        }
    }
}        