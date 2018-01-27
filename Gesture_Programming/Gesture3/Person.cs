using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Gesture3
{
    //This class is used to store the values of each skeleton the kinect finds
    public class Person
    {
        //Value used to determine how far the user has to push to trigger a click event
        public double pushDif = 400;
        //Used to check if the skeleton is currently being drawn on a page
        public Boolean skeleCheck = false;

        //Sets the size of the skeletons joints
        //Used to visualise where the users hands are
        private int JOINT_HEIGHT = 40;
        private int JOINT_WIDTH = 40;

        //Data structure to hold the x,y, and depth coordinates of each instanced joint from the kinect
        public struct Joint
        {
            public int X;
            public int Y;
            public int D;
        }

        #region joints
        //Creates joint variables for each potential body joint in order to track joints
        public Joint jHandLeft,
                     jHandRight,
                     jSpine
                     = new Joint();

        #endregion
        #region drawing skeleton
        //The two joints tht need to be visualised on screen represented as circles
        public Ellipse LeftPalm = new Ellipse();
        public Ellipse RightPalm = new Ellipse();
        #endregion

        
        //Person Constructor that takes an integer parameter as an ID value
        public Person(int id)
        {
            //Sets the colour of the hand joints using the ID to assign a colour
            SolidColorBrush jointColorBrush = new SolidColorBrush();
            switch (id)
            {
                case 0: jointColorBrush.Color = Colors.Blue; ; break;
                case 1: jointColorBrush.Color = Colors.Pink; ; break;
                case 2: jointColorBrush.Color = Colors.Yellow; ; break;
                case 3: jointColorBrush.Color = Colors.Orange; ; break;
                case 4: jointColorBrush.Color = Colors.Green; ; break;
                case 5: jointColorBrush.Color = Colors.Brown; ; break;
            }

            //Gets the drawable joints from a function
            List<Ellipse> joints = this.getJoints();
            //Goes through each of the joints and gives them a colour and size
            for (int i = 0; i < joints.Count; i++)
            {
                joints[i].Fill = jointColorBrush;
                joints[i].Width = JOINT_HEIGHT;
                joints[i].Height = JOINT_WIDTH;
            }
        }

        #region array skeleton
        //Function to return both drawable joints as a list
        public List<Ellipse> getJoints()
        {
            List<Ellipse> Joints = new List<Ellipse>();
            Joints.Add(LeftPalm);
            Joints.Add(RightPalm);
            return Joints;
        }
        #endregion

        //Method to draw each of the joints
        public void DrawPerson()
        {
            DrawJoint(this.LeftPalm, this.jHandLeft.X, this.jHandLeft.Y);
            DrawJoint(this.RightPalm, this.jHandRight.X, this.jHandRight.Y);
        }
        
        //Method to draw a given joint using the x and y coordinate parameters
        private void DrawJoint(Ellipse joint, int X, int Y)
        {
            //Uses a margin to set the location of the circle
            joint.Margin = new Thickness(X - joint.Width / 2, Y - joint.Height / 2, 0, 0);
        }

    }
}