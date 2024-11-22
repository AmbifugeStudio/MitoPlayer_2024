using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using Mysqlx.Crud;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Views
{
    public interface IChartView
    {
        event EventHandler<EventArgs> AnalyseTrackEvent;
        event EventHandler<EventArgs> CancelAnalysationEvent;
        event EventHandler<Messenger> SetCurrentFeatureTypeEvent;

        void InitializeTagsAndTagValues(List<Tag> tagList, Dictionary<string, Dictionary<string, Color>> tagValueDictionary);
        void InitializeTrackList(DataTableModel model);
        void UpdatePlot(PlotView plot);
        void UpdateSoundWavePlot(PlotView plot);
        void UpdateViewAfterCancel();
        void StartAnalysation();
        void UpdateProgressOnView(Messenger item);
        void FinishAnalysation();
    }
}
