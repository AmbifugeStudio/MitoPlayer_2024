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
using Accord.Math.Transforms;
using Accord.Math;
using System.IO;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics;

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

        private List<Tag> tagList { get; set; }
        private Dictionary<String, Dictionary<String, Color>> tagValueDictionary { get; set; }
        private Track currentTrack { get; set; }
        private BindingSource tracklistBindingSource { get; set; }
        private DataTable tracklistTable { get; set; }
       
        private OxyColor BackgroundColor { get; set; }
        private OxyColor FontColor { get; set; }
        private OxyColor ButtonColor { get; set; }
        private OxyColor ButtonBorderColor { get; set; }
        private OxyColor ActiveColor { get; set; }

        private ObservableConcurrentBag<Messenger> messenger { get; set; }
        private CancellationTokenSource cancellationToken { get; set; }

        public ChartPresenter(IChartView view, MediaPlayerComponent mediaPlayer, ITagDao tagDao, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.view = view;
            this.tagDao = tagDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;
            this.mediaPlayerComponent = mediaPlayer;
            this.currentFeatureType = FeatureType.Chroma;
            
            this.InitializeTagsAndTagValues();
            this.InitializeTrack();
            this.InitializeTracklistStructure();
            this.InitializeTracklistContent();
            this.InitializeTrackList();
            this.InitializeColors();
            this.InitializeSoundWaves();

            this.messenger = new ObservableConcurrentBag<Messenger>();
            this.messenger.ItemAdded += Messenger_ItemAdded;
            this.view.AnalyseTrackEvent += AnalyseTrackEvent;
            this.view.CancelAnalysationEvent += CancelAnalysationEvent;
            this.view.SetCurrentFeatureTypeEvent += SetCurrentFeatureTypeEvent;
        }

      

        private void InitializeTagsAndTagValues()
        {
            this.tagList = this.tagDao.GetAllTag().Where(x => !x.HasMultipleValues).ToList();
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
        private void InitializeColors()
        {
            this.BackgroundColor = OxyColor.Parse("#363639");
            this.FontColor = OxyColor.Parse("#c6c6c6");
            this.ButtonColor = OxyColor.Parse("#292a2d");
            this.ButtonBorderColor = OxyColor.Parse("#1b1b1b");
            this.ActiveColor = OxyColor.Parse("#FFBF80");
        }


        private double CalculateBPM(float[] samples, int sampleRate)
        {
            int frameSize = 2048; // Növelt frame méret
            List<int> peakIndices = new List<int>();

            for (int i = 0; i < samples.Length - frameSize; i += frameSize)
            {
                float sum = 0;
                for (int j = i; j < i + frameSize && j < samples.Length; j++)
                {
                    sum += samples[j] * samples[j];
                }

                float rms = (float)Math.Sqrt(sum / frameSize);
                if (rms > 0.01) // RMS küszöb további csökkentése
                {
                    peakIndices.Add(i);
                }
            }

            if (peakIndices.Count < 2)
            {
                return 0; // Nem található elegendő csúcs a BPM számításhoz
            }

            List<double> intervals = new List<double>();
            for (int i = 1; i < peakIndices.Count; i++)
            {
                double interval = (peakIndices[i] - peakIndices[i - 1]) / (double)sampleRate;
                if (interval > 0.3 && interval < 1.0) // Szűrjük az irreális időközöket (60 BPM - 200 BPM)
                {
                    intervals.Add(interval);
                }
            }

            if (intervals.Count == 0)
            {
                return 0; // Nincs elegendő érvényes időköz
            }

            double averageInterval = intervals.Average();
            double bpm = 60.0 / averageInterval;

            return bpm;
        }
        private void InitializeSoundWaves()
        {
            if(this.currentTrack != null && !String.IsNullOrEmpty(this.currentTrack.Path))
            {
                using (var reader = new AudioFileReader(this.currentTrack.Path))
                {
                   /* float[] buffer = new float[reader.Length / 4];
                    reader.Read(buffer, 0, buffer.Length);
                    int originalSampleRate = reader.WaveFormat.SampleRate;
                    int frameSize = 1024; // Csökkentett frame méret
                    */
                    List<float> samples = new List<float>();
                    float[] buffer = new float[reader.WaveFormat.SampleRate];
                    int read;
                    while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        for (int i = 0; i < read; i++)
                        {
                            samples.Add(buffer[i]);
                        }
                    }

                    double bpm = CalculateBPM(samples.ToArray(), reader.WaveFormat.SampleRate);

                    /* double[] energy = CalculateEnergy(buffer, frameSize);
                     double[] spectralFlux = CalculateSpectralFlux(buffer, frameSize);
                     double[] zcr = CalculateZCR(buffer, frameSize);
                     double[] frequencySpectrum = CalculateFrequencySpectrum(buffer, frameSize);
                     double[] tempoChanges = CalculateTempoChanges(buffer, frameSize, originalSampleRate);

                     int smoothingWindowSize = 11;
                     energy = GaussianSmooth(energy, smoothingWindowSize);
                     spectralFlux = GaussianSmooth(spectralFlux, smoothingWindowSize);
                     zcr = GaussianSmooth(zcr, smoothingWindowSize);
                     frequencySpectrum = GaussianSmooth(frequencySpectrum, smoothingWindowSize);
                     tempoChanges = GaussianSmooth(tempoChanges, smoothingWindowSize);

                     var (energyMean, energyStdDev) = CalculateMeanAndStdDev(energy);
                     var (spectralFluxMean, spectralFluxStdDev) = CalculateMeanAndStdDev(spectralFlux);
                     var (zcrMean, zcrStdDev) = CalculateMeanAndStdDev(zcr);
                     var (freqMean, freqStdDev) = CalculateMeanAndStdDev(frequencySpectrum);
                     var (tempoMean, tempoStdDev) = CalculateMeanAndStdDev(tempoChanges);

                     Console.WriteLine($"Energy Mean: {energyMean}, Energy StdDev: {energyStdDev}");
                     Console.WriteLine($"Spectral Flux Mean: {spectralFluxMean}, Spectral Flux StdDev: {spectralFluxStdDev}");
                     Console.WriteLine($"ZCR Mean: {zcrMean}, ZCR StdDev: {zcrStdDev}");
                     Console.WriteLine($"Frequency Spectrum Mean: {freqMean}, Frequency Spectrum StdDev: {freqStdDev}");
                     Console.WriteLine($"Tempo Changes Mean: {tempoMean}, Tempo Changes StdDev: {tempoStdDev}");

                     int dropIndex = DetectDropIndex(spectralFlux, energy, zcr, frequencySpectrum, tempoChanges, spectralFluxMean, spectralFluxStdDev, energyMean, energyStdDev, zcrMean, zcrStdDev, freqMean, freqStdDev, tempoMean, tempoStdDev);

                     double frameDuration = (double)frameSize / originalSampleRate;
                     double dropTime = dropIndex * frameDuration;

                     Console.WriteLine($"Drop detected at: {dropTime} seconds");*/
                }
            }
        }
        private int DetectDropIndex(double[] spectralFlux, double[] energy, double[] zcr, double[] frequencySpectrum, double[] tempoChanges, double fluxMean, double fluxStdDev, double energyMean, double energyStdDev, double zcrMean, double zcrStdDev, double freqMean, double freqStdDev, double tempoMean, double tempoStdDev)
        {
            double fluxThreshold = fluxMean + 0.5 * fluxStdDev;
            double energyThreshold = energyMean + 0.5 * energyStdDev;
            double zcrThreshold = zcrMean + 0.5 * zcrStdDev;
            double freqThreshold = freqMean + 0.5 * freqStdDev;
            double tempoThreshold = tempoMean + 0.5 * tempoStdDev;

            for (int i = 0; i < spectralFlux.Length; i++)
            {
                double combinedScore = (spectralFlux[i] / fluxThreshold) + (energy[i] / energyThreshold) + (zcr[i] / zcrThreshold) + (frequencySpectrum[i] / freqThreshold) + (tempoChanges[i] / tempoThreshold);
                if (combinedScore > 4.0) // Súlyozott küszöbérték
                {
                    return i;
                }
            }
            return -1; // Ha nem talál drop-ot, adjon vissza -1-et
        }
        private double[] CalculateFrequencySpectrum(float[] buffer, int frameSize)
        {
            int numFrames = buffer.Length / frameSize;
            double[] frequencySpectrum = new double[numFrames];

            for (int i = 0; i < numFrames; i++)
            {
                float[] frame = new float[frameSize];
                Array.Copy(buffer, i * frameSize, frame, 0, frameSize);

                // Apply a window function to the frame (e.g., Hamming window)
                for (int j = 0; j < frameSize; j++)
                {
                    frame[j] *= (float)(0.54 - 0.46 * Math.Cos(2 * Math.PI * j / (frameSize - 1)));
                }

                // Perform FFT on the frame
                Complex[] fftResult = FFT(frame);

                // Calculate the magnitude of the FFT result
                double magnitudeSum = 0;
                for (int j = 0; j < fftResult.Length / 2; j++)
                {
                    magnitudeSum += Math.Sqrt(fftResult[j].X * fftResult[j].X + fftResult[j].Y * fftResult[j].Y);
                }

                // Store the average magnitude in the frequency spectrum array
                frequencySpectrum[i] = magnitudeSum / (fftResult.Length / 2);
            }

            return frequencySpectrum;
        }
        private NAudio.Dsp.Complex[] FFT(float[] frame)
        {
            int n = frame.Length;
            NAudio.Dsp.Complex[] fftResult = new NAudio.Dsp.Complex[n];

            for (int i = 0; i < n; i++)
            {
                fftResult[i].X = frame[i];
                fftResult[i].Y = 0;
            }

            NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(n, 2.0), fftResult);

            return fftResult;
        }
        private double[] CalculateTempoChanges(float[] buffer, int frameSize, int sampleRate)
        {
            int numFrames = buffer.Length / frameSize;
            double[] tempoChanges = new double[numFrames];

            for (int i = 0; i < numFrames; i++)
            {
                float[] frame = new float[frameSize];
                Array.Copy(buffer, i * frameSize, frame, 0, frameSize);

                // Calculate the tempo of the frame
                double tempo = CalculateTempo(frame, sampleRate);

                // Avoid NaN values
                if (double.IsNaN(tempo))
                {
                    tempo = 0;
                }

                // Store the tempo in the tempo changes array
                tempoChanges[i] = tempo;
            }

            return tempoChanges;
        }

        

        private double CalculateTempo(float[] frame, int sampleRate)
        {
            // Autocorrelation
            int maxLag = sampleRate / 2; // Maximum lag to consider (half a second)
            double[] autocorrelation = new double[maxLag];

            for (int lag = 0; lag < maxLag; lag++)
            {
                double sum = 0;
                for (int i = 0; i < frame.Length - lag; i++)
                {
                    sum += frame[i] * frame[i + lag];
                }
                autocorrelation[lag] = sum;
                Console.WriteLine($"Lag {lag}: {autocorrelation[lag]}");
            }

            // Normalize the autocorrelation values
            double maxAutocorrelation = autocorrelation.Max();
            if (maxAutocorrelation > 0)
            {
                for (int i = 0; i < maxLag; i++)
                {
                    autocorrelation[i] /= maxAutocorrelation;
                }
            }

            // Find the peak in the autocorrelation
            int peakIndex = 0;
            double peakValue = 0;
            for (int i = 1; i < maxLag; i++)
            {
                if (autocorrelation[i] > peakValue)
                {
                    peakValue = autocorrelation[i];
                    peakIndex = i;
                }
                Console.WriteLine($"Index {i}: {autocorrelation[i]}, PeakValue: {peakValue}, PeakIndex: {peakIndex}");
            }

            // Avoid division by zero
            if (peakIndex == 0)
            {
                return 0;
            }

            // Calculate the tempo in beats per minute (BPM)
            double tempo = 60.0 * sampleRate / peakIndex;

            // Ensure the tempo is within a reasonable range
            if (tempo < 40 || tempo > 240)
            {
                return 0;
            }

            return tempo;
        }
        private float[] Downsample(float[] data, int factor)
        {
            int newSize = data.Length / factor;
            float[] result = new float[newSize];
            for (int i = 0; i < newSize; i++)
            {
                result[i] = data[i * factor];
            }
            return result;
        }
        private void PlotSoundWaves(float[] soundwave, double totalDurationInMinutes, int originalSampleRate)
        {
            var plotView = new PlotView
            {
                Dock = DockStyle.Fill
            };

            var plotModel = new PlotModel { Title = "Soundwave" };
            plotModel.TitleColor = this.FontColor;

            // Calculate the total duration in seconds using the downsampled sample rate
           // double totalDurationInSeconds = soundwave.Length / (double)downsampledSampleRate;

            // Add X axis (Time)
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Time (minutes)",
                Minimum = 0,
                Maximum = totalDurationInMinutes, // Set the maximum to the total duration in minutes
                TitleColor = this.FontColor,
                TextColor = this.FontColor,
                TicklineColor = this.FontColor,
                LabelFormatter = value =>
                {
                    TimeSpan time = TimeSpan.FromMinutes(value);
                    return time.ToString(@"m\:ss");
                }
            };
            plotModel.Axes.Add(xAxis);

            // Add Y axis (Amplitude)
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Amplitude",
                TitleColor = this.FontColor,
                TextColor = this.FontColor,
                TicklineColor = this.FontColor
            };
            plotModel.Axes.Add(yAxis);

            var lineSeries = new LineSeries
            {
                Title = "Soundwave",
                MarkerType = MarkerType.None,
                Color = this.ActiveColor
            };

            // Ensure all points are added
            for (int i = 0; i < soundwave.Length; i++)
            {
                double timeInMinutes = (i / (double)soundwave.Length) * totalDurationInMinutes; // Scale to total duration
                lineSeries.Points.Add(new DataPoint(timeInMinutes, soundwave[i]));
            }

            plotModel.Series.Add(lineSeries);

            // Loop to experiment with different thresholds
           /* for (float e = 0.05f; e <= 0.2f; e += 0.05f)
            {
                for (float f = 0.01f; f <= 0.05f; f += 0.01f)
                {
                    for (float z = 0.02f; z <= 0.1f; z += 0.02f)
                    {
                        var sections = DetectSections(soundwave, sampleRate, e, f, z);
                        Console.WriteLine($"Energy: {e}, Flux: {f}, Zero-Crossing: {z}, Sections: {sections.Count}");
                    }
                }
            }*/

            // Detect sections
            var sections = DetectSections(soundwave, originalSampleRate);

            // Add vertical line annotations for sections
            foreach (var section in sections)
            {
                double startTimeInMinutes = (section.start / (double)originalSampleRate) / 60; // Convert to minutes
                var lineAnnotation = new LineAnnotation
                {
                    Type = LineAnnotationType.Vertical,
                    X = startTimeInMinutes,
                    Color = OxyColors.Black,
                    LineStyle = LineStyle.Solid,
                    StrokeThickness = 2,
                    Text = section.section,
                    TextColor = OxyColors.Black,
                    TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Left,
                    TextVerticalAlignment = VerticalAlignment.Top,
                    TextMargin = 5
                };
                plotModel.Annotations.Add(lineAnnotation);
            }

            // Add the vertical line annotation
            this.verticalLine = new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                X = 0, // Initial position
                Color = this.ButtonColor,
                LineStyle = LineStyle.Solid,
                StrokeThickness = 2,
            };
            plotModel.Annotations.Add(verticalLine);

            plotView.Model = plotModel;

            this.view.UpdateSoundWavePlot(plotView);
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
        float[][] chromaFeatureArray;
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
                if (this.currentFeatureType == FeatureType.Chroma) {
                    this.PlotChromaFeatures(this.chromaFeatureArray, track.Length);
                }
            }
        }
        private async Task AnalyseTrackAsync(Track track, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (this.currentFeatureType == FeatureType.Chroma)
                    {
                        using (var reader = new AudioFileReader(track.Path))
                        {
                            this.chromaFeatureArray = this.ExtractChromaFeatures(track.Path);
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
        public float[][] ExtractChromaFeatures(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                int sampleRate = reader.WaveFormat.SampleRate;
                int channels = reader.WaveFormat.Channels;
                var buffer = new float[sampleRate * channels];
                int samplesRead = reader.Read(buffer, 0, buffer.Length);

                // Convert to mono
                float[] monoBuffer = new float[samplesRead / channels];
                for (int i = 0; i < samplesRead; i += channels)
                {
                    monoBuffer[i / channels] = buffer[i];
                }

                // Frame the signal
                int frameSize = 2048;
                int hopSize = frameSize / 2;
                int numFrames = (monoBuffer.Length - frameSize) / hopSize + 1;
                float[][] chromaFeatures = new float[numFrames][];

                for (int frame = 0; frame < numFrames; frame++)
                {
                    float[] windowedFrame = new float[frameSize];
                    Array.Copy(monoBuffer, frame * hopSize, windowedFrame, 0, frameSize);

                    // Apply Hamming window
                    for (int i = 0; i < frameSize; i++)
                    {
                        windowedFrame[i] *= (float)(0.54 - 0.46 * Math.Cos(2 * Math.PI * i / (frameSize - 1)));
                    }

                    // Compute STFT
                    Complex[] complexSpectrum = windowedFrame.Select(v => new Complex { X = v, Y = 0 }).ToArray();
                    FastFourierTransform.FFT(true, (int)Math.Log(frameSize, 2.0), complexSpectrum);

                    // Calculate chroma vector
                    float[] chromaVector = new float[12];
                    for (int i = 0; i < complexSpectrum.Length / 2; i++)
                    {
                        double frequency = i * sampleRate / frameSize;
                        if (frequency > 0)
                        {
                            int pitchClass = (int)Math.Round(12 * Math.Log(frequency / 440.0, 2)) % 12;
                            if (pitchClass < 0) pitchClass += 12; // Ensure pitch class is between 0 and 11
                            chromaVector[pitchClass] += (float)Math.Sqrt(complexSpectrum[i].X * complexSpectrum[i].X + complexSpectrum[i].Y * complexSpectrum[i].Y);
                        }
                    }

                    // Normalize chroma vector
                    float max = chromaVector.Max();
                    for (int i = 0; i < chromaVector.Length; i++)
                    {
                        chromaVector[i] /= max;
                    }

                    chromaFeatures[frame] = chromaVector;
                }

                return chromaFeatures;
            }
        }
        private LineAnnotation verticalLine { get; set; }
        private void PlotChromaFeatures(float[][] chromaFeatures, double songLengthInSeconds)
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

            // Add X axis (Frames)
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Frames",
                Minimum = 0,
                Maximum = chromaFeatures.Length, // Set the maximum to the number of frames
                MajorStep = chromaFeatures.Length / (songLengthInSeconds / 15), // Major ticks every 15 seconds worth of frames
                MinorStep = chromaFeatures.Length / (songLengthInSeconds / 5), // Minor ticks every 5 seconds worth of frames
                LabelFormatter = value => value.ToString("F0"), // Format as integer frames
                TitleColor = this.FontColor, // Change the axis title color to blue
                TextColor = this.FontColor, // Change the axis text color to green
                TicklineColor = this.FontColor
            };
            plotModel.Axes.Add(xAxis);

            // Add Y axis (Chroma Features with Keycode)
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
                X1 = chromaFeatures.Length, // Set X1 to the number of chroma feature frames
                Y0 = 0,
                Y1 = 12,
                Interpolate = false,
                RenderMethod = HeatMapRenderMethod.Rectangles
            };

            double[,] data = new double[chromaFeatures.Length, 12];
            for (int x = 0; x < chromaFeatures.Length; x++)
            {
                for (int y = 0; y < 12; y++)
                {
                    data[x, y] = chromaFeatures[x][y];
                }
            }
            heatMapSeries.Data = data;
            plotModel.Series.Add(heatMapSeries);

            // Add the vertical line annotation
           /* var verticalLine = new LineAnnotation
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
            plotModel.Annotations.Add(verticalLine);*/

            plotView.Model = plotModel;

            this.view.UpdatePlot(plotView);
        }
        private double FindReasonableMaxValue(float[][] chromaFeatures, double percentile = 0.99)
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
        public void UpdateLinePosition(double currentTimeInSeconds)
        {
            if (this.currentTrack == null || this.currentTrack.TrackIdInPlaylist != this.mediaPlayerComponent.CurrentTrackIdInPlaylist)
            {
                currentTimeInSeconds = 0;
            }

            if (this.verticalLine != null)
            {
                this.verticalLine.X = currentTimeInSeconds /60;
                this.verticalLine.PlotModel.InvalidatePlot(false); // Redraw the plot without updating the data
            }
        }


        double[] GaussianSmooth(double[] data, int windowSize)
        {
            double[] smoothedData = new double[data.Length];
            double[] kernel = new double[windowSize];
            double sigma = windowSize / 2.0;
            double sum = 0;

            for (int i = 0; i < windowSize; i++)
            {
                kernel[i] = Math.Exp(-0.5 * Math.Pow((i - windowSize / 2) / sigma, 2));
                sum += kernel[i];
            }

            for (int i = 0; i < windowSize; i++)
            {
                kernel[i] /= sum;
            }

            for (int i = 0; i < data.Length; i++)
            {
                double value = 0;
                for (int j = 0; j < windowSize; j++)
                {
                    int index = i + j - windowSize / 2;
                    if (index >= 0 && index < data.Length)
                    {
                        value += data[index] * kernel[j];
                    }
                }
                smoothedData[i] = value;
            }

            return smoothedData;
        }
        (double mean, double stdDev) CalculateMeanAndStdDev(double[] data)
        {
            double mean = data.Average();
            double sumOfSquares = data.Select(val => (val - mean) * (val - mean)).Sum();
            double stdDev = Math.Sqrt(sumOfSquares / data.Length);
            return (mean, stdDev);
        }
        double[] CalculateEnergy(float[] samples, int frameSize)
        {
            int numFrames = samples.Length / frameSize;
            double[] energy = new double[numFrames];
            for (int i = 0; i < numFrames; i++)
            {
                double frameEnergy = 0;
                for (int j = 0; j < frameSize; j++)
                {
                    int index = i * frameSize + j;
                    if (index < samples.Length)
                    {
                        frameEnergy += samples[index] * samples[index];
                    }
                }
                energy[i] = frameEnergy;
            }
            return energy;
        }
        double[] CalculateSpectralFlux(float[] samples, int frameSize)
        {
            int numFrames = samples.Length / frameSize;
            double[] spectralFlux = new double[numFrames - 1];
            for (int i = 1; i < numFrames; i++)
            {
                Complex32[] currentFrame = new Complex32[frameSize];
                Complex32[] previousFrame = new Complex32[frameSize];
                for (int j = 0; j < frameSize; j++)
                {
                    int currentIndex = i * frameSize + j;
                    int previousIndex = (i - 1) * frameSize + j;
                    if (currentIndex < samples.Length)
                    {
                        currentFrame[j] = new Complex32(samples[currentIndex], 0);
                    }
                    if (previousIndex < samples.Length)
                    {
                        previousFrame[j] = new Complex32(samples[previousIndex], 0);
                    }
                }

                Fourier.Forward(currentFrame);
                Fourier.Forward(previousFrame);

                double flux = 0;
                for (int j = 0; j < frameSize; j++)
                {
                    double diff = currentFrame[j].Magnitude - previousFrame[j].Magnitude;
                    flux += diff * diff;
                }
                spectralFlux[i - 1] = flux;
            }
            return spectralFlux;
        }
        double[] CalculateZCR(float[] samples, int frameSize)
        {
            int numFrames = samples.Length / frameSize;
            double[] zcr = new double[numFrames];
            for (int i = 0; i < numFrames; i++)
            {
                int zeroCrossings = 0;
                for (int j = 1; j < frameSize; j++)
                {
                    int index = i * frameSize + j;
                    if (index < samples.Length && samples[index] * samples[index - 1] < 0)
                    {
                        zeroCrossings++;
                    }
                }
                zcr[i] = (double)zeroCrossings / frameSize;
            }
            return zcr;
        }
        public double VerseSpectralFluxStdDevMultiplier { get; set; } = 0.5;
        public double VerseZCRStdDevMultiplier { get; set; } = 0.5;
        public double VerseEnergyStdDevMultiplier { get; set; } = 0.5; // New property for energy
        public double BreakdownZCRStdDevMultiplier { get; set; } = 0.5;
        public double BreakdownEnergyStdDevMultiplier { get; set; } = 0.5;
        public double BreakdownSpectralFluxStdDevMultiplier { get; set; } = 0.5; // New property for spectral flux
        public double BreakdownEndEnergyStdDevMultiplier { get; set; } = 0.5;
        public double BreakdownEndSpectralFluxStdDevMultiplier { get; set; } = 0.5;
        public double BreakdownEndZCRStdDevMultiplier { get; set; } = 0.5; // New property for ZCR

        int DetectVerseStart(double[] energy, double[] spectralFlux, double[] zcr, double spectralFluxMean, double spectralFluxStdDev, double zcrMean, double zcrStdDev, double energyMean, double energyStdDev)
        {
            int startFrame = 847+25; // Start logging 100 frames before the expected verse start (947 - 100)
            int endFrame = 1047-25; // Log 200 frames around the expected verse start

            for (int i = startFrame; i < endFrame; i++)
            {
                double dynamicSpectralFluxThreshold = spectralFluxMean - VerseSpectralFluxStdDevMultiplier * spectralFluxStdDev;
                double dynamicZCRThreshold = zcrMean + VerseZCRStdDevMultiplier * zcrStdDev;
                double dynamicEnergyThreshold = energyMean + VerseEnergyStdDevMultiplier * energyStdDev; // Adjusted threshold for energy

                // Apply smoothing or averaging if needed
                double smoothedSpectralFlux = (spectralFlux[i] + spectralFlux[Math.Max(0, i - 1)] + spectralFlux[Math.Min(spectralFlux.Length - 1, i + 1)]) / 3;
                double smoothedZCR = (zcr[i] + zcr[Math.Max(0, i - 1)] + zcr[Math.Min(zcr.Length - 1, i + 1)]) / 3;
                double smoothedEnergy = (energy[i] + energy[Math.Max(0, i - 1)] + energy[Math.Min(energy.Length - 1, i + 1)]) / 3;

                Console.WriteLine($"Frame {i}: SpectralFlux={smoothedSpectralFlux}, ZCR={smoothedZCR}, Energy={smoothedEnergy}");
                Console.WriteLine($"Thresholds: SpectralFlux<{dynamicSpectralFluxThreshold}, ZCR>{dynamicZCRThreshold}, Energy>{dynamicEnergyThreshold}");

                if (smoothedSpectralFlux < dynamicSpectralFluxThreshold && smoothedZCR > dynamicZCRThreshold && smoothedEnergy > dynamicEnergyThreshold)
                {
                    Console.WriteLine($"Verse Start Detected at Frame {i}: SpectralFlux={smoothedSpectralFlux}, ZCR={smoothedZCR}, Energy={smoothedEnergy}");
                    return i;
                }
            }
            Console.WriteLine("No Verse Start Detected");
            return 0;
        }

        int DetectBreakdownStart(double[] energy, double[] spectralFlux, double[] zcr, double zcrMean, double zcrStdDev, double energyMean, double energyStdDev, double spectralFluxMean, double spectralFluxStdDev)
        {
            int startFrame = 2792; // Start logging 50 frames before the expected breakdown start (2842 - 50)
            int endFrame = 2942; // Log 100 frames around the expected breakdown start

            for (int i = startFrame; i < endFrame; i++)
            {
                double dynamicZCRThreshold = zcrMean + BreakdownZCRStdDevMultiplier * zcrStdDev;
                double dynamicEnergyThreshold = energyMean - BreakdownEnergyStdDevMultiplier * energyStdDev;
                double dynamicSpectralFluxThreshold = spectralFluxMean - BreakdownSpectralFluxStdDevMultiplier * spectralFluxStdDev; // Adjusted threshold for spectral flux

                if (zcr[i] > dynamicZCRThreshold && energy[i] < dynamicEnergyThreshold && spectralFlux[i] < dynamicSpectralFluxThreshold)
                {
                    Console.WriteLine($"Breakdown Start Detected at Frame {i}: ZCR={zcr[i]}, Energy={energy[i]}, SpectralFlux={spectralFlux[i]}");
                    return i;
                }
            }
            Console.WriteLine("No Breakdown Start Detected");
            return 0;
        }

        int DetectBreakdownEnd(double[] energy, double[] spectralFlux, double[] zcr, double energyMean, double energyStdDev, double spectralFluxMean, double spectralFluxStdDev, double zcrMean, double zcrStdDev)
        {
            int startFrame = 3288; // Start logging 50 frames before the expected breakdown end (3338 - 50)
            int endFrame = 3438; // Log 100 frames around the expected breakdown end

            for (int i = startFrame; i < endFrame; i++)
            {
                double dynamicEnergyThreshold = energyMean + BreakdownEndEnergyStdDevMultiplier * energyStdDev;
                double dynamicSpectralFluxThreshold = spectralFluxMean - BreakdownEndSpectralFluxStdDevMultiplier * spectralFluxStdDev;
                double dynamicZCRThreshold = zcrMean - BreakdownEndZCRStdDevMultiplier * zcrStdDev; // Adjusted threshold for ZCR

                if (energy[i] > dynamicEnergyThreshold && spectralFlux[i] < dynamicSpectralFluxThreshold && zcr[i] < dynamicZCRThreshold)
                {
                    Console.WriteLine($"Breakdown End Detected at Frame {i}: Energy={energy[i]}, SpectralFlux={spectralFlux[i]}, ZCR={zcr[i]}");
                    return i;
                }
            }
            Console.WriteLine("No Breakdown End Detected");
            return 0;
        }



        private List<(string section, int start, int end)> DetectSections(float[] soundwave, int sampleRate)
        {
            int windowSize = sampleRate / 10; // 0.1 second window
            List<(string section, int start, int end)> sections = new List<(string section, int start, int end)>();

            float energyThreshold = 0.1f; // Adjust this threshold based on your needs
            float fluxThreshold = 0.02f; // Adjust this threshold based on your needs
            float zeroCrossingThreshold = 0.05f; // Adjust this threshold based on your needs

            bool inSection = false;
            int sectionStart = 0;
            string currentSection = "Intro";

            // Open a log file to write the values
            using (StreamWriter logFile = new StreamWriter("section_detection_log.txt"))
            {
                logFile.WriteLine("WindowStart,Energy,SpectralFlux,ZeroCrossingRate");

                for (int i = 0; i < soundwave.Length - windowSize; i += windowSize)
                {
                    float[] window = soundwave.Skip(i).Take(windowSize).ToArray();

                    // Calculate energy
                    float energy = window.Select(x => x * x).Sum();

                    // Calculate spectral flux
                    float flux = 0;
                    for (int j = 1; j < window.Length; j++)
                    {
                        flux += Math.Abs(window[j] - window[j - 1]);
                    }

                    // Calculate zero-crossing rate
                    float zeroCrossingRate = 0;
                    for (int j = 1; j < window.Length; j++)
                    {
                        if (window[j] * window[j - 1] < 0)
                        {
                            zeroCrossingRate++;
                        }
                    }
                    zeroCrossingRate /= window.Length;

                    // Log the values
                    logFile.WriteLine($"{i},{energy},{flux},{zeroCrossingRate}");

                    if ((energy > energyThreshold || flux > fluxThreshold || zeroCrossingRate > zeroCrossingThreshold) && !inSection)
                    {
                        inSection = true;
                        sectionStart = i;
                    }
                    else if ((energy < energyThreshold && flux < fluxThreshold && zeroCrossingRate < zeroCrossingThreshold) && inSection)
                    {
                        inSection = false;
                        sections.Add((currentSection, sectionStart, i));

                        // Transition to the next section
                        if (currentSection == "Intro")
                        {
                            currentSection = "Main Verse";
                        }
                        else if (currentSection == "Main Verse")
                        {
                            currentSection = "Breakdown";
                        }
                        else if (currentSection == "Breakdown")
                        {
                            currentSection = "Second Verse";
                        }
                    }
                }

                if (inSection)
                {
                    sections.Add((currentSection, sectionStart, soundwave.Length));
                }
            }

            return sections;
        }




        public List<float[]> ExtractChromaFeaturesOld(string filePath)
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
        private void PlotChromaFeaturesOld(float[][] chromaFeatures, double songLengthInSeconds)
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
               // X1 = chromaFeatures.Count, // Set X1 to the number of chroma feature frames
                X1 = chromaFeatures.Length, // Set X1 to the number of chroma feature frames
                Y0 = 0,
                Y1 = 12,
                Interpolate = false,
                RenderMethod = HeatMapRenderMethod.Rectangles
            };

            /* double[,] data = new double[chromaFeatures.Count, 12];
             for (int x = 0; x < chromaFeatures.Count; x++)
             {
                 for (int y = 0; y < 12; y++)
                 {
                     data[x, y] = chromaFeatures[x][y];
                 }
             }*/
            double[,] data = new double[chromaFeatures.Length, 12];
            for (int x = 0; x < chromaFeatures.Length; x++)
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

    }
}
