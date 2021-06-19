using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSFCS.DMDS.Client.Model
{
    public class CurveAxisModel
    {
        #region Field
        public int Curve { get; set; }
        public string Title { get; set; }
        public string XTitle { get; set; }
        public string XStringFormat { get; set; }
        public string YTitle { get; set; }
        public double YMajorStep { get; set; }
        public double YMinimum { get; set; }
        public double YMaximum { get; set; }
        #endregion
    }
}
