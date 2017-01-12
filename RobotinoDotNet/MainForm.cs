using System;
using System.Windows.Forms;
using System.Threading;
namespace RobotinoDotNet
{
    public partial class MainForm : Form
    {
        Robot robot;
        Joystick joystick;
        string hostname = "127.0.0.1";

        float s = 1f, rs = -1f;

        public MainForm()
        {
            InitializeComponent();

            robot = new Robot();            
            robot.connect(hostname, true);
            CameraControl cameraControl = new CameraControl(robot);
            tableLayoutPanel1.Controls.Add(cameraControl);

            joystick = new Joystick();
            joystick.PrintListHID();
            joystick.ConnectDevise(0);
            joystick.StartPollDevice(10);

            Thread myThread = new Thread(Run);
            myThread.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        void Run()
        {
            float s = 1f, rs = -1f;

            while (true)
            {
                for (byte i = 0; i < 9; i++)
                    Console.Write(robot.DistanceSensorStatus(i) + " ");

                Console.WriteLine();


                

                if (joystick.getStatusButton("1"))
                {
                    s = 0.1f; rs = -0.1f;
                }

                if (joystick.getStatusButton("2"))
                {
                    s = 0.25f; rs = -0.25f;
                }

                if (joystick.getStatusButton("3"))
                {
                    s = 0.50f; rs = -0.50f;
                }

                if (joystick.getStatusButton("4"))
                {
                    s = 1f; rs = -1f;
                }

                if (joystick.getStatusButton("up"))
                {
                    robot.move(s, 0, 0);
                }

                if (joystick.getStatusButton("down"))
                {
                    robot.move(rs, 0, 0);
                }

                if (joystick.getStatusButton("left"))
                {
                    robot.move(0, s, 0);
                }
                if (joystick.getStatusButton("right"))
                {
                    robot.move(0, rs, 0);
                }

                if (joystick.getStatusButton("LT"))
                {
                    robot.move(0, 0, s);
                }
                if (joystick.getStatusButton("RT"))
                {
                    robot.move(0, 0, rs);
                }

                if (joystick.getStatusButton("10"))
                {
                    return;
                }
            }
        }

    }
    
}
