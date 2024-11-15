using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using NAudio.Dsp;
using NAudio.Wave;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MitoPlayer_2024.Presenters.ModelTrainerPresenter;
using System.Drawing;
using OxyPlot.Annotations;
using System.Threading;

namespace MitoPlayer_2024.Presenters
{
    public class ChartPresenter
    {
        private IChartView view;
        private ITagDao tagDao { get; set; }
        private ITrackDao trackDao { get; set; }
        private ISettingDao settingDao { get; set; }
        private MediaPlayerComponent mediaPlayerComponent { get; set; }
        
        private FeatureType currentFeatureType { get; set; }
        private List<Tag> TagList { get; set; }
        private Track currentTrack { get; set; }
        private Dictionary<String, Dictionary<String, Color>> tagValueDictionary { get; set; }
        private List<Tag> tagList { get; set; }
        private DataTable tracklistTable { get; set; }
        private BindingSource tracklistBindingSource { get; set; }
       
        public ChartPresenter(IChartView view, MediaPlayerComponent mediaPlayer, ITagDao tagDao, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.view = view;
            this.tagDao = tagDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;
            this.mediaPlayerComponent = mediaPlayer;

            this.view.AnalyseTrackEvent += AnalyseTrackEvent;
            this.view.CancelAnalysationEvent += CancelAnalysationEvent;
            this.view.SetCurrentFeatureTypeEvent += SetCurrentFeatureTypeEvent;

            this.currentFeatureType = FeatureType.Chroma;
            
            this.InitializeTagsAndTagValues();
            this.InitializeTrack();
            this.InitializeTracklistStructure();
            this.InitializeTracklistContent();
            this.InitializeTrackList();
            this.InitializeColors();

            this.messenger = new ObservableConcurrentBag<Messenger>();
            this.messenger.ItemAdded += Messenger_ItemAdded;
        }

        private OxyColor BackgroundColor { get; set; }
        private OxyColor FontColor { get; set; }
        private OxyColor ButtonColor { get; set; }
        private OxyColor ButtonBorderColor { get; set; }
        private OxyColor ActiveColor { get; set; }
        private void InitializeColors()
        {
            this.BackgroundColor = OxyColor.Parse("#363639");
            this.FontColor = OxyColor.Parse("#c6c6c6");
            this.ButtonColor = OxyColor.Parse("#292a2d");
            this.ButtonBorderColor = OxyColor.Parse("#1b1b1b");
            this.ActiveColor = OxyColor.Parse("#FFBF80");
        }

