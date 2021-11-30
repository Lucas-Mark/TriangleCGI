using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace TriangleCGI
{
    public partial class Form1 : Form
    {


        /* Hier definiere ich ein neues Zentrum. Von dem ich grundsätzlich mit Vektorenrotationen neue Eckpunkte definiere*/
        Point Center = new Point(250, 250); //Center
        readonly int Radius = 200; /* Hier ist der Radius wichtig für das Vektorprodukt ich möchte mich ja immer am Rand des Radiuses bewegen (Einheitskreis)*/
        readonly List<Point> Points = new List<Point>();

        readonly Bitmap bmp = new Bitmap(500, 500);

        public Form1()
        {
            InitializeComponent();



        }


        private void button1_Click(object sender, EventArgs e)
        {

            Point Start = new Point(Center.X, Center.Y - Radius); //Start Point dieser Punkt ist über dem Zentrum um den Einheitsvektor genau zu definieren (Beim dreieck zum beispiel die Oberste Spitze)

            double currentPointX = Start.X;
            double currentPointY = Start.Y;
            if (Double.TryParse(textBox2.Text, out double corners)) /*Corners ist die Variable für die Eckpunkteanzahl. also Textbox 2 erst wenn diese Zahl gültig ist wird der Code ausgeführt*/
            {
                

                for (double i = corners; i > 0; i--) //Dieser for-Loop arbeitet die anzahl der Startpunkte ab und setzt diese. Fügt sie auch gleichzeitig der Liste hinzu um später fürs auswürfeln verfügbar zu sein
                {
                    Point currentPoint = new Point(Convert.ToInt32(currentPointX), Convert.ToInt32(currentPointY));
                    Points.Add(currentPoint);
                    CalculatNextPoint(corners, ref currentPointX, ref currentPointY);
                }

                foreach (var point in Points) //Hier wird aus der Liste die Punkte anzahl und Koordinaten gezogen um sie einzuzeichnen.
                {
                    bmp.SetPixel(Convert.ToInt32(point.X), Convert.ToInt32(point.Y), Color.HotPink);

                }

                pictureBox1.Image = bmp;
            }

            //Ab hier endet meine Verbesserung. Im Prinzip ist das der alte Schulcode mit meiner Logik
            Random rnd = new Random();
            int coord4 = rnd.Next(1, 501);
            Point startPointCustom = new Point(coord4, coord4);
            bool success = Int64.TryParse(textBox1.Text, out _);

            if (success)
            {
                Point oldpoint = startPointCustom;
                for (var i = 0; i < Convert.ToInt32(textBox1.Text); i++)
                {
               
                    Random pointchoose = new Random();
                    int pointSetup1 = pointchoose.Next(0, Points.Count);
                    Point pointSetup2 = Points[pointSetup1];
                    Point newpoint = new Point(pointSetup2.X / 2 + oldpoint.X / 2, pointSetup2.Y / 2 + oldpoint.Y / 2);
                    Color randomColorPoint = Color.Black;
                    oldpoint = newpoint;
                    bmp.SetPixel(newpoint.X, newpoint.Y, randomColorPoint);


                    if (i % 5000 == 0) //Performance Verbesserung für das Einzeichnen. Die Punkte werden im 10k Schritt eingezeichnet
                    {
                        pictureBox1.Image = bmp;
                        pictureBox1.Update();
                    }
                }
            }
            else MessageBox.Show("Try Valid Number");



        }

        //Was diese Funktion macht ist nichts anderes als die Vektorenrotation wie oben angesprochen. Ich erstelle mithilfe von Cosinus und Sinus ein Dreieck und drehe das Bitmap um die anzahl der Punkte
        private void CalculatNextPoint(double corners, ref double currentPointX, ref double currentPointY)
        {
            double angle = ConvertToRadians(360 / corners); //Hier kann man es genau sehen. Ich bestimme die Gradanzahl und teile diese durch die anzahl der Eckpunkte die ich habe um so ein Sektor zu ermitteln.

            double Sin = Math.Round(Math.Sin(angle), 5); //Ich runde hier das ergebnis immer auf 5 kommastellen damit die Performance ein bisschen flüssiger ist
            double Cos = Math.Round(Math.Cos(angle), 5);

            double CAX = Center.X - currentPointX;
            double CAY = Center.Y - currentPointY;

            double CBX = CAX * Cos - CAY * Sin;
            double CBY = CAX * Sin + CAY * Cos;

            currentPointX = Math.Round(Center.X - CBX, 0);
            currentPointY = Math.Round(Center.Y - CBY, 0);
        }
        private double ConvertToRadians(double angle) //Diese Funktion brauche ich noch damit ich wirklich Radiant rechne
        {
            return (Math.PI / 180) * angle;
        }

    }

}
