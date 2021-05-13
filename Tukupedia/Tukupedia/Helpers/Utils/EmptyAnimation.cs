using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Tukupedia.Helpers.Utils
{
    class EmptyAnimation
    {
        private class AnimData
        {
            private FrameworkElement elem;
            private double loopIdx;
            private const double __LIMIT__ = 2 * Math.PI;
            private readonly Thickness startMargin;

            public AnimData(FrameworkElement feedElem)
            {
                elem = feedElem;
                loopIdx = 0;
                startMargin = elem.Margin;
            }

            public bool move()
            {
                if (loopIdx >= __LIMIT__)
                {
                    elem.Margin = startMargin;
                    return true;
                }

                double gap = 20 * Math.Sin(3 * loopIdx) / (Math.Pow(loopIdx + Math.PI / 12, 2) + Math.PI / 2);

                elem.Margin = new Thickness(startMargin.Left - gap, startMargin.Top, startMargin.Right + gap, startMargin.Bottom);

                loopIdx += 0.2;

                return false;
            }
        }

        public delegate void animCallbackDelegate();

        private readonly int animFPS;
        private DispatcherTimer animTimer;
        private List<AnimData> animList;
        private animCallbackDelegate animCallback;
        private bool status;

        public EmptyAnimation(int FPS)
        {
            animTimer = new DispatcherTimer();
            animFPS = FPS;
            status = false;
        }

        public void makeAnimation(List<Control> liFrEl)
        {
            if (status) return;
            animList = new List<AnimData>();
            foreach (Control nn in liFrEl)
            {
                animList.Add(new AnimData(nn));
            }
        }

        public void setCallback(animCallbackDelegate callback)
        {
            animCallback = callback;
        }

        public void playAnim()
        {
            if (status) return;
            status = true;
            animTimer.Interval = TimeSpan.FromMilliseconds(1000.0 / animFPS);
            animTimer.Tick += tickAnim;
            animTimer.Start();
        }

        public void tickAnim(object sender, EventArgs e)
        {
            bool finish = true;

            foreach (AnimData nn in animList)
            {
                finish &= nn.move();
            }

            if (finish)
            {
                breakTimer();
                if (animCallback != null)
                {
                    animCallback();
                    animCallback = null;
                }
            }
        }

        private void breakTimer()
        {
            if (animTimer != null) animTimer.Stop();

            animTimer = new DispatcherTimer();

            status = false;
        }
    }
}