        private void InitializeTagsAndTagValues()
        {
            this.TagList = this.tagDao.GetAllTag().Where(x => !x.HasMultipleValues).ToList();
            this.tagValueDictionary = new Dictionary<String, Dictionary<String, Color>>();

            List<Tag> tagList = this.tagDao.GetAllTag();
            if (tagList != null && tagList.Count > 0)
            {
                this.tagList = tagList;

                List<TagValue> tagValueList = new List<TagValue>();

                foreach (Tag tag in this.tagList)
                {
                    Dictionary<String, Color> tvDic = new Dictionary<String, Color>();

                    tagValueList = this.tagDao.GetTagValuesByTagId(tag.Id);
                    if (tagValueList != null && tagValueList.Count > 0)
                    {
                        foreach (TagValue tv in tagValueList)
                        {
                            tvDic.Add(tv.Name, tv.Color);
                        }
                        this.tagValueDictionary.Add(tag.Name, tvDic);
                    }
                }
            }
            this.view.InitializeTagsAndTagValues(this.tagList, this.tagValueDictionary);
        }
        private void InitializeTrack()
        {
            if (this.mediaPlayerComponent != null && this.mediaPlayerComponent.CurrentTrackIdInPlaylist != -1 && this.tagList != null && this.tagList.Count > 0)
            {
                PlaylistContent plc = this.trackDao.GetPlaylistContentByTrackIdInPlaylist(this.mediaPlayerComponent.CurrentTrackIdInPlaylist);
                if (plc != null)
                {
                    Track track = this.trackDao.GetTrackWithTags(plc.TrackId, this.tagList);
                    if(track != null)
                    {
                        track.TrackIdInPlaylist = plc.TrackIdInPlaylist;
                        this.currentTrack = track;
                    }
                }
            }
        }
        private void InitializeTracklistStructure()
        {
            this.tracklistBindingSource = new BindingSource();
            this.tracklistTable = new DataTable();

            List<TrackProperty> tpList = new List<TrackProperty>();
            tpList = this.settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.TracklistColumns.ToString());
            if (tpList != null && tpList.Count > 0)
            {
                for (int i = 0; i <= tpList.Count - 1; i++)
                {
                    if(tpList[i].Name == "Id" || tpList[i].Name =="Artist" || tpList[i].Name == "Title" || tpList[i].Name == "Length" ||
                        (i > 10 && !tpList[i].Name.Contains("TagValueId")))
                    {
                        this.tracklistTable.Columns.Add(tpList[i].Name, Type.GetType(tpList[i].Type));
                    }                    
                }
            }
        }
        private void InitializeTracklistContent()
        {
            this.tracklistTable.Clear();

            if (this.currentTrack != null)
            {
                DataRow dataRow = this.tracklistTable.NewRow();
                dataRow["Id"] = this.currentTrack.Id;
                dataRow["Artist"] = this.currentTrack.Artist;
                dataRow["Title"] = this.currentTrack.Title;
                dataRow["Length"] = this.LengthToString(this.currentTrack.Length);

                if (this.currentTrack.TrackTagValues != null)
                {
                    foreach (TrackTagValue ttv in this.currentTrack.TrackTagValues)
                    {
                        if (ttv.HasMultipleValues)
                        {
                            dataRow[ttv.TagName] = ttv.Value;
                        }
                        else
                        {
                            dataRow[ttv.TagName] = ttv.TagValueName;
                        }
                    }
                }

                this.tracklistTable.Rows.Add(dataRow);
            }
        }
        private String LengthToString(double length)
        {
            TimeSpan t = TimeSpan.FromSeconds(length);
            String lengthInString = "";

            if (t.Hours > 0)
            {
                lengthInString = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
            }
            else
            {
                if (t.Minutes > 0)
                {
                    lengthInString = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
                }
                else
                {
                    lengthInString = string.Format("{0:D2}:{1:D2}", 0, t.Seconds);
                }

            }
            return lengthInString;
        }
        private void InitializeTrackList()
        {
            this.tracklistBindingSource.DataSource = this.tracklistTable;

            DataTableModel model = new DataTableModel()
            {
                BindingSource = this.tracklistBindingSource
            };

            this.view.InitializeTrackList(model);
        }

        private void SetCurrentFeatureTypeEvent(object sender, Messenger e)
        {
            switch (e.StringField1)
            {
                case "Chroma": currentFeatureType = FeatureType.Chroma; break;
                case "MFCCs": currentFeatureType = FeatureType.MFCCs; break;
                case "HPCP": currentFeatureType = FeatureType.HPCP; break;
                case "HPSS": currentFeatureType = FeatureType.HPSS; break;
                case "SpectralContrast": currentFeatureType = FeatureType.SpectralContrast; break;
                case "SpectralCentroid": currentFeatureType = FeatureType.SpectralCentroid; break;
                case "SpectralBandwidth": currentFeatureType = FeatureType.SpectralBandwidth; break;
                case "Tonnetz": currentFeatureType = FeatureType.Tonnetz; break;
                case "ZCR": currentFeatureType = FeatureType.ZCR; break;
                case "RMS": currentFeatureType = FeatureType.RMS; break;
                case "Pitch": currentFeatureType = FeatureType.Pitch; break;
            }
        }


        private ObservableConcurrentBag<Messenger> messenger { get; set; }
        private CancellationTokenSource cancellationToken { get; set; }
        private void Messenger_ItemAdded(object sender, ItemAddedEventArgs<Messenger> e)
        {
            this.view.UpdateProgressOnView(e.Item);

            if (this.messenger.Count > 100)
            {
                while (!this.messenger.IsEmpty)
                {
                    this.messenger.TryTake(out _);
                }
            }
        }

        private void AnalyseTrackEvent(object sender, EventArgs e)
        {
            if (this.currentTrack != null)
            {
                if (this.currentFeatureType != FeatureType.Empty)
                {
                    this.cancellationToken = new CancellationTokenSource();
                    this.AnalyseTrack(this.currentTrack, cancellationToken.Token);
                }
            }
        }

