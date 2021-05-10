using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Tukupedia.Helpers.Utils
{
    public class TransitionData
    {
        private FrameworkElement elem;
        private readonly Thickness targetMargin;
        private readonly double targetOpacity;
        private readonly double speedMargin;
        private readonly double speedOpacity;
        private readonly double deltaMargin;
        private readonly double deltaOpacity;

        public TransitionData(
            FrameworkElement feedElem,
            Thickness feedTargetMargin,
            double feedTargetOpacity,
            double feedSpeedMargin,
            double feedSpeedOpacity)
        {
            elem = feedElem;
            targetMargin = feedTargetMargin;
            targetOpacity = feedTargetOpacity;
            speedMargin = feedSpeedMargin;
            speedOpacity = feedSpeedOpacity;

            deltaMargin = (
                Math.Abs(elem.Margin.Top - targetMargin.Top) +
                Math.Abs(elem.Margin.Left - targetMargin.Left) +
                Math.Abs(elem.Margin.Right - targetMargin.Right) +
                Math.Abs(elem.Margin.Bottom - targetMargin.Bottom)
                ) / 4 * (1.0 / 100.0);
            deltaOpacity = Math.Abs(elem.Opacity - targetOpacity) * 1.0 / 100.0;
        }

        public bool move()
        {
            Thickness tmp = elem.Margin;

            //base case
            if (Math.Abs(tmp.Top - targetMargin.Top) +
                Math.Abs(tmp.Left - targetMargin.Left) +
                Math.Abs(tmp.Right - targetMargin.Right) +
                Math.Abs(tmp.Bottom - targetMargin.Bottom) < deltaMargin)
            {
                elem.Margin = targetMargin;
            }
            if (Math.Abs(elem.Opacity - targetOpacity) < deltaOpacity)
            {
                elem.Opacity = targetOpacity;
            }
            if (elem.Margin == targetMargin &&
                elem.Opacity == targetOpacity)
            {
                return true;
            }

            //margin
            tmp.Top = (tmp.Top + speedMargin * targetMargin.Top) / (1 + speedMargin);
            tmp.Left = (tmp.Left + speedMargin * targetMargin.Left) / (1 + speedMargin);
            tmp.Right = (tmp.Right + speedMargin * targetMargin.Right) / (1 + speedMargin);
            tmp.Bottom = (tmp.Bottom + speedMargin * targetMargin.Bottom) / (1 + speedMargin);
            elem.Margin = tmp;

            //opacity
            elem.Opacity = (elem.Opacity + speedOpacity * targetOpacity) / (1 + speedOpacity);

            return false;
        }
    }

    public class TransitionQueue
    {
        private List<List<TransitionData>> transQueue;
        private int idxNow = -1;

        public void addElementTransition
            (FrameworkElement cons, Thickness targetMargin, double targetOpacity, double speedMargin, double speedOpacity, string order)
        {
            if (order != "after previous" && order != "with previous")
            {
                Console.WriteLine("Warning : unknown order");
                return;
            }

            if (idxNow == -1)
            {
                transQueue = new List<List<TransitionData>>();
            }

            if (idxNow == -1 || order == "after previous")
            {
                transQueue.Add(new List<TransitionData>());
                idxNow += 1;
            }

            transQueue[idxNow].Add(new TransitionData(cons, targetMargin, targetOpacity, speedMargin, speedOpacity));
        }

        public bool tick()
        {
            bool finishLine = true;

            foreach (TransitionData td in transQueue[0])
            {
                finishLine &= td.move();
            }

            if (finishLine && transQueue.Count == 1)
            {
                return true;
            }
            if (finishLine) transQueue.RemoveAt(0);

            return false;
        }
    }

    public class Transition
    {
        public delegate void transitionCallbackDelegate();

        private readonly int transitionFPS;
        private DispatcherTimer transitionTimer;
        private TransitionQueue transQueue;
        private transitionCallbackDelegate transCallback;
        bool reset = true;

        public Transition(int FPS)
        {
            transitionTimer = new DispatcherTimer();
            transitionFPS = FPS;
        }

        public void makeTransition(
            FrameworkElement element,
            Thickness targetMargin,
            double targetOpacity,
            double speedMargin,
            double speedOpacity,
            string order)
        {
            if (reset)
            {
                transQueue = new TransitionQueue();
                breakTimer();
                reset = false;
            }

            transQueue.addElementTransition(
                element,
                targetMargin,
                targetOpacity,
                speedMargin,
                speedOpacity,
                order);
        }

        public void setCallback(transitionCallbackDelegate callback)
        {
            transCallback = callback;
        }

        public void playTransition()
        {
            transitionTimer.Interval = TimeSpan.FromMilliseconds(1000.0 / transitionFPS);
            transitionTimer.Tick += tickTransition;
            transitionTimer.Start();
            reset = true;
        }

        public void tickTransition(object sender, EventArgs e)
        {
            bool finish = transQueue.tick();

            if (finish)
            {
                breakTimer();
                if (transCallback != null)
                {
                    transCallback();
                    transCallback = null;
                }
            }
        }

        private void breakTimer()
        {
            if (transitionTimer != null) transitionTimer.Stop();

            transitionTimer = new DispatcherTimer();
        }
    }
}
