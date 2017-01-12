using Mighty.HID;
using System;
using System.Collections.Generic;

namespace RobotinoDotNet
{
    class Joystick
    {
        HIDDev dev;
        List<HIDInfo> devs;
        System.Timers.Timer spinTimer;
        byte[] devOut = new byte[9];

        public delegate void run();

        public byte[] GetReport => devOut;

        public Joystick()
        {
            SearchDevises();
        }

        public void SearchDevises()
        {
            /* browse for hid devices */
            devs = HIDBrowse.Browse();
        }


        public void PrintListHID()
        {
            Console.WriteLine("List of USB HID devices:");            
            /* display VID and PID for every device found */
            foreach (var dev in devs)
            {
                Console.WriteLine("VID = " + dev.Vid.ToString("X4") +
                    " PID = " + dev.Pid.ToString("X4") +
                    " Product: " + dev.Product);
            }

            
        }

        public void ConnectDevise(int num)
        {
            //исправить
           // Console.Write("HID dev num: ");
            //num = int.Parse(Console.ReadLine());
            //
            dev = new HIDDev();
            dev.Open(devs[num]);
        }

        public void StartPollDevice(int interval)
        {
            dev.Read(devOut);
            
            spinTimer = new System.Timers.Timer();
            spinTimer.Elapsed += new System.Timers.ElapsedEventHandler(PullDevise);
            spinTimer.Interval = interval;
            spinTimer.Enabled = true;

            
            
            void PullDevise(object obj, System.Timers.ElapsedEventArgs e)
            {
                dev.Read(devOut);
            }
        }

        public void StartPollDevice(int interval, run Run)
        {
            /*
            dev.Read(devOut);
            bool trigger = true;
            for (int i = 0; i < 9; i++)
            {
                if (devOut[i] != 0) {
                    trigger = false;
                    break;
                }

            }
            if (trigger)
            {
                throw new ArgumentException("device is not joystick");                
            }
            */
            spinTimer = new System.Timers.Timer();
            spinTimer.Elapsed += new System.Timers.ElapsedEventHandler(PullDevise);
            spinTimer.Interval = interval;
            spinTimer.Enabled = true;

            void PullDevise(object obj, System.Timers.ElapsedEventArgs e)
            {
                dev.Read(devOut);

                Run();
            }
        }

        public void StopPollDevice()
        {
            spinTimer.Enabled = false;
        }

        public bool getStatusButton(String key)
        {
            switch (key)
            {
                case "up":
                    {
                        return (devOut[2] == 0) ? true : false;
                    }

                case "down":
                    {
                        return (devOut[2] == 255) ? true : false;
                    }

                case "left":
                    {
                        return (devOut[1] == 0) ? true : false;
                    }

                case "right":
                    {
                        return (devOut[1] == 255) ? true : false;
                    }

                case "1":
                    {
                        return (devOut[6] == 31) ? true : false;
                    }

                case "2":
                    {
                        return (devOut[6] == 47) ? true : false;
                    }

                case "3":
                    {
                        return (devOut[6] == 79) ? true : false;
                    }

                case "4":
                    {
                        return (devOut[6] == 143) ? true : false;
                    }

                case "LT":
                    {
                        return (devOut[7] == 1) ? true : false;
                    }

                case "RT":
                    {
                        return (devOut[7] == 2) ? true : false;
                    }

                case "LB":
                    {
                        return (devOut[7] == 4) ? true : false;
                    }

                case "RB":
                    {
                        return (devOut[7] == 8) ? true : false;
                    }

                case "9":
                    {
                        return (devOut[7] == 16) ? true : false;
                    }

                case "10":
                    {
                        return (devOut[7] == 32) ? true : false;
                    }
            }
            return false;
        }


    }
}