        List<float[]> chromaFeatures = new List<float[]>();
        private async void AnalyseTrack(Track track, CancellationToken cancellationToken)
        {
            try
            {
                this.view.StartAnalysation();
                await AnalyseTrackAsync(track, cancellationToken);
            }
            finally
            {
                this.view.FinishAnalysation();
                if (currentFeatureType == FeatureType.Chroma) {
                    this.PlotChromaFeatures(chromaFeatures, track.Length);
                }
            }
        }

        
        private async Task AnalyseTrackAsync(Track track, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (currentFeatureType == FeatureType.Chroma)
                    {
                        using (var reader = new AudioFileReader(track.Path))
                        {
                            chromaFeatures = this.ExtractChromaFeatures(track.Path);
                        }
                    } 
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });
        }

        private void CancelAnalysationEvent(object sender, EventArgs e)
        {
            if(this.cancellationToken != null)
            {
                this.cancellationToken.Cancel();
                this.view.UpdateViewAfterCancel();
            }
        }

        public List<float[]> ExtractChromaFeatures(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                int sampleRate = reader.WaveFormat.SampleRate;
                int bufferSize = 1024; // Process in chunks of 4096 samples
               // int overlap = bufferSize / 2; // 50% overlap
                float[] buffer = new float[bufferSize];
                List<float[]> chromaFeaturesList = new List<float[]>();

                int bytesRead;
                float[] pitchHistogram = new float[12]; // Initialize pitch histogram
                long totalBytes = reader.Length;
                long processedBytes = 0;

                // Buffer to hold the overlapping samples
               // float[] overlapBuffer = new float[overlap];

                while ((bytesRead = sampleProvider.Read(buffer, 0, bufferSize)) > 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    // Apply windowing function
                    for (int i = 0; i < bufferSize; i++)
                    {
                        buffer[i] *= (float)(0.5 * (1 - Math.Cos(2 * Math.PI * i / (bufferSize - 1))));
                    }

                    // Apply FFT
                    Complex[] complexBuffer = new Complex[bufferSize];
                    for (int i = 0; i < bufferSize; i++)
                    {
                        complexBuffer[i].X = buffer[i];
                        complexBuffer[i].Y = 0;
                    }
                    FastFourierTransform.FFT(true, (int)Math.Log(bufferSize, 2.0), complexBuffer);

                    // Calculate chroma features
                    float[] chroma = new float[12];
                    for (int i = 0; i < complexBuffer.Length / 2; i++)
                    {
                        double frequency = i * sampleRate / bufferSize;
                        if (frequency > 0) // Avoid log of zero or negative
                        {
                            int note = (int)Math.Round(12 * Math.Log(frequency / 440.0, 2)) % 12;
                            if (note < 0) note += 12;
                            chroma[note] += (float)Math.Sqrt(complexBuffer[i].X * complexBuffer[i].X + complexBuffer[i].Y * complexBuffer[i].Y);
                            pitchHistogram[note] += chroma[note]; // Aggregate pitch histogram
                        }
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }



                    chromaFeaturesList.Add(chroma);

                    // Copy the last part of the buffer to the overlap buffer
                  /*  Array.Copy(buffer, bufferSize - overlap, overlapBuffer, 0, overlap);

                    // Read the next buffer, starting with the overlap buffer
                    bytesRead = sampleProvider.Read(buffer, overlap, bufferSize - overlap);
                    Array.Copy(overlapBuffer, 0, buffer, 0, overlap);*/

                    processedBytes = reader.Position;
                    double progress = (double)processedBytes / totalBytes * 100;
                    Console.WriteLine($"Processing: {progress:F2}%");

                    this.messenger.Add(new Messenger()
                    {
                        IntegerField1 = (int)progress
                    });
                }

                // Normalize pitch histogram
                Console.WriteLine("Normalizing pitch histogram...");
                float maxPitchValue = pitchHistogram.Max();
                if (maxPitchValue > 0)
                {
                    for (int i = 0; i < pitchHistogram.Length; i++)
                    {
                        pitchHistogram[i] /= maxPitchValue;
                    }
                }

                // Weight chroma features by pitch histogram
                Console.WriteLine("Weighting chroma features by pitch histogram...");
                for (int i = 0; i < chromaFeaturesList.Count; i++)
                {
                    for (int j = 0; j < chromaFeaturesList[i].Length; j++)
                    {
                        chromaFeaturesList[i][j] *= pitchHistogram[j];
                    }
                }

                return chromaFeaturesList;
            }

        }

        private LineAnnotation verticalLine { get; set; }
        
        
        private void PlotChromaFeatures(List<float[]> chromaFeatures, double songLengthInSeconds)
        {
            var plotView = new PlotView
            {
                Dock = DockStyle.Fill
            };

            var plotModel = new PlotModel { Title = "Chroma Features Over Time" };
            plotModel.TitleColor = this.FontColor;

            // Find a reasonable maximum value in the chroma features
            double maxValue = this.FindReasonableMaxValue(chromaFeatures);

            // Add a color axis
            var colorAxis = new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Jet(500),
                HighColor = OxyColors.Gray,
                LowColor = OxyColors.Black,
                Minimum = 0,
                Maximum = maxValue // Set the maximum based on a reasonable value
            };
            plotModel.Axes.Add(colorAxis);

            // Add X axis (Time)
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Time (seconds)",
                Minimum = 0,
                Maximum = songLengthInSeconds, // Set the maximum to the length of the song in seconds
                MajorStep = 15, // Major ticks every 15 seconds
                MinorStep = 5, // Minor ticks every 5 seconds
                LabelFormatter = value => TimeSpan.FromSeconds(value).ToString(@"mm\:ss"), // Correct format for minutes:seconds
                TitleColor = this.FontColor, // Change the axis title color to blue
                TextColor = this.FontColor, // Change the axis text color to green
                TicklineColor = this.FontColor 
            };
            plotModel.Axes.Add(xAxis);

            // Add Y axis (Chroma Features)
            var yAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title = "Chroma Features",
                Key = "ChromaAxis",
                ItemsSource = new[]
                {
                    "C (A01)", "C# (A02)", "D (A03)", "D# (A04)", "E (A05)", "F (A06)", "F# (A07)", "G (A08)", "G# (A09)", "A (A10)", "A# (A11)", "B (A12)"
                },
                TitleColor = this.FontColor, // Change the axis title color to blue
                TextColor = this.FontColor, // Change the axis text color to green
                TicklineColor = this.FontColor
            };
            plotModel.Axes.Add(yAxis);

            var heatMapSeries = new HeatMapSeries
            {
                X0 = 0,
                X1 = chromaFeatures.Count, // Set X1 to the number of chroma feature frames
                Y0 = 0,
                Y1 = 12,
                Interpolate = false,
                RenderMethod = HeatMapRenderMethod.Rectangles
            };

            double[,] data = new double[chromaFeatures.Count, 12];
            for (int x = 0; x < chromaFeatures.Count; x++)
            {
                for (int y = 0; y < 12; y++)
                {
                    data[x, y] = chromaFeatures[x][y];
                }
            }
            heatMapSeries.Data = data;
            plotModel.Series.Add(heatMapSeries);

            // Add the vertical line annotation
            verticalLine = new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                X = 0, // Initial position
                Color = OxyColors.Orange,
                LineStyle = LineStyle.Solid,
                StrokeThickness = 2,
                Text = "Current Position",
                TextColor = OxyColors.Orange,
                TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Left,
                TextVerticalAlignment = VerticalAlignment.Top,
                TextMargin = 5
            };
            plotModel.Annotations.Add(verticalLine);

            plotView.Model = plotModel;

            this.view.UpdatePlot(plotView);
        }
        public void UpdateLinePosition(double currentTimeInSeconds)
        {

            if(this.currentTrack == null || this.currentTrack.TrackIdInPlaylist != this.mediaPlayerComponent.CurrentTrackIdInPlaylist)
            {
                currentTimeInSeconds = 0;
            }

            if (verticalLine != null)
            {
                verticalLine.X = currentTimeInSeconds;
                verticalLine.PlotModel.InvalidatePlot(false); // Redraw the plot without updating the data
            }

        }
        private double FindReasonableMaxValue(List<float[]> chromaFeatures, double percentile = 0.99)
        {
            List<float> allValues = new List<float>();
            foreach (var chroma in chromaFeatures)
            {
                allValues.AddRange(chroma);
            }
            allValues.Sort();
            int index = (int)(allValues.Count * percentile);
            return allValues[index];
        }
        private static double Log2(double x)
        {
            return Math.Log(x) / Math.Log(2);
        }
    }


}
