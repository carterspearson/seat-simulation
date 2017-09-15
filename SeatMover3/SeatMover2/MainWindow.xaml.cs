using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace SeatMover2
{
    public partial class MainWindow : Window
    {
        public SeatVisual2 seatVisual2;
        public SeatVisual seatVisual;
        public Bitmap bitmap;

        public MainWindow()
        {
            InitializeComponent();

            seatVisual = new SeatVisual(seat, back, arm1, arm2, seatPlastic, track, runner);
            seatVisual2 = new SeatVisual2(seat_2, back_2, arm1_2, arm2_2, seatPlastic_2, track_2, runner_2);
        }

        private void sliderGroup_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            seatVisual2.updateInformationPosition(sliderUp.Value, sliderTrack.Value, sliderSeatUp.Value, sliderRecline.Value);
            seatVisual.updateInformationPosition(sliderUp.Value, sliderTrack.Value, sliderSeatUp.Value, sliderRecline.Value);
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textBlock2.Text = " " + System.Windows.Forms.Cursor.Position;
        }
    }
}

public class SeatVisual2
{
    public TranslateTransform trackTransform, vertTransform, liftTranslateTransform, fixSeatTransform, fixBackTransform;
    public RotateTransform seatRotateTransform, backRotateTransform, arm1RotateTransform, arm2RotateTransform, liftRotateTransform;
    public TransformGroup transformGroupSeat, transformGroupBack, transformGroupRunner, transformGroupSeatPlastic, transformGroupArm1, transformGroupArm2;
    public System.Windows.Controls.Image seat, back, arm1, arm2, seatPlastic, track, runner;

    public double d1, d2, d3, d4;
    public double d1to2;
    public double heightLaser1, edgeToLaser3;
    public double maxAngle1, minAngle1, angle3;

    public SeatVisual2(System.Windows.Controls.Image seat_, System.Windows.Controls.Image back_, System.Windows.Controls.Image arm1_, System.Windows.Controls.Image arm2_,
        System.Windows.Controls.Image seatPlastic_, System.Windows.Controls.Image track_, System.Windows.Controls.Image runner_)
    {
        seat = seat_;
        back = back_;
        arm1 = arm1_;
        arm2 = arm2_;
        seatPlastic = seatPlastic_;
        track = track_;
        runner = runner_;

        //parameters
        maxAngle1 = 61;
        minAngle1 = 4;
        angle3 = 5;
        d1 = 81;
        d3 = 126;
        d1to2 = 198-192;
        //

        seatRotateTransform = new RotateTransform(0);
        backRotateTransform = new RotateTransform(0);
        trackTransform = new TranslateTransform(0, 0);
        liftTranslateTransform = new TranslateTransform(0, 0);
        arm1RotateTransform = new RotateTransform(0);
        arm2RotateTransform = new RotateTransform(0);
        liftRotateTransform = new RotateTransform(0);
        fixBackTransform = new TranslateTransform(0,0);
        fixSeatTransform = new TranslateTransform(0, 0);

        transformGroupSeat = new TransformGroup();
        transformGroupBack = new TransformGroup();
        transformGroupRunner = new TransformGroup();
        transformGroupSeatPlastic = new TransformGroup();
        transformGroupArm1 = new TransformGroup();
        transformGroupArm2 = new TransformGroup();

        back.RenderTransformOrigin = new System.Windows.Point(.167, .851); // these values have to be changed if the png files are changed
        seat.RenderTransformOrigin = new System.Windows.Point(.167, .851);
        arm1.RenderTransformOrigin = new System.Windows.Point(.167, .851);
        arm2.RenderTransformOrigin = new System.Windows.Point(.589, .891);
        seatPlastic.RenderTransformOrigin = new System.Windows.Point(.167, .851);

        transformGroupArm1.Children.Add(arm1RotateTransform);
        transformGroupArm2.Children.Add(arm2RotateTransform);
        transformGroupSeatPlastic.Children.Add(liftRotateTransform);
        transformGroupSeat.Children.Add(seatRotateTransform);
        transformGroupBack.Children.Add(backRotateTransform);

        transformGroupRunner.Children.Add(trackTransform);
        transformGroupArm2.Children.Add(transformGroupRunner);
        transformGroupArm1.Children.Add(transformGroupRunner);
        transformGroupSeatPlastic.Children.Add(trackTransform);
        transformGroupSeatPlastic.Children.Add(liftTranslateTransform);
        transformGroupSeat.Children.Add(transformGroupSeatPlastic);
        transformGroupBack.Children.Add(transformGroupSeatPlastic);
    }

