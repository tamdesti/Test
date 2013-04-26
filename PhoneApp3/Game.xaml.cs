using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;

namespace PhoneApp3
{
    public partial class Game : PhoneApplicationPage
    {
        List<Button> Arraybtn = new List<Button>();
        bool haschoice = false;
        int IndexbtnChoice = -1;
        SolidColorBrush color = new SolidColorBrush();
        List<bool> delList = new List<bool>();
        int Scores = 0;
        Random rnd = new Random();
        public Game()
        {
            InitializeComponent();
            AddButton();
            checkIT(-1,-1);
        }
        public void checkIT(int a,int b)
        {
            for (int i=0; i<8; i++)
                for (int j = 0; j < 6; j++)
                    checkRow(i*8 + j, 1);
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 8; j++)
                    checkCol(i * 8 + j, 1);
            changecolor(a,b);
        }
        bool change = false;

        void animationColor(String btnName)
        {
            ColorAnimation colorAnimation = new ColorAnimation();
            colorAnimation.From = Colors.Red;
            colorAnimation.To = Colors.Green;
            colorAnimation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(colorAnimation, recordtxt.Name);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));
            Storyboard mouseEnterStoryboard = new Storyboard();
            mouseEnterStoryboard.Children.Add(colorAnimation);
            mouseEnterStoryboard.Begin();
            //DoubleAnimation d = new DoubleAnimation();
        }
        public void changecolor(int a,int b)
        {
            String S = "";
            int n=8;
            for (int i=0; i<n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (delList[i * 8 + j])
                    {
                        delList[i * 8 + j] = false;
                        Button btn = Arraybtn[i * 8 + j];
                        setBackground4button(ref btn);
                        if (a!= -1)
                        animationColor(btn.Name);
                        Arraybtn[i * 8 + j] = btn;
                        Scores++;
                        change = true;
                        S += (i * 8 + j).ToString() + " ";
                    }
                }
            scoretxt.Text = Scores.ToString();
            if (change)
            {
                change = false;
                checkIT(-1, -1);
                recordtxt.Text += S;
            }
            else
                if (a != -1)
                {
                    SolidColorBrush swapcolor = (SolidColorBrush)Arraybtn[a].Background;
                    SolidColorBrush selectcolor = (SolidColorBrush)Arraybtn[b].Background;
                    Arraybtn[b].Background = swapcolor;
                    Arraybtn[a].Background = selectcolor;
                }
        }
        void setBackground4button(ref Button btn)
        {
            switch (rnd.Next(0, 5))
            {
                case 0:
                    btn.Background = new SolidColorBrush(Colors.Red);
                    btn.Content = "0";
                    break;
                case 1:
                    btn.Background = new SolidColorBrush(Colors.Green);
                    btn.Content = "1";
                    break;
                case 2:
                    btn.Background = new SolidColorBrush(Colors.Blue);
                    btn.Content = "2";
                    break;
                case 3:
                    btn.Background = new SolidColorBrush(Colors.Yellow);
                    btn.Content = "3";
                    break;
                default:
                    btn.Background = new SolidColorBrush(Colors.White);
                    btn.Content = "4";
                    break;
            }
        }
        public void AddButton()
        {
            int n=8;
            for (int i=0; i<n; i++)
                for (int j = 0; j < n; j++)
                {
                    delList.Add(false);
                    Button newbtn = new Button();
                    newbtn.Name = "btn" + i.ToString() + j.ToString();
                    newbtn.Height = 70;
                    newbtn.Width = 70;
                    newbtn.HorizontalAlignment = HorizontalAlignment.Left;
                    newbtn.VerticalAlignment = VerticalAlignment.Top;
                    newbtn.Margin = new Thickness(50*j, 50*i, 10, 10);
                    newbtn.Click += new RoutedEventHandler(newbtn_Click);
                    setBackground4button(ref newbtn);
                    Arraybtn.Add(newbtn);
                    ContentPanel.Children.Add(newbtn);
                }
        }

        void newbtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (!haschoice)
            {
                color = (SolidColorBrush)btn.Background;
                btn.Background = new SolidColorBrush(Colors.Black);
                IndexbtnChoice = Arraybtn.IndexOf(btn);
                haschoice = true;
            }
            else
            {
                int indexbtnswap = Arraybtn.IndexOf(btn);
                haschoice = false;
                if (indexbtnswap == IndexbtnChoice || !Keben(indexbtnswap,IndexbtnChoice))
                {
                    Arraybtn[IndexbtnChoice].Background = color;
                    return;
                }
                SolidColorBrush swapcolor = (SolidColorBrush)Arraybtn[indexbtnswap].Background;
                Arraybtn[IndexbtnChoice].Background = swapcolor;
                Arraybtn[indexbtnswap].Background = color;
                checkIT(indexbtnswap,IndexbtnChoice);
            }
        }
        bool Keben(int a, int b)
        {
            if (a > b) { a = a + b; b = a - b; a = a - b; }
            if (b == a + 8 || b == a + 1) return true;
            return false;
        }
        
        void checkRow(int index, int totalBlock)
        {
            if (index % 8 != 7 && (((SolidColorBrush)Arraybtn[index + 1].Background).Color == ((SolidColorBrush)Arraybtn[index].Background).Color))
                checkRow(index + 1, totalBlock + 1);
            else
                if (totalBlock >= 3)
                {
                    for (int i = 0; i < totalBlock; i++)
                    {
                        delList[index - i] = true;
                    }
                }
        }
        void checkCol(int index, int totalBlock)
        {
            if (index / 8 != 7 && (((SolidColorBrush)Arraybtn[index + 8].Background).Color == ((SolidColorBrush)Arraybtn[index].Background).Color))
                checkCol(index + 8, totalBlock + 1);
            else
                if (totalBlock >= 3)
                {
                    for (int i = 0; i < totalBlock; i++)
                    {
                        delList[index - i*8] = true;
                    }
                }

        }

        private void ResetRecord(object sender, RoutedEventArgs e)
        {
            recordtxt.Text = "";
            colorStoryboard.Begin();
        }
    }
}