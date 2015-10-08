using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorkerDemo
{
    public class Words
    {
        public class CurrentState
        {
            public int LinesCounted;
            public int WordsMatched;
        }

        public string SourceFile;
        public string CompareString;
        private int WordCount;
        private int LinesCounted;

        public void CountWords(
            System.ComponentModel.BackgroundWorker worker,
            System.ComponentModel.DoWorkEventArgs e)
        {
            CurrentState state = new CurrentState();
            string line = "";
            int elapsedTime = 20;
            DateTime lastReportDateTime = DateTime.Now;

            if (CompareString == null || CompareString == string.Empty) {
                throw new Exception("CompareString not specified.");
            }

            try {
                using (System.IO.StreamReader stream = new System.IO.StreamReader(SourceFile)) {
                    while (!stream.EndOfStream) {
                        if (worker.CancellationPending) {
                            e.Cancel = true;
                            break;
                        }
                        else {
                            line = stream.ReadLine();
                            WordCount += CountInString(line, CompareString);
                            LinesCounted += 1;

                            int compare = DateTime.Compare(
                                DateTime.Now, lastReportDateTime.AddMilliseconds(elapsedTime));
                            if (compare > 0) {
                                state.LinesCounted = LinesCounted;
                                state.WordsMatched = WordCount;
                                worker.ReportProgress(0, state);
                                lastReportDateTime = DateTime.Now;
                            }
                        }
                    }

                    state.LinesCounted = LinesCounted;
                    state.WordsMatched = WordCount;
                    worker.ReportProgress(0, state);
                }
            }
            catch (Exception exc) {
                throw exc;
            }
        }

        private int CountInString(
            string SourceString,
            string CompareString)
        {
            if (SourceString == null) {
                return 0;
            }

            string EscapedCompareString = System.Text.RegularExpressions.Regex.Escape(CompareString);

            System.Text.RegularExpressions.Regex regex;
            regex = new System.Text.RegularExpressions.Regex(
                @"\b" + EscapedCompareString + @"\b",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            System.Text.RegularExpressions.MatchCollection matches;
            matches = regex.Matches(SourceString);
            return matches.Count;
        }
    }
}
