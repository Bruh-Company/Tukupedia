using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tukupedia.ViewModels
{
    public class TransitionData
    {
        private Control elem;
        private readonly Thickness targetMargin;
        private readonly double targetOpacity;

        public TransitionData(Control feedElem, Thickness feedTargetMargin, double feedTargetOpacity)
        {
            elem = feedElem;
            targetMargin = feedTargetMargin;
            targetOpacity = feedTargetOpacity;
        }

        public bool move()
        {
            Thickness tmp = elem.Margin;

            //base case
            if (Math.Abs(tmp.Top - targetMargin.Top) +
                Math.Abs(tmp.Left - targetMargin.Left) +
                Math.Abs(tmp.Right - targetMargin.Right) +
                Math.Abs(tmp.Bottom - targetMargin.Bottom) < 1)
            {
                elem.Margin = targetMargin;
            }
            if (Math.Abs(elem.Opacity - targetOpacity) < 0.5)
            {
                elem.Opacity = targetOpacity;
            }
            if (elem.Margin == targetMargin &&
                elem.Opacity == targetOpacity)
            {
                return true;
            }

            const short
                lhsM = 4, //start
                rhsM = 1; //target
            const short
                lhsO = 3, //start
                rhsO = 1; //target

            //margin
            tmp.Top = (lhsM * tmp.Top + rhsM * targetMargin.Top) / (lhsM + rhsM);
            tmp.Left = (lhsM * tmp.Left + rhsM * targetMargin.Left) / (lhsM + rhsM);
            tmp.Right = (lhsM * tmp.Right + rhsM * targetMargin.Right) / (lhsM + rhsM);
            tmp.Bottom = (lhsM * tmp.Bottom + rhsM * targetMargin.Bottom) / (lhsM + rhsM);
            elem.Margin = tmp;

            //opacity
            elem.Opacity = (lhsO * elem.Opacity + rhsO * targetOpacity) / (lhsO + rhsO);

            return false;
        }
    }

    public class TransitionQueue
    {
        private List<List<TransitionData>> transQueue;
        private int idxNow = -1;

        public void addControlTransition
            (Control cons, Thickness targetMargin, double targetOpacity, string order)
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

            transQueue[idxNow].Add(new TransitionData(cons, targetMargin, targetOpacity));
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
}