    public void createVisualLaser(double laser1, double laser2, double laser3, double laser4, TextBlock textblock1, TextBlock textblock2)
    {
        double trackDistance = laser4; // probably need to do some kind of conversion from laser distance to pixels

        textblock1.Text = " ";
        textblock2.Text = " ";

        double seatAngle = Math.Atan2(laser1 - laser2, d1to2);

    }

    public void updateInformationPosition(double sliderY, double sliderX, double sliderSeatAngle, double sliderBackAngle)
    {
        double valueSeatAngle = sliderSeatAngle;
        double valueBackAngle = sliderBackAngle;

        trackTransform.X = sliderX*Math.Cos(angle3 * Math.PI / 180);
        trackTransform.Y = sliderX * Math.Sin(angle3 * Math.PI / 180);

        arm1RotateTransform.Angle = -sliderY;
        double angle1 = (-.0095 * (sliderY + 66 - 113) * (sliderY + 66 - 113) + 82) - 61;
        arm2RotateTransform.Angle = -angle1;

        liftTranslateTransform.X = 30 * Math.Cos((sliderY + 66) * (Math.PI / 180)) - 30 * Math.Cos(66*(Math.PI / 180));
        liftTranslateTransform.Y = -30 * Math.Sin((sliderY + 66) * (Math.PI / 180)) + 30 * Math.Sin(66 * (Math.PI / 180));
        liftRotateTransform.Angle = -2*(180/Math.PI)*(Math.Atan2(30 * Math.Sin(angle1 * (Math.PI / 180)) - 30 * Math.Sin(sliderY * (Math.PI / 180)), 
            98 - 30 * Math.Cos(sliderY * (Math.PI / 180)) + 30 * Math.Cos(angle1 * (Math.PI / 180))));

        seatRotateTransform.Angle = valueSeatAngle;
        backRotateTransform.Angle = valueBackAngle;

        fixBackTransform.X = -angle1 * .1;
        fixBackTransform.Y = -angle1*.17;

        fixSeatTransform.X = -angle1 * .1;
        fixSeatTransform.Y = -angle1 * .17;

        updateVisual();
    }

    public void updateVisual()
    {
        seat.RenderTransform = transformGroupSeat;
        back.RenderTransform = transformGroupBack;
        runner.RenderTransform  = trackTransform;
        arm1.RenderTransform = transformGroupArm1;
        arm2.RenderTransform = transformGroupArm2;
        seatPlastic.RenderTransform = transformGroupSeatPlastic;
    }
}

public class SeatVisual : SeatVisual2
{
    public SeatVisual(System.Windows.Controls.Image seat_, System.Windows.Controls.Image back_, System.Windows.Controls.Image arm1_, System.Windows.Controls.Image arm2_,
        System.Windows.Controls.Image seatPlastic_, System.Windows.Controls.Image track_, System.Windows.Controls.Image runner_) : base(seat_, back_, arm1_, arm2_,
        seatPlastic_, track_, runner_)
    {
        seat.RenderTransformOrigin = new System.Windows.Point(.695, .623);
        back.RenderTransformOrigin = new System.Windows.Point(.695, .623);

        transformGroupSeat.Children.Remove(transformGroupSeatPlastic);
        transformGroupBack.Children.Remove(transformGroupSeatPlastic);

        transformGroupSeat.Children.Add(trackTransform);
        transformGroupSeat.Children.Add(liftTranslateTransform);
        transformGroupSeat.Children.Add(fixSeatTransform);

        transformGroupBack.Children.Add(trackTransform);
        transformGroupBack.Children.Add(liftTranslateTransform);
        transformGroupBack.Children.Add(fixBackTransform);

    }
}

sealed class Win32
{
    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

    [DllImport("gdi32.dll")]
    static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

    static public System.Drawing.Color GetPixelColor(System.Drawing.Point point)
    {
        IntPtr hdc = GetDC(IntPtr.Zero);
        uint pixel = GetPixel(hdc, point.X, point.Y);
        ReleaseDC(IntPtr.Zero, hdc);
        System.Drawing.Color color = System.Drawing.Color.FromArgb((int)(pixel & 0x000000FF),
                     (int)(pixel & 0x0000FF00) >> 8,
                     (int)(pixel & 0x00FF0000) >> 16);
        return color;
    }
}
