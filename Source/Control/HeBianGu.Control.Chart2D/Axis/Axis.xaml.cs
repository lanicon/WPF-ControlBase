﻿using HeBianGu.Base.WpfBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HeBianGu.Control.Chart2D
{
    public class Axis : XyLayer
    {

        public int AlignLenght
        {
            get { return (int)GetValue(AlignLenghtProperty); }
            set { SetValue(AlignLenghtProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AlignLenghtProperty =
            DependencyProperty.Register("AlignLenght", typeof(int), typeof(Axis), new PropertyMetadata(5, (d, e) =>
             {
                 Axis control = d as Axis;

                 if (control == null) return;

                 //int config = e.NewValue as int;


             }));

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelStyleProperty =
            DependencyProperty.Register("LabelStyle", typeof(Style), typeof(Axis), new PropertyMetadata(default(Style), (d, e) =>
             {
                 Axis control = d as Axis;

                 if (control == null) return;

                 Style config = e.NewValue as Style;

             }));

        public bool TextAlignmentCenter
        {
            get { return (bool)GetValue(TextAlignmentCenterProperty); }
            set { SetValue(TextAlignmentCenterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextAlignmentCenterProperty =
            DependencyProperty.Register("TextAlignmentCenter", typeof(bool), typeof(Axis), new PropertyMetadata(default(bool), (d, e) =>
            {
                Axis control = d as Axis;

                if (control == null) return;

                //bool config = e.NewValue as bool;

                control.TryDraw();

            }));


        public bool AlignAlignmentCenter
        {
            get { return (bool)GetValue(AlignAlignmentCenterProperty); }
            set { SetValue(AlignAlignmentCenterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AlignAlignmentCenterProperty =
            DependencyProperty.Register("AlignAlignmentCenter", typeof(bool), typeof(Axis), new PropertyMetadata(default(bool), (d, e) =>
            {
                Axis control = d as Axis;

                if (control == null) return;

                control.TryDraw();

            }));


        protected override void InitX()
        {
            base.InitX();

            if (this.TextAlignmentCenter)
            {
                double span = (this.maxX - this.minX) / this.xAxis.Count;

                this.maxX = this.maxX + span / 2;

                this.minX = this.minX - span / 2;
            }

            if (this.TextAlignmentCenter)
            {
                double span = (this.maxY - this.minY) / this.yAxis.Count;

                this.maxY = this.maxY + span / 2;

                this.minY = this.minY - span / 2;
            }
        }

        protected override void InitY()
        {
            base.InitY();

            if (this.TextAlignmentCenter)
            {
                double span = (this.maxY - this.minY) / this.yAxis.Count;

                this.maxY = this.maxY + span / 2;

                this.minY = this.minY - span / 2;
            }
        }
    }

    public class xAxis : Axis
    {
        //  Message：目前不起作用 
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(xAxis), new PropertyMetadata(0.0, (d, e) =>
             {
                 xAxis control = d as xAxis;

                 if (control == null) return;

                 //double config = e.NewValue as double;


                 control.TryDraw();

             }));

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);

            double span = this.TextAlignmentCenter ? this.AlignAlignmentCenter ? 0 : (this.maxX - this.minX) / (this.xAxis.Count) : 0;

            //this.Y = this.DockTop ? this.yAxis.Max() : this.Y;

            //double y = this.DockTop ? this.yAxis.Max() : this.Y;

            double y = this.Y;

            //  Do ：绘制坐标
            foreach (var item in this.xAxis)
            {
                //  Do ：底线
                System.Windows.Shapes.Line yright = new System.Windows.Shapes.Line();
                yright.X1 = 0;
                yright.X2 = this.ActualWidth;
                yright.Y1 = 0;
                yright.Y2 = 0;
                yright.Style = this.LineStyle;

                Canvas.SetBottom(yright, this.GetY(y, this.ActualHeight));

                canvas.Children.Add(yright);


                //  Do ：刻度线
                System.Windows.Shapes.Line l = new System.Windows.Shapes.Line();
                l.X1 = 0;
                l.X2 = 0;
                l.Y1 = 0;
                l.Y2 = this.VerticalAlignment == VerticalAlignment.Top ? -this.AlignLenght : this.AlignLenght;
                l.Style = this.LineStyle;
                Canvas.SetLeft(l, this.GetX(item + span / 2, this.ActualWidth));

                canvas.Children.Add(l);

                //  Do ：显示文本
                Label t = new Label();

                t.Content = this.xDisplay.Count > this.xAxis.IndexOf(item) ? this.xDisplay[this.xAxis.IndexOf(item)] : item.ToString();
                t.Style = this.LabelStyle;

                t.Loaded += (o, e) =>
                {
                    Canvas.SetLeft(t, this.GetX(item, this.ActualWidth) - t.ActualWidth / 2);
                    Canvas.SetTop(t, this.VerticalAlignment == VerticalAlignment.Top ? -(this.AlignLenght + t.ActualHeight) : this.AlignLenght);
                };
                canvas.Children.Add(t);
            }
        }

    }

    public class yAxis : Axis
    {
        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);

            double span = this.TextAlignmentCenter ? this.AlignAlignmentCenter ? 0 : (this.maxY - this.minY) / (this.yAxis.Count) : 0;

            //Y坐标
            foreach (var item in this.yAxis)
            {
                //  Do ：底线
                System.Windows.Shapes.Line xleft = new System.Windows.Shapes.Line();
                xleft.X1 = 0;
                xleft.X2 = 0;
                xleft.Y1 = 0;
                xleft.Y2 = this.ActualHeight;
                xleft.Style = this.LineStyle;
                Canvas.SetLeft(xleft, 0);

                canvas.Children.Add(xleft);

                //  Do ：刻度
                System.Windows.Shapes.Line l = new System.Windows.Shapes.Line();
                l.X1 = 0;
                l.X2 = this.AlignLenght;
                l.Y1 = 0;
                l.Y2 = 0;
                l.Style = this.LineStyle;

                Canvas.SetTop(l, this.GetY(item + span / 2, this.ActualHeight));

                if (this.HorizontalAlignment == HorizontalAlignment.Right)
                {
                    Canvas.SetLeft(l, 0);
                }
                else
                {
                    Canvas.SetRight(l, 0);
                }

                canvas.Children.Add(l);

                // Todo ：绘制文本 
                Label t = new Label();
                t.Content = this.yDisplay.Count > this.yAxis.IndexOf(item) ? this.yDisplay[this.yAxis.IndexOf(item)] : item.ToString();
                t.Style = this.LabelStyle;

                t.Loaded += (o, e) =>
                {
                    Canvas.SetTop(t, this.GetY(item, this.ActualHeight) - t.ActualHeight / 2);
                    Canvas.SetRight(t, this.HorizontalAlignment == HorizontalAlignment.Right ? -(this.AlignLenght + t.ActualWidth) : this.AlignLenght);
                };

                canvas.Children.Add(t);
            }
        }

        //protected override void InitY()
        //{
        //    if (this.Data.Count > 0)
        //    {
        //        this.minY = this.Data.Min();

        //        this.maxY = this.Data.Max();
        //    }
        //}
    }


    /// <summary> 极坐标X轴 </summary>
    public class RadiusAxis : Axis
    {
        public double Len
        {
            get { return (double)GetValue(LenProperty); }
            set { SetValue(LenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LenProperty =
            DependencyProperty.Register("Len", typeof(double), typeof(RadiusAxis), new PropertyMetadata(default(double), (d, e) =>
             {
                 RadiusAxis control = d as RadiusAxis;

                 if (control == null) return;

                 //double config = e.NewValue as double;

                 control.TryDraw();

             }));


        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);

            double span = this.TextAlignmentCenter ? this.AlignAlignmentCenter ? 0 : (this.maxX - this.minX) / (this.xAxis.Count) : 0;

            //  Do ：绘制坐标
            foreach (var item in this.xAxis)
            {
                //  Do ：底线
                System.Windows.Shapes.Line yright = new System.Windows.Shapes.Line();
                yright.X1 = this.ActualWidth / 2;
                yright.X2 = this.ActualWidth / 2 + Len;
                yright.Y1 = 0;
                yright.Y2 = 0;
                yright.Style = this.LineStyle;

                Canvas.SetBottom(yright, this.GetY(0, this.ActualHeight));

                canvas.Children.Add(yright);

                //  Do ：刻度线
                System.Windows.Shapes.Line l = new System.Windows.Shapes.Line();
                l.X1 = 0;
                l.X2 = 0;
                l.Y1 = 0;
                l.Y2 = this.VerticalAlignment == VerticalAlignment.Top ? -this.AlignLenght : this.AlignLenght;
                l.Style = this.LineStyle;
                Canvas.SetLeft(l, this.GetX(item + span / 2, this.Len));

                canvas.Children.Add(l);

                //  Do ：显示文本
                Label t = new Label();

                t.Content = this.xDisplay.Count > this.xAxis.IndexOf(item) ? this.xDisplay[this.xAxis.IndexOf(item)] : item.ToString();
                t.Style = this.LabelStyle;

                t.Loaded += (o, e) =>
                {
                    Canvas.SetLeft(t, this.GetX(item, this.Len) - t.ActualWidth / 2);
                    Canvas.SetTop(t, this.VerticalAlignment == VerticalAlignment.Top ? -(this.AlignLenght + t.ActualHeight) : this.AlignLenght);
                };
                canvas.Children.Add(t);
            }
        }
    }

    /// <summary> 极坐标X轴 </summary>
    public class AngleAxis : Axis
    {
        public double Len
        {
            get { return (double)GetValue(LenProperty); }
            set { SetValue(LenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LenProperty =
            DependencyProperty.Register("Len", typeof(double), typeof(AngleAxis), new PropertyMetadata(default(double), (d, e) =>
            {
                AngleAxis control = d as AngleAxis;

                if (control == null) return;

                //double config = e.NewValue as double;

                control.TryDraw();

            }));


        protected virtual void DrawCircle(Canvas canvas)
        {
            //  Do ：绘制圆环 
            Path path = new Path();

            double item = this.xAxis.Max();

            path.Style = this.LineStyle;

            EllipseGeometry ellipse = new EllipseGeometry(new Rect(0, 0, this.GetX(item, this.Len * 2), this.GetX(item, this.Len * 2)));

            ellipse.Center = new Point(0, 0);

            path.Data = ellipse;

            this.Children.Add(path);
        }

        void DrawLine(Canvas canvas)
        {
            Point center = new Point(0, 0);

            double angle = 360 / this.yAxis.Count;

            for (int i = 0; i < this.yAxis.Count; i++)
            {
                Path path = new Path();
                path.Style = this.LineStyle;

                PathFigure pf = new PathFigure();


                {
                    Point start = new Point(-this.Len, center.Y);

                    Matrix matrix = new Matrix();

                    matrix.RotateAt(angle * i, center.X, center.Y);

                    Point end = matrix.Transform(start);

                    pf.StartPoint = end;

                }

                {
                    Point start = new Point(-this.Len - this.AlignLenght, center.Y);

                    Matrix matrix = new Matrix();

                    matrix.RotateAt(angle * i, center.X, center.Y);

                    Point end = matrix.Transform(start);

                    pf.Segments.Add(new LineSegment(end, true));

                }

                PathGeometry pg = new PathGeometry(new List<PathFigure>() { pf });

                path.Data = pg;

                this.Children.Add(path);


            }
        }

        void DrawText()
        {
            double angle = 360 / this.yAxis.Count;

            for (int i = 0; i < this.yAxis.Count; i++)
            {
                Point center = new Point(0, 0);

                Point start = new Point(this.Len + this.AlignLenght * 4, center.Y);

                Matrix matrix = new Matrix();

                matrix.RotateAt(angle * i, center.X, center.Y);

                Point end = matrix.Transform(start);

                //  Do ：显示文本
                Label t = new Label();

                t.Content = this.yDisplay.Count > i ? this.yDisplay[i] : this.yAxis[i].ToString();
                t.Style = this.LabelStyle;

                double hlen = this.AlignLenght / 10;

                t.Loaded += (o, e) =>
                {
                    double endParam = (end.X > center.X ? hlen + hlen / 2 : -hlen - t.ActualWidth - hlen / 2);

                    Canvas.SetLeft(t, end.X + endParam);
                    Canvas.SetTop(t, end.Y - t.ActualHeight / 2);
                };
                this.Children.Add(t);
            }
        }

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);

            this.DrawCircle(this);

            this.DrawLine(this);

            this.DrawText();
        }
    }


    public class RadarAxis : AngleAxis
    {
        protected override void DrawCircle(Canvas canvas)
        {
            Point center = new Point(0, 0);

            double angle = 360 / this.yAxis.Count;

            Path path = new Path();
            path.Style = this.LineStyle;

            PathFigure pf = new PathFigure();

            pf.IsClosed = true;

            for (int i = 0; i < this.yAxis.Count; i++)
            {
                if (i == 0)
                {
                    pf.StartPoint = new Point(this.Len, center.Y);
                    continue;
                }

                {
                    Point start = new Point(this.Len, center.Y);

                    Matrix matrix = new Matrix();

                    matrix.RotateAt(angle * i, center.X, center.Y);

                    Point end = matrix.Transform(start);

                    pf.Segments.Add(new LineSegment(end, true));
                }
            }

            PathGeometry pg = new PathGeometry(new List<PathFigure>() { pf });

            path.Data = pg;

            this.Children.Add(path);


            path.Loaded += (l, k) =>
            {
                this.RunPath(path,500);
            };
        }
    }

}
