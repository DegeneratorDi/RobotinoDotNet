﻿using System;
using System.Drawing;
using rec.robotino.api2;

namespace RobotinoDotNet
{

    public class Robot
    {
        protected readonly Com _com;
        protected readonly OmniDrive _omniDrive;
        protected readonly Camera _camera;

        public delegate void ImageReceivedEventHandler(Robot sender, Image img);

        public Robot() {
            _com = new MyCom(this);
            _omniDrive = new OmniDrive();
            _camera = new MyCamera(this);

            _omniDrive.setComId(_com.id());
            _camera.setComId(_com.id());

        }

        public event ImageReceivedEventHandler ImageReceived;

        public void connect(String hostname, bool block)
        {

            Console.WriteLine("Connecting...");
            _com.setAddress(hostname);
            try {
                _com.connectToServer(block);
            }
            catch
            {
                Console.WriteLine("Robot not conected");
            }
        }


        public void move(float x, float y, float h)
        {

           _omniDrive.setVelocity(x, y, h);
            System.Threading.Thread.Sleep(10);
           _omniDrive.setVelocity(0, 0, 0);
        }

        private class MyCom : Com
        {
            Robot robot;
            System.Timers.Timer spinTimer;

            public MyCom(Robot robot)
            {
                this.robot = robot;
                spinTimer = new System.Timers.Timer();
                spinTimer.Elapsed += new System.Timers.ElapsedEventHandler(onSpinTimerTimeout);
                spinTimer.Interval = 10;
                spinTimer.Enabled = true;
            }

            public void onSpinTimerTimeout(object obj, System.Timers.ElapsedEventArgs e)
            {
                processEvents();
            }

        }


        private class MyCamera : Camera
        {
            Robot robot;

            public MyCamera(Robot robot)
            {
                this.robot = robot;
                setCameraNumber(0);
                setBGREnabled(true);
            }

            public override void imageReceivedEvent(Image data, uint dataSize, uint width, uint height, uint step)
            {
                /*
                 * we could pass the Image directly to the CameraControl, because this function is called from the main thread from Com::processEvents
                 */
                if (robot.ImageReceived != null)
                    robot.ImageReceived.BeginInvoke(robot, data, null, null);
            }
        }

    }

}