#region Imports

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Pilgrims.ProjectManagement.Contracts.Classes
{
    public class BackgroundWorkHelper
    {
        private readonly ValueMonitor<int> _percentageProgress = new ValueMonitor<int>(0);
        private readonly ValueMonitor<TimeSpan> _timeLeft = new ValueMonitor<TimeSpan>(TimeSpan.MaxValue);
        private DateTime _startTime;
        private List<Action> _toDo;
        private BackgroundWorker _worker;

        public BackgroundWorkHelper()
        {
            IsParallel = false;
            BackgroundWorker.WorkerReportsProgress = true;
            BackgroundWorker.WorkerSupportsCancellation = true;
            _percentageProgress.ValueChanged += percentageProgress_ValueChanged;

            BackgroundWorker.DoWork += worker_DoWork;
        }

        public BackgroundWorkHelper(List<Action> actionsToDo)
            : this()
        {
            _toDo = actionsToDo;
        }

        public BackgroundWorker BackgroundWorker
        {
            get { return _worker ?? (_worker = new BackgroundWorker()); }
        }

        public bool IsParallel { get; set; }

        public IValueMonitor<TimeSpan> TimeLeft
        {
            get { return _timeLeft; }
        }

        public int Total
        {
            get { return _toDo == null ? 0 : _toDo.Count; }
        }

        public void SetActionsTodo(List<Action> toDoActions, bool cancelCurrent = false)
        {
            if (BackgroundWorker.IsBusy && cancelCurrent)
                BackgroundWorker.CancelAsync();
            BackgroundWorker.DoWork -= worker_DoWork;
            BackgroundWorker.DoWork += worker_DoWork;
            BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            _toDo = toDoActions;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ((BackgroundWorker) sender).Dispose();
        }

        private void percentageProgress_ValueChanged(int oldValue, int newValue)
        {
            BackgroundWorker.ReportProgress(newValue);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_toDo == null) throw new InvalidOperationException("You must provide actions to execute");
            Thread.Sleep(10);
            var total = _toDo.Count;
            _startTime = DateTime.Now;
            var current = 0;
            if (IsParallel == false)
                foreach (var next in _toDo)
                {
                    next();
                    current++;
                    if (_worker.CancellationPending) return;
                    _percentageProgress.Value = (int) (current / (double) total * 100.0);
                    var passedMs = (DateTime.Now - _startTime).TotalMilliseconds;
                    var oneUnitMs = passedMs / current;
                    var leftMs = (total - current) * oneUnitMs;
                    _timeLeft.Value = TimeSpan.FromMilliseconds(leftMs);
                }
            else
                try
                {
                    Parallel.For(0, total,
                        (index, loopstate) =>
                        {
                            _toDo.ElementAt(index)();
                            if (_worker.CancellationPending) loopstate.Stop();
                            Interlocked.Increment(ref current);

                            _percentageProgress.Value = (int) (current / (double) total * 100.0);
                            var passedMs = (DateTime.Now - _startTime).TotalMilliseconds;
                            var oneUnitMs = passedMs / current;
                            var leftMs = (total - current) * oneUnitMs;
                            _timeLeft.Value = TimeSpan.FromMilliseconds(leftMs);
                        });
                }
                catch (Exception ex)
                {
                   Functions.ShowMessage(ex, "Background Action Error");
                }
        }
    }
}