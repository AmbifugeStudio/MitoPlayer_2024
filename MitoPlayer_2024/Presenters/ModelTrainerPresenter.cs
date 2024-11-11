using Accord.Math;
using HDF.PInvoke;
using MathNet.Numerics.IntegralTransforms;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class ModelTrainerPresenter
    {
        private IModelTrainerView view;
        private ITagDao tagDao { get; set; }
        private ITrackDao trackDao { get; set; }
        private ISettingDao settingDao { get; set; }
        private List<Model.Track> trackList { get; set; }
        private DataTable inputTrackTable { get; set; }
        private BindingSource inputTrackListBindingSource { get; set; }
        private DataTable resultTrackTable { get; set; }
        private BindingSource resultTrackListBindingSource { get; set; }
        private DataTable trainingDataTable { get; set; }
        private BindingSource traningDataListBindingSource { get; set; }

        private List<Tag> TagList { get; set; }
        private List<Playlist> PlaylistList { get; set; }
        private List<TrainingData> TemplateList { get; set; }
        private Tag CurrentTag { get; set; }
        private TrackProperty CurrentTrackProperty { get; set; }
        private Playlist CurrentPlaylist { get; set; }
        private TrainingData CurrentTemplate { get; set; }
        private TrainingData CurrentTrainingData { get; set; }
        private bool IsChromaFeaturesEnabled { get; set; }
        private bool IsMFCCsEnabled { get; set; }
        private bool IsSpectralContrastEnabled { get; set; }
        private bool IsHPCPEnabled { get; set; }
        private bool IsHPSEnabled { get; set; }
        private bool IsSpectralCentroidEnabled { get; set; }
        private bool IsTonnetzFeaturesEnabled { get; set; }
        private bool IsSpectralBandwidthEnabled { get; set; }
        private bool IsZeroCrossingRateEnabled { get; set; }
        private bool IsRmsEnergyEnabled { get; set; }
        private bool IsPitchEnabled { get; set; }
        private bool IsTracklistDetailsDisplayed { get; set; }
        private int BatchSize { get; set; }

       
        public ModelTrainerPresenter(IModelTrainerView modelTrainerView, ITagDao tagDao,ITrackDao trackDao, ISettingDao settingDao)
        {
            this.view = modelTrainerView;
            this.tagDao = tagDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;

            this.view.SelectTag += SelectTag;
            this.view.SelectPlaylist += SelectPlaylist;
            this.view.SelectTemplate += SelectTemplate;

            this.view.CloseViewWithOk += CloseViewWithOk;

            this.view.IsChromaFeaturesEnabled += IsChromaFeaturesEnabledEvent;
            this.view.IsMFCCsEnabled += IsMFCCsEnabledEvent;
            this.view.IsSpectralContrastEnabled += IsSpectralContrastEnabledEvent;
            this.view.IsHPCPEnabled += IsHPCPEnabledEvent;
            this.view.IsHPSEnabled += IsHPSEnabledEvent;
            this.view.IsSpectralCentroidEnabled += IsSpectralCentroidEnabledEvent;
            this.view.IsTonnetzFeaturesEnabled += IsTonnetzFeaturesEnabledEvent;
            this.view.IsSpectralBandwidthEnabled += IsSpectralBandwidthEnabledEvent;
            this.view.IsZCREnabled += IsZCREnabledEvent;
            this.view.IsRMSEnabled += IsRMSEnabledEvent;
            this.view.IsPitchEnabled += IsPitchEnabledEvent;
            this.view.BatchProcessChanged += BatchProcessChangedEvent;
            this.view.CancelGenerationEvent += CancelGenerationEvent;
            this.view.LoadTrainingDataEvent += LoadTrainingDataEvent;
            this.view.TrainModelEvent += TrainModelEvent;
            this.view.DeleteTrainingDataEvent += DeleteTrainingDataEvent;
            this.view.SetIsTracklistDetailsDisplayedEvent += SetIsTracklistDetailsDisplayedEvent;
            this.view.CalculateDataSetQualityEvent += CalculateDataSetQualityEvent;


            this.view.GenerateTrainingData += GenerateTrainingDataEvent;

            this.TagList = this.tagDao.GetAllTag().Where(x => !x.HasMultipleValues).ToList();
            this.PlaylistList = this.trackDao.GetAllPlaylist().FindAll(x=>x.IsModelTrainer);
            this.TemplateList = this.trackDao.GetAllTrainingData().FindAll(x => x.IsTemplate);
            this.BatchSize = this.settingDao.GetIntegerSetting(Settings.TrainingModelBatchCount.ToString());
            this.IsTracklistDetailsDisplayed = this.settingDao.GetBooleanSetting(Settings.IsTracklistDetailsDisplayed.ToString()).Value;

            this.InitializeTagsAndTagValues();

            this.view.InitializeView(this.TagList,this.PlaylistList, this.TemplateList, this.BatchSize, this.IsTracklistDetailsDisplayed);

            this.InitializeTrainingDataListStructure();
            this.InitializeTrainingDataListContent();

            this.ConcBag = new ObservableConcurrentBag<MessageTest>();
            this.ConcBag.ItemAdded += ConcBag_ItemAdded;

        }

        

        private Dictionary<String, Dictionary<String, Color>> tagValueDictionary { get; set; }
        private List<Tag> tagList { get;set; }
        private void InitializeTagsAndTagValues()
        {
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
        private void SetIsTracklistDetailsDisplayedEvent(object sender, ListEventArgs e)
        {
            this.IsTracklistDetailsDisplayed = e.BooleanField1;
            this.settingDao.SetBooleanSetting(Settings.IsTracklistDetailsDisplayed.ToString(), this.IsTracklistDetailsDisplayed);

            if(this.IsTracklistDetailsDisplayed && this.CurrentPlaylist != null && this.CurrentTag != null)
            {
                this.CalculateDataSetQuality();
            }
        }
        private void LoadTrainingDataEvent(object sender, ListEventArgs e)
        {
            this.CurrentTrainingData = this.trackDao.GetTrainingData(e.IntegerField1);
        }
        private void SelectTag(object sender, ListEventArgs e)
        {
            this.CurrentTag = this.tagDao.GetTagByName(e.StringField1);
            this.CurrentTrackProperty = this.settingDao.GetTrackPropertyByNameAndGroup(this.CurrentTag.Name, ColumnGroup.TracklistColumns.ToString());
        
            if(this.CurrentPlaylist != null)
            {
                this.InitializeInputDataTableStructure();
                this.InitializeInputDataTableContent();

                this.InitializeOutputDataTableStructure();

                if(this.IsTracklistDetailsDisplayed)
                    this.CalculateDataSetQuality();
            }
        }
        private void SelectPlaylist(object sender, ListEventArgs e)
        {
            this.CurrentPlaylist =this.trackDao.GetPlaylistByName(e.StringField1);

            if (this.CurrentPlaylist != null)
            {
                this.trackList = this.trackDao.GetTracklistWithTagsByPlaylistId(this.CurrentPlaylist.Id, this.TagList);
            }

            if (this.CurrentTag != null)
            {
                this.InitializeInputDataTableStructure();
                this.InitializeInputDataTableContent();

                this.InitializeOutputDataTableStructure();

                if (this.IsTracklistDetailsDisplayed)
                    this.CalculateDataSetQuality();
            }
        }
        private void SelectTemplate(object sender, ListEventArgs e)
        {
            this.CurrentTemplate =this.trackDao.GetTrainingData(e.IntegerField1);

            if (this.CurrentTemplate != null)
            {
                this.view.InitializeFeatureSettings(
                    this.CurrentTemplate.ExtractChromaFeatures,
                    this.CurrentTemplate.HarmonicPercussiveSeparation,
                    this.CurrentTemplate.ExtractMFCCs,
                    this.CurrentTemplate.ExtractHPCP,
                    this.CurrentTemplate.ExtractSpectralContrast,
                    this.CurrentTemplate.ExtractSpectralCentroid,
                    this.CurrentTemplate.ExtractSpectralBandwidth,
                    this.CurrentTemplate.ExtractTonnetzFeatures,
                    this.CurrentTemplate.ExtractZeroCrossingRate,
                    this.CurrentTemplate.ExtractPitch,
                    this.CurrentTemplate.ExtractRmsEnergy
                );
            }
        }
        private void InitializeTrainingDataListStructure()
        {
            this.traningDataListBindingSource = new BindingSource();
            this.trainingDataTable = new DataTable();
            this.trainingDataTable.Columns.Add("Id", typeof(int));
            this.trainingDataTable.Columns.Add("Date", typeof(string));
            this.trainingDataTable.Columns.Add("Name", typeof(string));
            this.trainingDataTable.Columns.Add("TagName", typeof(string));
            this.trainingDataTable.Columns.Add("SampleCount", typeof(string));
            this.trainingDataTable.Columns.Add("Balance(%)", typeof(string));
            this.trainingDataTable.Columns.Add("Size", typeof(string));
        }
        private void InitializeInputDataTableStructure()
        {
            this.inputTrackListBindingSource = new BindingSource();
            this.inputTrackTable = new DataTable();
            this.inputTrackTable.Columns.Add("Artist", typeof(string));
            this.inputTrackTable.Columns.Add("Title", typeof(string));
            this.inputTrackTable.Columns.Add(this.CurrentTrackProperty.Name, Type.GetType(this.CurrentTrackProperty.Type));
        }
        private void InitializeOutputDataTableStructure()
        {
            this.inputTrackListBindingSource = new BindingSource();
            this.inputTrackTable = new DataTable();
            this.inputTrackTable.Columns.Add("Artist", typeof(string));
            this.inputTrackTable.Columns.Add("Title", typeof(string));
            this.inputTrackTable.Columns.Add(this.CurrentTrackProperty.Name, Type.GetType(this.CurrentTrackProperty.Type));
        }
        private void InitializeInputDataTableContent()
        {
            String artist = String.Empty;
            String title = String.Empty;
            String tag = String.Empty;

            this.inputTrackTable.Clear();

            List<Tag> tagList = this.tagDao.GetAllTag();
            if(tagList != null && tagList.Count > 0)
            {
                this.trackList = this.trackDao.GetTracklistWithTagsByPlaylistId(this.CurrentPlaylist.Id, tagList);

                if (this.trackList != null && this.trackList.Count > 0)
                {
                    for (int i = 0; i <= this.trackList.Count - 1; i++)
                    {
                        artist = this.trackList[i].Artist;
                        title = this.trackList[i].Title;

                        TrackTagValue ttv = this.trackList[i].TrackTagValues.Find(x => x.TagId == this.CurrentTag.Id);
                        if(ttv != null)
                        {
                            if (ttv.HasMultipleValues)
                            {
                                tag = ttv.Value;
                            }
                            else
                            {
                                tag = ttv.TagValueName;
                            }
                        }
                        else
                        {
                            tag = String.Empty;
                        }
                        this.inputTrackTable.Rows.Add(artist, title, tag);
                    }
                }

                this.inputTrackListBindingSource.DataSource = this.inputTrackTable;
                this.view.SetInputTrackListBindingSource(this.inputTrackListBindingSource);
            }
        }
        private Decimal balance { get; set; }
        private const int requiredMinimalSampleCount = 100;
        private void CalculateDataSetQualityEvent(object sender, EventArgs e)
        {
            this.CalculateDataSetQuality();
        }
        private void CalculateDataSetQuality()
        {
            String result = String.Empty;
            Dictionary<String, int> tagValueNumberDic = new Dictionary<String, int>();

            List<TagValue> tvList = this.tagDao.GetTagValuesByTagId(this.CurrentTag.Id);
            if(tvList != null  && tvList.Count > 0)
            {
                for (int i = 0; i <= tvList.Count - 1; i++)
                {
                    if (!tagValueNumberDic.ContainsKey(tvList[i].Name))
                    {
                        tagValueNumberDic.Add(tvList[i].Name, 0);
                    }
                }  
            }

            if (this.trackList != null && this.trackList.Count > 0)
            {
                for (int i = 0; i <= this.trackList.Count - 1; i++)
                {
                    TrackTagValue ttv = this.trackList[i].TrackTagValues.Find(x => x.TagId == this.CurrentTag.Id);
                    if (ttv != null && !ttv.HasMultipleValues && !String.IsNullOrEmpty(ttv.TagValueName))
                    {
                        if (!tagValueNumberDic.ContainsKey(ttv.TagValueName))
                        {
                            tagValueNumberDic.Add(ttv.TagValueName, 1);
                        }
                        else
                        {
                            tagValueNumberDic[ttv.TagValueName]++;
                        }
                    }
                }

                tagValueNumberDic = tagValueNumberDic.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

                // Calculate the total number of keys
                int totalTagValues = tagValueNumberDic.Values.Sum();
                result += $"Total number of {this.CurrentTag.Name}: {totalTagValues}\n\n";
                result += $"Required minimal sample count: {requiredMinimalSampleCount} / {this.CurrentTag.Name} \n\n";

                bool sampleCountIsValid = true;

                // Calculate the proportion of each key
                foreach (var kvp in tagValueNumberDic)
                {
                    double proportion = (double)kvp.Value / totalTagValues * 100;
                    String passOrFail = kvp.Value >= requiredMinimalSampleCount ? "PASS" : "FAIL";

                    if (sampleCountIsValid)
                    {
                        if(kvp.Value < requiredMinimalSampleCount)
                        {
                            sampleCountIsValid = false;
                        }
                    }

                    result += $"{this.CurrentTag.Name}: {kvp.Key} - Proportion: {proportion:F2} % - Count: {kvp.Value}/{requiredMinimalSampleCount} - {passOrFail} \n";
                }

                // Check if the dataset is balanced
                double averageProportion = 100.0 / tagValueNumberDic.Count;
                double threshold = 10.0; // Define a threshold for balance, e.g., 10%

                bool isBalanced = tagValueNumberDic.Values.All(count =>
                Math.Abs((double)count / totalTagValues * 100 - averageProportion) <= threshold);

                result += $"\nThe sample count is enough? - {sampleCountIsValid}\n";

                result += $"\nIs the dataset balanced? - {isBalanced}\n\n";

                // Calculate the percentage value of the balance
                double balancePercentage = tagValueNumberDic.Values.Average(count => Math.Abs((double)count / totalTagValues * 100 - averageProportion));
                double normalizedBalancePercentage = 100 - balancePercentage; // Normalize to 0-100% scale

                this.balance = (Decimal)normalizedBalancePercentage;

                result += $"Balance percentage: {balancePercentage:F2}%\n";
                result += $"Normalized balance percentage: {normalizedBalancePercentage:F2}%\n";

                // Add explanation
                result += "\nA smaller balance percentage indicates that the distribution of keys is closer to the average proportion, suggesting a more balanced dataset.";

                // Display the result in a dialog window
                
                MessageBox.Show(result, this.CurrentTag.Name + " Balance Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void InitializeTrainingDataListContent()
        {
            int id = 0;
            String createDate = String.Empty;
            String name = String.Empty;
            int sampleCount = 0;
            Decimal balance = 0;
            String tagName = String.Empty;
            String size = String.Empty;

            if(this.trainingDataTable != null && this.trainingDataTable.Rows.Count > 0)
                this.trainingDataTable.Clear();

            List<TrainingData> trainingDataList = this.trackDao.GetAllTrainingData().FindAll(x => !x.IsTemplate);

            if (trainingDataList != null && trainingDataList.Count > 0)
            {
                for (int i = 0; i <= trainingDataList.Count - 1; i++)
                {
                    id = trainingDataList[i].Id;
                    createDate = trainingDataList[i].CreateDate.ToShortDateString() + " " + trainingDataList[i].CreateDate.ToShortTimeString();
                    name = trainingDataList[i].Name;
                    sampleCount = trainingDataList[i].SampleCount;
                    balance = trainingDataList[i].Balance;

                    Tag tag = this.tagDao.GetTag(trainingDataList[i].TagId);
                    if(tag!= null)
                    {
                        tagName = tag.Name;
                    }

                    if (System.IO.File.Exists(trainingDataList[i].FilePath))
                    {
                        FileInfo fileInfo = new FileInfo(trainingDataList[i].FilePath);

                        this.trainingDataTable.Rows.Add(id, createDate, name, tagName, sampleCount, balance, (fileInfo.Length/1048576.0).ToString("N0") + " MB");
                    }
                    else
                    {
                        this.trackDao.DeleteTrainingData(trainingDataList[i].Id);
                    }
                }
            }
            this.traningDataListBindingSource.DataSource = this.trainingDataTable;
            this.view.SetTraningDataListBindingSource(this.traningDataListBindingSource);

        }
        private void IsChromaFeaturesEnabledEvent(object sender, ListEventArgs e)
        {
            IsChromaFeaturesEnabled = e.BooleanField1;
        }
        private void IsMFCCsEnabledEvent(object sender, ListEventArgs e)
        {
            IsMFCCsEnabled = e.BooleanField1;
        }
        private void IsSpectralContrastEnabledEvent(object sender, ListEventArgs e)
        {
            IsSpectralContrastEnabled = e.BooleanField1;
        }
        private void IsHPCPEnabledEvent(object sender, ListEventArgs e)
        {
            IsHPCPEnabled = e.BooleanField1;
        }
        private void IsHPSEnabledEvent(object sender, ListEventArgs e)
        {
            IsHPSEnabled = e.BooleanField1;
        }
        private void IsSpectralCentroidEnabledEvent(object sender, ListEventArgs e)
        {
            IsSpectralCentroidEnabled = e.BooleanField1;
        }
        private void IsTonnetzFeaturesEnabledEvent(object sender, ListEventArgs e)
        {
            IsTonnetzFeaturesEnabled = e.BooleanField1;
        }
        private void IsPitchEnabledEvent(object sender, ListEventArgs e)
        {
            IsPitchEnabled = e.BooleanField1;
        }
        private void IsRMSEnabledEvent(object sender, ListEventArgs e)
        {
            IsRmsEnergyEnabled = e.BooleanField1;
        }
        private void IsZCREnabledEvent(object sender, ListEventArgs e)
        {
            IsZeroCrossingRateEnabled = e.BooleanField1;
        }
        private void IsSpectralBandwidthEnabledEvent(object sender, ListEventArgs e)
        {
            IsSpectralBandwidthEnabled = e.BooleanField1;
        }
        private void BatchProcessChangedEvent(object sender, ListEventArgs e)
        {
            this.BatchSize = e.IntegerField1;
        }
        private String hdf5FilePath { get; set; }
        private bool isGenerating { get; set; }
        private CancellationTokenSource cancellationTokenSourceForGenerating { get; set; }
        private void GenerateTrainingDataEvent(object sender, EventArgs e)
        {
            if (isGenerating)
            {
                MessageBox.Show("Generating is in progress, please wait!", "Training Model Validation", MessageBoxButtons.OK);
            }
            else
            {
                if (this.CurrentTag == null || this.CurrentPlaylist == null)
                {
                    MessageBox.Show("Tag and Playlist must be set!", "Training Model Validation", MessageBoxButtons.OK);
                }
                else if (this.trackList == null || this.trackList.Count == 0)
                {
                    MessageBox.Show("The model training tracklist is empty!", "Training Model Validation", MessageBoxButtons.OK);
                }
                else if (
                    !IsChromaFeaturesEnabled &&
                    !IsHPCPEnabled &&
                    !IsHPSEnabled &&
                    !IsMFCCsEnabled &&
                    !IsTonnetzFeaturesEnabled &&
                    !IsPitchEnabled &&
                    !IsRmsEnergyEnabled &&
                    !IsSpectralBandwidthEnabled &&
                    !IsSpectralCentroidEnabled &&
                    !IsSpectralContrastEnabled &&
                    !IsZeroCrossingRateEnabled)
                {
                    MessageBox.Show("At least one feature must be set!", "Training Model Validation", MessageBoxButtons.OK);
                }
                else
                {
                    
                    hdf5FilePath = this.SaveTrainingDataSet();
                    if (!String.IsNullOrEmpty(hdf5FilePath))
                    {
                        isGenerating = true;
                        this.view.ChangeGeneratingStatus(true);
                        cancellationTokenSourceForGenerating = new CancellationTokenSource();
                        this.GenerateTrainingDataEvent(cancellationTokenSourceForGenerating.Token);
                    }
                }
            }
        }
        private String SaveTrainingDataSet()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "HDF5 files (*.hdf5)|*.hdf5";
            sfd.RestoreDirectory = true;
            sfd.FileName = "TrainingDataSet_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + this.CurrentPlaylist.Name + "_" + this.CurrentTag.Name;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                return sfd.FileName;
            }
            else
            {
                return "";
            }
        }
        private async void GenerateTrainingDataEvent(CancellationToken cancellationToken)
        {
            try
            {
                await GenerateTrainingDataAsync(cancellationToken);
            }
            finally
            {
                isGenerating = false;
                this.view.ChangeGeneratingStatus(false);
            }
        }
        private async Task GenerateTrainingDataAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    int batchSize = this.BatchSize; // Adjust the batch size as needed
                    int totalTracks = this.trackList.Count;
                    int processedTracks = 0;

                    if (!String.IsNullOrEmpty(hdf5FilePath))
                    {
                        DateTime startTime = DateTime.Now;
                        DateTime lastUpdateTime = DateTime.Now;

                        this.ConcBag.Add(new MessageTest()
                        {
                            LogState = LogState.ParallelProcess,
                            Sum = processedTracks,
                            Total = totalTracks,
                        });

                        for (int i = 0; i < totalTracks; i += batchSize)
                        {
                            var batch = this.trackList.Skip(i).Take(batchSize).ToList();

                            ConcurrentBag<Track> concurrentTracks = new ConcurrentBag<Track>(batch);

                            Parallel.ForEach(concurrentTracks, track =>
                            {
                                if (cancellationToken.IsCancellationRequested)
                                {
                                    return;
                                }

                                if (track.TrackTagValues.Exists(x => x.TagName == this.CurrentTag.Name))
                                {
                                    String trainingTagValue = track.TrackTagValues.Find(x => x.TagName == this.CurrentTag.Name).TagValueName;
                                    if (!String.IsNullOrEmpty(trainingTagValue))
                                    {
                                        this.AddTrack(track.Path, trainingTagValue, cancellationToken);
                                        Interlocked.Increment(ref processedTracks);

                                        Console.WriteLine($"Add to model: {processedTracks}/{totalTracks} ready");

                                        TimeSpan elapsedTime = DateTime.Now - lastUpdateTime;
                                        lastUpdateTime = DateTime.Now;

                                        TimeSpan remaining = TimeSpan.Zero;
                                        if (processedTracks > 0)
                                        {
                                            double averageTimePerTrack = (DateTime.Now - startTime).TotalSeconds / processedTracks;
                                            double remainingTimeInSeconds = averageTimePerTrack * (totalTracks - processedTracks);
                                            remaining = TimeSpan.FromSeconds(remainingTimeInSeconds);
                                        }

                                        this.ConcBag.Add(new MessageTest()
                                        {
                                            LogState = LogState.ParallelProcess,
                                            Sum = processedTracks,
                                            Total = totalTracks,
                                            RemainingTime = remaining
                                        });
                                    }
                               }
                            });

                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }

                            try
                            {
                                this.WriteToHDF5(hdf5FilePath);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }

                            tracks.Clear();
                        }
                    }

                    TrainingData trainingData = new TrainingData()
                    {
                        Id = this.trackDao.GetNextId(TableName.TrainingData.ToString()),
                        FilePath = this.hdf5FilePath,
                        TagId = this.CurrentTag.Id,
                        Name = this.CurrentPlaylist?.Name,
                        CreateDate = DateTime.Now,
                        SampleCount = this.trackList.Count,
                        Balance = this.balance
                    };

                    this.trackDao.CreateTrainingData(trainingData);


                    this.ConcBag.Add(new MessageTest()
                    {
                        LogState = LogState.Finish,
                        Sum = processedTracks,
                        Total = totalTracks,
                        RemainingTime = new TimeSpan(),
                        EstimatedSize = 0,
                        Log = "COMPLETED SUCCESSFULLY!\n"
                    });

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });

            this.InitializeTrainingDataListContent();
        }
        public ObservableConcurrentBag<MessageTest> ConcBag { get; set; }
        public class MessageTest
        {
            public int Sum { get; set; }
            public int Total { get; set; }
            public TimeSpan RemainingTime { get; set; }
            public double EstimatedSize { get; set; }
            public String Log { get; set; }
            public int ExtractionProgressValue { get; set; }
            public int CurrentSample { get; set; }
            public int TotalSamples { get; set; }
            public String FilePath { get; set; }
            public LogState LogState { get; set; }

            public MessageTest(int sum, int total, TimeSpan remainingTime)
            {
                this.Sum = sum;
                this.Total = total;
                this.RemainingTime = remainingTime;
            }

            public MessageTest()
            {
            }
            public MessageTest(double estimatedSize)
            {
                this.EstimatedSize = estimatedSize;
            }
        }
        private void ConcBag_ItemAdded(object sender, ItemAddedEventArgs<MessageTest> e)
        {
            Console.WriteLine($"Item added: {e.Item}");
            this.view.UpdateProgressOnView(e.Item);

            if (this.ConcBag.Count > 100)
            {
                while (!this.ConcBag.IsEmpty)
                {
                    this.ConcBag.TryTake(out _);
                }
            }
        }
        public void CancelGenerationEvent(object sender, EventArgs e)
        {
            if (cancellationTokenSourceForGenerating != null)
            {
                this.ConcBag.Add(new MessageTest()
                {
                    LogState = LogState.Canceled,
                    RemainingTime = new TimeSpan(),
                    Sum = 0,
                    Total = 0,
                    EstimatedSize = 0,
                    Log = "GENERATING CANCELED!\n"
                }); ;

                cancellationTokenSourceForGenerating.Cancel();
                isGenerating = false;
                this.view.ChangeGeneratingStatus(false);

                if (System.IO.File.Exists(hdf5FilePath))
                {
                    System.IO.File.Delete(hdf5FilePath);
                }
            }
        }

        List<TrackForTraining> tracks = new List<TrackForTraining>();
        public class TrackForTraining
        {
            public String Path { get; set; }
            public String Key { get; set; }
            public float[] Features { get; set; }
        }
        public void AddTrack(string filePath, string tagValue, CancellationToken cancellationToken)
        {
            var features = ExtractFeatures(filePath, cancellationToken);
            tracks.Add(new TrackForTraining { Path = filePath, Key = tagValue, Features = features });
        }


      /*  private float[] ExtractFeatures(string filePath, CancellationToken cancellationToken)
        {
            const int aggregationWindowSize = 44100; // Assuming 44.1kHz sample rate for 1-second windows
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                float[] buffer = new float[aggregationWindowSize];
                int samplesRead;
                List<float> aggregatedFeatures = new List<float>();

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    // Perform FFT and extract features as before
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i].X = buffer[i];
                        fftBuffer[i].Y = 0;
                    }
                    FastFourierTransform.FFT(true, (int)Math.Log(buffer.Length, 2.0), fftBuffer);

                    // Extract features (e.g., MFCCs, chroma features)
                    float[] mfccFeatures = ExtractMFCCs(buffer, reader.WaveFormat.SampleRate);
                    aggregatedFeatures.AddRange(mfccFeatures);

                    // Aggregate features over the window
                    float mean = mfccFeatures.Average();
                    float variance = mfccFeatures.Select(f => (f - mean) * (f - mean)).Average();
                    aggregatedFeatures.Add(mean);
                    aggregatedFeatures.Add(variance);

                    // Add other feature extraction and aggregation methods as needed
                }

                // Ensure the feature vector has a fixed length
                const int fixedFeatureLength = 500; // Set your desired fixed feature length
                if (aggregatedFeatures.Count < fixedFeatureLength)
                {
                    aggregatedFeatures.AddRange(new float[fixedFeatureLength - aggregatedFeatures.Count]);
                }
                else if (aggregatedFeatures.Count > fixedFeatureLength)
                {
                    aggregatedFeatures = aggregatedFeatures.Take(fixedFeatureLength).ToList();
                }

                return aggregatedFeatures.ToArray();
            }
        }*/
        /*
        private List<float> ExtractFeatures(ISampleProvider sampleProvider, int sampleRate)
        {
            float[] buffer = new float[sampleRate * 2]; // Buffer for 1 second of audio
            int samplesRead;
            List<float> features = new List<float>();

            while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
            {
                // Perform FFT
                Complex[] fftBuffer = new Complex[buffer.Length];
                for (int i = 0; i < buffer.Length; i++)
                {
                    fftBuffer[i] = new Complex(buffer[i], 0);
                }
                Fourier.Forward(fftBuffer, FourierOptions.Matlab);

                // Extract features
                float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                features.AddRange(chromaFeatures);

                float pitch = ExtractPitch(buffer, sampleRate);
                features.Add(pitch);

                // Add other feature extraction methods as needed
            }

            return features;
        }



        */
        private float[] ExtractFeatures(string filePath, CancellationToken cancellationToken)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                float[] buffer = new float[reader.WaveFormat.SampleRate * 2];
                int samplesRead;
                List<float> features = new List<float>();
                int totalSamples = (int)Math.Ceiling((double)reader.Length / (buffer.Length * 2 * 2)); // Adjust for bytes per sample and channels
                int currentSample = 0;

                if (enableExtractFeaturesLog)
                    Console.WriteLine($"Total samples: {totalSamples}");

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    // Perform FFT
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i].X = buffer[i];
                        fftBuffer[i].Y = 0;
                    }
                    FastFourierTransform.FFT(true, (int)Math.Log(buffer.Length, 2.0), fftBuffer);

                    if (IsChromaFeaturesEnabled)
                    {
                        // Extract chroma features from FFT result
                        float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                        features.AddRange(chromaFeatures);

                        if (IsTonnetzFeaturesEnabled)
                        {
                            // Extract Tonnetz features
                            float[] tonnetzFeatures = ExtractTonnetzFeatures(chromaFeatures);
                            features.AddRange(tonnetzFeatures);
                        }
                    }

                    if (IsMFCCsEnabled)
                    {
                        // Extract MFCCs
                        float[] mfccFeatures = ExtractMFCCs(buffer, reader.WaveFormat.SampleRate);
                        features.AddRange(mfccFeatures);
                    }

                    if (IsSpectralContrastEnabled)
                    {
                        // Extract spectral contrast
                        float[] spectralContrastFeatures = ExtractSpectralContrast(fftBuffer, SpectralContrastFeatureLength);
                        features.AddRange(spectralContrastFeatures);
                    }

                    if (IsHPCPEnabled)
                    {
                        // Extract HPCP
                        float[] hpcpFeatures = ExtractHPCP(fftBuffer, reader.WaveFormat.SampleRate);
                        features.AddRange(hpcpFeatures);
                    }

                    if (IsHPSEnabled)
                    {
                        // Harmonic/Percussive Source Separation
                        var (harmonic, percussive) = ExtractHPSFeatures(buffer, reader.WaveFormat.SampleRate);
                        features.AddRange(harmonic);
                        features.AddRange(percussive);
                    }

                    if (IsSpectralCentroidEnabled)
                    {
                        // Extract spectral centroid
                        float spectralCentroid = ExtractSpectralCentroid(fftBuffer);
                        features.Add(spectralCentroid);
                    }

                    if (IsSpectralBandwidthEnabled)
                    {
                        // Extract spectral bandwidth
                        float spectralBandwidth = ExtractSpectralBandwidth(buffer);
                        features.Add(spectralBandwidth);
                    }

                    if (IsZeroCrossingRateEnabled)
                    {
                        // Extract zero-crossing rate
                        float zcr = ExtractZeroCrossingRate(buffer);
                        features.Add(zcr);
                    }

                    if (IsRmsEnergyEnabled)
                    {
                        // Extract RMS energy
                        float rms = ExtractRmsEnergy(buffer);
                        features.Add(rms);
                    }

                    if (IsPitchEnabled)
                    {
                        // Extract pitch
                        float pitch = ExtractPitch(buffer, reader.WaveFormat.SampleRate);
                        features.Add(pitch);
                    }

                    currentSample++;
                    int progressValue = currentSample * 100 / totalSamples;

                    if (enableExtractFeaturesLog)
                        Console.WriteLine($"{progressValue}% complete (Sample {currentSample} of {totalSamples}) - File: {filePath}");

                    this.ConcBag.Add(new MessageTest()
                    {
                        LogState = LogState.Extraction,
                        ExtractionProgressValue = progressValue,
                        CurrentSample = currentSample,
                        TotalSamples = totalSamples,
                        FilePath = filePath
                    });

                }

                return features.ToArray();
            }
        }

        public const int ChromaFeatureLength = 12;
        public const int MFCCFeatureLength = 13;
        public const int SpectralContrastFeatureLength = 6;
        public const int HPCPFeatureLength = 12;
        public const int SpectralCentroidFeatureLength = 1;
        public const int SpectralBandwidthFeatureLength = 1;
        public const int ZeroCrossingRateFeatureLength = 1;
        public const int RMSEnergyFeatureLength = 1;
        public const int PitchFeatureLength = 1;
        public const int TonnetzFeatureLength = 6;
        //public const int HarmonicFeatureCount = 10; 
        //public const int PercussiveFeatureCount = 10; 
        private float[] ExtractChromaFeatures(Complex[] fftBuffer)
        {
            float[] chromaFeatures = new float[ChromaFeatureLength];
            int totalBins = fftBuffer.Length / 2;

            for (int i = 0; i < totalBins; i++)
            {
                double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                if (magnitude > 0)
                {
                    int pitchClass = (int)Math.Round(12 * Log2(i + 1)) % 12; // Use i + 1 to avoid log(0)
                    if (pitchClass < 0)
                    {
                        pitchClass += 12; // Ensure pitchClass is non-negative
                    }
                    chromaFeatures[pitchClass] += (float)magnitude;
                }

                // Log progress
                if (enableExtractChromaFeaturesLog)
                {
                    if (i % (totalBins / 10) == 0) // Log every 10% of progress
                    {
                        Console.WriteLine($"ExtractChromaFeatures progress: {i * 100 / totalBins}% complete");
                    }
                }

            }
            if (enableExtractChromaFeaturesLog)
                Console.WriteLine($"ExtractChromaFeatures progress: {100}% complete");

            return chromaFeatures;
        }
        private float[] ExtractMFCCs(float[] buffer, int sampleRate, int numCoefficients = MFCCFeatureLength, int numFilters = 26, int fftSize = 512)
        {
            // Step 1: Pre-emphasis
            float[] preEmphasized = new float[buffer.Length];
            float preEmphasis = 0.97f;
            for (int i = 1; i < buffer.Length; i++)
            {
                preEmphasized[i] = buffer[i] - preEmphasis * buffer[i - 1];
            }
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 1: Pre-emphasis completed.");

            // Step 2: Framing
            int frameSize = fftSize;
            int frameStep = frameSize / 2;
            int numFrames = (buffer.Length - frameSize) / frameStep + 1;
            float[][] frames = new float[numFrames][];
            for (int i = 0; i < numFrames; i++)
            {
                frames[i] = new float[frameSize];
                Array.Copy(preEmphasized, i * frameStep, frames[i], 0, frameSize);
            }
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 2: Framing completed.");


            // Step 3: Windowing
            for (int i = 0; i < numFrames; i++)
            {
                for (int j = 0; j < frameSize; j++)
                {
                    frames[i][j] *= (float)(0.54 - 0.46 * Math.Cos(2 * Math.PI * j / (frameSize - 1))); // Hamming window
                }
            }
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 3: Windowing completed.");

            // Step 4: FFT and Power Spectrum
            float[][] powerSpectrum = new float[numFrames][];
            for (int i = 0; i < numFrames; i++)
            {
                Complex[] fftBuffer = new Complex[fftSize];
                for (int j = 0; j < frameSize; j++)
                {
                    fftBuffer[j] = new Complex { X = frames[i][j], Y = 0 };
                }
                FastFourierTransform.FFT(true, (int)Math.Log(fftSize, 2.0), fftBuffer);
                powerSpectrum[i] = new float[fftSize / 2 + 1];
                for (int j = 0; j < fftSize / 2 + 1; j++)
                {
                    powerSpectrum[i][j] = (float)(fftBuffer[j].X * fftBuffer[j].X + fftBuffer[j].Y * fftBuffer[j].Y) / fftSize;
                }
            }
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 4: FFT and Power Spectrum completed.");

            // Step 5: Mel Filter Banks
            float[][] melFilterBanks = new float[numFilters][];
            for (int i = 0; i < numFilters; i++)
            {
                melFilterBanks[i] = new float[fftSize / 2 + 1];
            }
            float[] melPoints = new float[numFilters + 2];
            for (int i = 0; i < melPoints.Length; i++)
            {
                melPoints[i] = MelScale(i * (sampleRate / 2) / (numFilters + 1));
            }
            for (int i = 1; i <= numFilters; i++)
            {
                for (int j = 0; j < fftSize / 2 + 1; j++)
                {
                    float freq = j * sampleRate / fftSize;
                    if (freq >= melPoints[i - 1] && freq <= melPoints[i])
                    {
                        melFilterBanks[i - 1][j] = (freq - melPoints[i - 1]) / (melPoints[i] - melPoints[i - 1]);
                    }
                    else if (freq >= melPoints[i] && freq <= melPoints[i + 1])
                    {
                        melFilterBanks[i - 1][j] = (melPoints[i + 1] - freq) / (melPoints[i + 1] - melPoints[i]);
                    }
                }
            }
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 5: Mel Filter Banks completed.");


            // Step 6: Mel Spectrum
            float[][] melSpectrum = new float[numFrames][];
            for (int i = 0; i < numFrames; i++)
            {
                melSpectrum[i] = new float[numFilters];
                for (int j = 0; j < numFilters; j++)
                {
                    for (int k = 0; k < fftSize / 2 + 1; k++)
                    {
                        melSpectrum[i][j] += powerSpectrum[i][k] * melFilterBanks[j][k];
                    }
                    melSpectrum[i][j] = (float)Math.Log(melSpectrum[i][j] + 1e-10); // Logarithm of Mel spectrum
                }
            }
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 6: Mel Spectrum completed.");

            // Step 7: Discrete Cosine Transform (DCT)
            float[][] mfcc = new float[numFrames][];
            for (int i = 0; i < numFrames; i++)
            {
                mfcc[i] = new float[numCoefficients];
                for (int j = 0; j < numCoefficients; j++)
                {
                    for (int k = 0; k < numFilters; k++)
                    {
                        mfcc[i][j] += melSpectrum[i][k] * (float)Math.Cos(Math.PI * j * (k + 0.5) / numFilters);
                    }
                }
            }
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 7: Discrete Cosine Transform (DCT) completed.");

            // Flatten the MFCCs into a single array
            List<float> mfccFeatures = new List<float>();
            foreach (var frame in mfcc)
            {
                mfccFeatures.AddRange(frame);
            }
            if (enableExtractMFCCsLog)
                Console.WriteLine("MFCC extraction completed.");

            return mfccFeatures.ToArray();
        }
        private float MelScale(float freq)
        {
            return 2595 * (float)Math.Log10(1 + freq / 700);
        }
        private float[] ExtractSpectralContrast(Complex[] fftBuffer, int numBands = SpectralContrastFeatureLength)
        {
            if (fftBuffer == null || fftBuffer.Length == 0)
            {
                throw new ArgumentException("FFT buffer must not be null or empty.");
            }

            float[] spectralContrast = new float[numBands];
            int[] bandEdges = new int[numBands + 1];

            // Define frequency bands more evenly
            for (int i = 0; i <= numBands; i++)
            {
                bandEdges[i] = (int)Math.Round(i * (fftBuffer.Length / (double)numBands));
            }
            if (enableExtractSpectralContrastLog)
                Console.WriteLine("Frequency bands defined.");

            for (int band = 0; band < numBands; band++)
            {
                int start = bandEdges[band];
                int end = bandEdges[band + 1];

                List<double> magnitudes = new List<double>();
                for (int i = start; i < end; i++)
                {
                    double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                    magnitudes.Add(magnitude);
                }

                magnitudes.Sort();
                double median = magnitudes[magnitudes.Count / 2];
                double min = magnitudes[0];
                double max = magnitudes[magnitudes.Count - 1];

                spectralContrast[band] = (float)(max - min) / (float)(median + 1e-10);

                // Log progress for each band
                if (enableExtractSpectralContrastLog)
                    Console.WriteLine($"Processed band {band + 1}/{numBands}: start={start}, end={end}, min={min}, max={max}, median={median}");
            }

            if (enableExtractSpectralContrastLog)
                Console.WriteLine("Spectral contrast extraction completed.");

            // Log the length of the returned array
            if (enableExtractSpectralContrastLog)
                Console.WriteLine($"Spectral contrast features length: {spectralContrast.Length}");

            return spectralContrast;
        }
        private float[] ExtractHPCP(Complex[] fftBuffer, int sampleRate)
        {
            float[] hpcp = new float[HPCPFeatureLength];
            int numBins = fftBuffer.Length / 2;
            double tuningFrequency = 440.0; // A4 tuning frequency

            for (int i = 0; i < numBins; i++)
            {
                double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                if (magnitude > 0)
                {
                    double frequency = i * sampleRate / (double)fftBuffer.Length;
                    double pitchClass = 12 * Log2(frequency / tuningFrequency);
                    int pitchClassIndex = (int)Math.Round(pitchClass) % 12;
                    if (pitchClassIndex < 0)
                    {
                        pitchClassIndex += 12; // Ensure pitchClassIndex is non-negative
                    }
                    hpcp[pitchClassIndex] += (float)magnitude;
                }
                if (enableExtractHPCPLog)
                {
                    // Log progress every 10% of the bins processed
                    if (i % (numBins / 10) == 0)
                    {
                        int progress = (i * 100) / numBins;
                        Console.WriteLine($"HPCP extraction progress: {progress}% complete");
                    }
                }
            }

            return hpcp;
        }
        private float ExtractSpectralCentroid(Complex[] fftBuffer)
        {
            double weightedSum = 0;
            double totalMagnitude = 0;
            int numBins = fftBuffer.Length / 2;

            if (enableExtractSpectralCentroidLog)
                Console.WriteLine("Starting spectral centroid calculation...");

            for (int i = 0; i < numBins; i++)
            {
                double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                weightedSum += i * magnitude;
                totalMagnitude += magnitude;

                if (enableExtractSpectralCentroidLog)
                {
                    // Log progress every 10% of the bins processed
                    if (i % (numBins / 10) == 0)
                    {
                        int progress = (i * 100) / numBins;
                        Console.WriteLine($"Spectral centroid calculation progress: {progress}% complete");
                    }
                }

            }

            float spectralCentroid = (float)(weightedSum / totalMagnitude);
            if (enableExtractSpectralCentroidLog)
                Console.WriteLine($"Spectral centroid calculation completed. Result: {spectralCentroid}");

            return spectralCentroid;
        }
        private float ExtractSpectralBandwidth(float[] audioSamples)
        {
            // Perform Fourier Transform
            int fftSize = (int)Math.Pow(2, Math.Ceiling(Math.Log(audioSamples.Length, 2)));
            Complex[] fftBuffer = new Complex[fftSize];
            for (int i = 0; i < audioSamples.Length; i++)
            {
                fftBuffer[i].X = audioSamples[i];
                fftBuffer[i].Y = 0;
            }

            FastFourierTransform.FFT(true, (int)Math.Log(fftSize, 2.0), fftBuffer);

            // Calculate Magnitudes
            double[] magnitudes = fftBuffer.Select(c => Math.Sqrt(c.X * c.X + c.Y * c.Y)).ToArray();

            // Calculate Spectral Bandwidth
            double mean = magnitudes.Average();
            double sum = magnitudes.Sum(m => Math.Pow(m - mean, 2));
            double spectralBandwidth = Math.Sqrt(sum / magnitudes.Length);

            return (float)spectralBandwidth;

        }
        private (float[] harmonic, float[] percussive) ExtractHPSFeatures(float[] buffer, int sampleRate)
        {
            // Convert buffer to spectrogram
            var spectrogram = ComputeSpectrogram(buffer, sampleRate);

            // Apply horizontal median filter for harmonic components
            var harmonicSpectrogram = ApplyMedianFilter(spectrogram, filterDirection: "horizontal");

            // Apply vertical median filter for percussive components
            var percussiveSpectrogram = ApplyMedianFilter(spectrogram, filterDirection: "vertical");

            // Convert spectrograms back to time domain
            var harmonic = InverseSpectrogram(harmonicSpectrogram, sampleRate);
            var percussive = InverseSpectrogram(percussiveSpectrogram, sampleRate);

            return (harmonic, percussive);
        }
        private float[][] ComputeSpectrogram(float[] buffer, int sampleRate, int fftSize = 512, int hopSize = 256)
        {
            int numFrames = (buffer.Length - fftSize) / hopSize + 1;
            float[][] spectrogram = new float[numFrames][];

            for (int i = 0; i < numFrames; i++)
            {
                float[] frame = new float[fftSize];
                Array.Copy(buffer, i * hopSize, frame, 0, fftSize);

                // Apply a window function (e.g., Hamming window)
                for (int j = 0; j < fftSize; j++)
                {
                    frame[j] *= (float)(0.54 - 0.46 * Math.Cos(2 * Math.PI * j / (fftSize - 1)));
                }

                // Perform FFT
                Complex[] fftBuffer = new Complex[fftSize];
                for (int j = 0; j < fftSize; j++)
                {
                    fftBuffer[j] = new Complex { X = frame[j], Y = 0 };
                }

                FastFourierTransform.FFT(true, (int)Math.Log(fftSize, 2.0), fftBuffer);

                // Compute magnitude spectrum
                float[] magnitudeSpectrum = new float[fftSize / 2 + 1];
                for (int j = 0; j < fftSize / 2 + 1; j++)
                {
                    magnitudeSpectrum[j] = (float)Math.Sqrt(fftBuffer[j].X * fftBuffer[j].X + fftBuffer[j].Y * fftBuffer[j].Y);
                }

                spectrogram[i] = magnitudeSpectrum;
            }

            return spectrogram;
        }
        private float[][] ApplyMedianFilter(float[][] spectrogram, string filterDirection)
        {
            int numRows = spectrogram.Length;
            int numCols = spectrogram[0].Length;
            float[][] filteredSpectrogram = new float[numRows][];

            for (int i = 0; i < numRows; i++)
            {
                filteredSpectrogram[i] = new float[numCols];
            }

            int filterSize = 3; // You can adjust the filter size as needed
            int halfFilterSize = filterSize / 2;

            if (filterDirection == "horizontal")
            {
                for (int i = 0; i < numRows; i++)
                {
                    for (int j = 0; j < numCols; j++)
                    {
                        List<float> window = new List<float>();

                        for (int k = -halfFilterSize; k <= halfFilterSize; k++)
                        {
                            int colIndex = j + k;
                            if (colIndex >= 0 && colIndex < numCols)
                            {
                                window.Add(spectrogram[i][colIndex]);
                            }
                        }

                        window.Sort();
                        filteredSpectrogram[i][j] = window[window.Count / 2];
                    }
                }
            }
            else if (filterDirection == "vertical")
            {
                for (int j = 0; j < numCols; j++)
                {
                    for (int i = 0; i < numRows; i++)
                    {
                        List<float> window = new List<float>();

                        for (int k = -halfFilterSize; k <= halfFilterSize; k++)
                        {
                            int rowIndex = i + k;
                            if (rowIndex >= 0 && rowIndex < numRows)
                            {
                                window.Add(spectrogram[rowIndex][j]);
                            }
                        }

                        window.Sort();
                        filteredSpectrogram[i][j] = window[window.Count / 2];
                    }
                }
            }
            else
            {
                throw new ArgumentException("Invalid filter direction. Use 'horizontal' or 'vertical'.");
            }

            return filteredSpectrogram;
        }
        private float[] InverseSpectrogram(float[][] spectrogram, int sampleRate, int fftSize = 512, int hopSize = 256)
        {
            int numFrames = spectrogram.Length;
            int frameSize = fftSize;
            float[] reconstructedSignal = new float[numFrames * hopSize + frameSize];

            for (int i = 0; i < numFrames; i++)
            {
                // Create a complex array for the inverse FFT
                Complex[] ifftBuffer = new Complex[fftSize];
                for (int j = 0; j < fftSize / 2 + 1; j++)
                {
                    ifftBuffer[j] = new Complex { X = spectrogram[i][j], Y = 0 };
                    if (j > 0 && j < fftSize / 2)
                    {
                        ifftBuffer[fftSize - j] = new Complex { X = spectrogram[i][j], Y = 0 };
                    }
                }

                // Perform inverse FFT
                FastFourierTransform.FFT(false, (int)Math.Log(fftSize, 2.0), ifftBuffer);

                // Apply the window function (e.g., Hamming window)
                for (int j = 0; j < frameSize; j++)
                {
                    ifftBuffer[j].X *= (float)(0.54 - 0.46 * Math.Cos(2 * Math.PI * j / (frameSize - 1)));
                }

                // Overlap-add the reconstructed frame to the output signal
                for (int j = 0; j < frameSize; j++)
                {
                    reconstructedSignal[i * hopSize + j] += ifftBuffer[j].X;
                }
            }

            return reconstructedSignal;
        }
        private float[] ExtractTonnetzFeatures(float[] chromaFeatures)
        {
            // Ensure the chroma features are in the correct format
            if (chromaFeatures.Length % ChromaFeatureLength != 0)
            {
                throw new ArgumentException("Chroma features length must be a multiple of 12.");
            }

            int numFrames = chromaFeatures.Length / ChromaFeatureLength;
            float[] tonnetzFeatures = new float[numFrames * TonnetzFeatureLength];


            // Tonnetz transformation matrix
            float[,] tonnetzMatrix = new float[,]
            {
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }
            };

            for (int frame = 0; frame < numFrames; frame++)
            {
                float[] chromaFrame = new float[ChromaFeatureLength];
                Array.Copy(chromaFeatures, frame * ChromaFeatureLength, chromaFrame, 0, ChromaFeatureLength);

                for (int i = 0; i < TonnetzFeatureLength; i++)
                {
                    float tonnetzValue = 0;
                    for (int j = 0; j < ChromaFeatureLength; j++)
                    {
                        tonnetzValue += tonnetzMatrix[i, j] * chromaFrame[j];
                    }
                    tonnetzFeatures[frame * TonnetzFeatureLength + i] = tonnetzValue;
                }
            }

            return tonnetzFeatures;
        }
        private float ExtractZeroCrossingRate(float[] buffer)
        {
            int zeroCrossings = 0;
            for (int i = 1; i < buffer.Length; i++)
            {
                if ((buffer[i - 1] >= 0 && buffer[i] < 0) || (buffer[i - 1] < 0 && buffer[i] >= 0))
                {
                    zeroCrossings++;
                }
            }
            return (float)zeroCrossings / buffer.Length;
        }
        private float ExtractRmsEnergy(float[] buffer)
        {
            double sumOfSquares = buffer.Select(sample => sample * sample).Sum();
            return (float)Math.Sqrt(sumOfSquares / buffer.Length);
        }
        private float ExtractPitch(float[] buffer, int sampleRate)
        {
            int maxLag = sampleRate / 50; // Minimum frequency of 50 Hz
            int minLag = sampleRate / 500; // Maximum frequency of 500 Hz
            float[] autoCorrelation = new float[maxLag];

            // Calculate auto-correlation
            for (int lag = minLag; lag < maxLag; lag++)
            {
                for (int i = 0; i < buffer.Length - lag; i++)
                {
                    autoCorrelation[lag] += buffer[i] * buffer[i + lag];
                }
            }

            // Find the lag with the maximum auto-correlation value
            int bestLag = minLag;
            for (int lag = minLag + 1; lag < maxLag; lag++)
            {
                if (autoCorrelation[lag] > autoCorrelation[bestLag])
                {
                    bestLag = lag;
                }
            }

            // Calculate the pitch
            float pitch = sampleRate / (float)bestLag;
            return pitch;
        }
        private static double Log2(double x)
        {
            return Math.Log(x) / Math.Log(2);
        }
        public void WriteToHDF5(string fullPath)
        {
            using (LoadingDialog loadingDialog = new LoadingDialog())
            {
                loadingDialog.Show();

                // Open the HDF5 file for read/write access, creating it if it doesn't exist
                long fileId = H5F.open(fullPath, H5F.ACC_RDWR);
                if (fileId < 0)
                {
                    fileId = H5F.create(fullPath, H5F.ACC_TRUNC);
                    if (fileId < 0)
                    {
                        throw new Exception("Failed to create or open HDF5 file.");
                    }
                }

                int writtenTracks = 0;
                long totalSizeInBytes = 0;

                foreach (var track in tracks)
                {
                    try
                    {
                        // Estimate size of the key attribute
                        long keyAttrSize = track.Key.Length;

                        // Estimate size of the features dataset
                        long featuresSize = track.Features.Length * sizeof(float);

                        // Add to total size
                        totalSizeInBytes += keyAttrSize + featuresSize;

                        // Create a group for each track
                        long groupId = H5G.create(fileId, track.Path);
                        if (groupId < 0)
                        {
                            throw new Exception($"Failed to create group for track {track.Path}.");
                        }

                        // Save key as an attribute
                        long keyAttrType = H5T.copy(H5T.C_S1);
                        H5T.set_size(keyAttrType, new IntPtr(track.Key.Length));
                        long keyAttrSpace = H5S.create(H5S.class_t.SCALAR);
                        long keyAttrId = H5A.create(groupId, "Key", keyAttrType, keyAttrSpace);
                        byte[] keyBytes = System.Text.Encoding.ASCII.GetBytes(track.Key);
                        GCHandle keyHandle = GCHandle.Alloc(keyBytes, GCHandleType.Pinned);
                        H5A.write(keyAttrId, keyAttrType, keyHandle.AddrOfPinnedObject());
                        keyHandle.Free();
                        H5A.close(keyAttrId);
                        H5S.close(keyAttrSpace);
                        H5T.close(keyAttrType);

                        // Save features as a dataset
                        long spaceId = H5S.create_simple(1, new ulong[] { (ulong)track.Features.Length }, null);
                        long datasetId = H5D.create(groupId, "Features", H5T.NATIVE_FLOAT, spaceId);
                        GCHandle handle = GCHandle.Alloc(track.Features, GCHandleType.Pinned);
                        H5D.write(datasetId, H5T.NATIVE_FLOAT, H5S.ALL, H5S.ALL, H5P.DEFAULT, handle.AddrOfPinnedObject());
                        handle.Free();
                        H5D.close(datasetId);
                        H5S.close(spaceId);

                        H5G.close(groupId);

                        // Update the writing progress bar
                        writtenTracks++;

                        if (totalSizeInBytes > 0)
                        {
                            double totalSizeInMegabytes = totalSizeInBytes / (1024.0 * 1024.0);
                            loadingDialog.SetProcessDescription($"Written track {writtenTracks}/{tracks.Count} to HDF5 file. Estimated size: {totalSizeInMegabytes:F2} MB.");
                            loadingDialog.Refresh();

                            this.ConcBag.Add(new MessageTest()
                            {
                                LogState = LogState.FileReading,
                                EstimatedSize = totalSizeInMegabytes * this.trackList.Count
                            });

                            Console.WriteLine($"Written track {writtenTracks}/{tracks.Count} to HDF5 file. Estimated size: {totalSizeInMegabytes:F2} MB.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error writing track {track.Path}: {ex.Message}");
                    }
                }

                H5F.close(fileId);
            }
        }
        private void DeleteTrainingDataEvent(object sender, ListEventArgs e)
        {
            DialogResult messageBoxResult = MessageBox.Show("Do you really want to delete this training data?", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (messageBoxResult == DialogResult.Yes)
            {
                this.trackDao.DeleteTrainingData(e.IntegerField1);

                for (int i = 0; i < this.trainingDataTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(this.trainingDataTable.Rows[i]["Id"]) == e.IntegerField1)
                    {
                        this.trainingDataTable.Rows.RemoveAt(i);
                        break;
                    }
                }
            }
        }









        private String modelFilePath { get; set; }
        private bool isTraining { get; set; }
        private CancellationTokenSource cancellationTokenSourceForTraining { get; set; }
        private void TrainModelEvent(object sender, EventArgs e)
        {
            if (isTraining)
            {
                MessageBox.Show("Training is in progress, please wait!", "Training Model Validation", MessageBoxButtons.OK);
            }
            else
            {
                if (this.CurrentTrainingData == null)
                {
                    MessageBox.Show("Training Data must be load for trainng!", "Training Model Validation", MessageBoxButtons.OK);
                }
                else
                {

                    modelFilePath = this.SaveModel();
                    if (!String.IsNullOrEmpty(modelFilePath))
                    {
                        isTraining = true;
                        this.view.ChangeTrainingStatus(true);
                        cancellationTokenSourceForTraining = new CancellationTokenSource();
                        this.TrainModelEvent(cancellationTokenSourceForTraining.Token);
                    }

                }
            }
        }
        private String SaveModel()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "zip files (*.zip)|*.zip";
            sfd.RestoreDirectory = true;
            sfd.FileName = "Model_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + this.CurrentPlaylist.Name + "_" + this.CurrentTag.Name;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                return sfd.FileName;
            }
            else
            {
                return "";
            }
        }
        private async void TrainModelEvent(CancellationToken cancellationToken)
        {
            try
            {
                await TrainModelAsync(cancellationToken);
            }
            finally
            {
                isTraining = false;
                this.view.ChangeTrainingStatus(false);
            }
        }
        private async Task TrainModelAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    var models = TrainModels(this.CurrentTrainingData.FilePath);
                    var ensembleModel = CombineModels(models);
                }
                catch (Exception ex)
                {

                }
            });
        }

        private static class FeatureConstants
        {
            public const int FeatureCount = 100;
        }
        public class AudioData
        {
            [LoadColumn(0)]
            public string Path { get; set; }

            [LoadColumn(1)]
            public string Label { get; set; } // Keep the label as a string

            [LoadColumn(2, 2 + FeatureConstants.FeatureCount - 1)]
            [VectorType(FeatureConstants.FeatureCount)]
            public float[] Features { get; set; }
        }
        private MLContext _mlContext = new MLContext();
        private List<ITransformer> TrainModels(string fileName)
        {
            var models = new List<ITransformer>();
            int batchNumber = 0;

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(nameof(AudioData.Label))
            .Append(_mlContext.Transforms.Concatenate("Features", nameof(AudioData.Features)))
            .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
            .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(new SdcaMaximumEntropyMulticlassTrainer.Options
            {
                LabelColumnName = nameof(AudioData.Label),
                FeatureColumnName = "Features"
            }))
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // Load data in batches and train separate models
            foreach (var batch in LoadBatchesFromHDF5(fileName))
            {
                batchNumber++;
                Console.WriteLine($"Starting training for batch {batchNumber}...");

                var data = _mlContext.Data.LoadFromEnumerable(batch);
                var model = pipeline.Fit(data);
                models.Add(model);

                Console.WriteLine($"Completed training for batch {batchNumber}.");
            }

            Console.WriteLine("All batches have been processed and models trained.");
            return models;
        }
        private IEnumerable<List<AudioData>> LoadBatchesFromHDF5(string fileName)
        {
            const int batchSize = 1000;
            var allTracks = LoadFromHDF5(fileName);
            int totalBatches = (int)Math.Ceiling((double)allTracks.Count / batchSize);
            int currentBatch = 0;

            for (int i = 0; i < allTracks.Count; i += batchSize)
            {
                currentBatch++;
                Console.WriteLine($"Loading batch {currentBatch} of {totalBatches}...");
                yield return allTracks.Skip(i).Take(batchSize).Select(track => new AudioData
                {
                    Path = track.Path,
                    Label = track.Label, // Use the Label property
                    Features = track.Features
                }).ToList();
                Console.WriteLine($"Batch {currentBatch} loaded.");
            }
        }
        private ITransformer CombineModels(List<ITransformer> models)
        {
            // Load a sample dataset to get the schema
            var sampleData = LoadSampleData();
            var sampleDataView = _mlContext.Data.LoadFromEnumerable(sampleData);

            // Create a list to store the predictions
            var predictions = new List<float[]>();

            // Make predictions with each model
            foreach (var model in models)
            {
                var transformedData = model.Transform(sampleDataView);
                var predictionColumn = _mlContext.Data.CreateEnumerable<Prediction>(transformedData, reuseRowObject: false)
                .Select(p => p.Score)
                .ToArray();
                predictions.Add(predictionColumn);
            }

            // Average the predictions
            var averagedPredictions = new float[predictions[0].Length];
            for (int i = 0; i < averagedPredictions.Length; i++)
            {
                averagedPredictions[i] = predictions.Select(p => p[i]).Average();
            }

            // Create a new model with the averaged predictions
            var averagedModel = CreateAveragedModel(averagedPredictions, sampleDataView.Schema);

            // Save the combined model
            string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ensembleVocalDetectionModel.zip");
            _mlContext.Model.Save(averagedModel, sampleDataView.Schema, modelFilePath);
            Console.WriteLine($"Ensemble model saved to {modelPath}");

            return averagedModel;
        }

        private IEnumerable<AudioData> LoadSampleData()
        {
            var audioDataList = new List<AudioData>();

            // Open the HDF5 file
            long fileId = H5F.open(this.CurrentTrainingData.FilePath, H5F.ACC_RDONLY);
            if (fileId < 0)
            {
                throw new Exception("Failed to open HDF5 file.");
            }

            // Iterate over each group (track) in the file
            foreach (var trackPath in GetTrackPaths(fileId))
            {
                // Open the group
                long groupId = H5G.open(fileId, trackPath);
                if (groupId < 0)
                {
                    throw new Exception($"Failed to open group for track {trackPath}.");
                }

                // Read the key attribute
                long keyAttrId = H5A.open(groupId, "Key");
                long keyAttrType = H5A.get_type(keyAttrId);
                long keyAttrSpace = H5A.get_space(keyAttrId);
                int keyLength = (int)H5T.get_size(keyAttrType);
                byte[] keyBytes = new byte[keyLength];
                GCHandle keyHandle = GCHandle.Alloc(keyBytes, GCHandleType.Pinned);
                H5A.read(keyAttrId, keyAttrType, keyHandle.AddrOfPinnedObject());
                keyHandle.Free();
                string key = System.Text.Encoding.ASCII.GetString(keyBytes);
                H5A.close(keyAttrId);
                H5S.close(keyAttrSpace);
                H5T.close(keyAttrType);

                // Read the features dataset
                long datasetId = H5D.open(groupId, "Features");
                long spaceId = H5D.get_space(datasetId);
                ulong[] dims = new ulong[1];
                ulong[] maxdims = null; // You can set this to null if you don't need the maximum dimensions
                H5S.get_simple_extent_dims(spaceId, dims, maxdims);
                float[] features = new float[dims[0]];
                GCHandle handle = GCHandle.Alloc(features, GCHandleType.Pinned);
                H5D.read(datasetId, H5T.NATIVE_FLOAT, H5S.ALL, H5S.ALL, H5P.DEFAULT, handle.AddrOfPinnedObject());
                handle.Free();
                H5D.close(datasetId);
                H5S.close(spaceId);

                // Close the group
                H5G.close(groupId);

                // Create an AudioData object and add it to the list
                audioDataList.Add(new AudioData
                {
                    Path = trackPath,
                    Label = key, // Keep the label as a string
                    Features = features
                });
            }

            H5F.close(fileId);

            return audioDataList;
        }

        private IEnumerable<string> GetTrackPaths(long fileId)
        {
            var trackPaths = new List<string>();

            // Define a callback function to be called for each group
            H5L.iterate_t callback = (long group, IntPtr name, ref H5L.info_t info, IntPtr op_data) =>
            {
                // Convert the name pointer to a string
                string trackPath = Marshal.PtrToStringAnsi(name);
                trackPaths.Add(trackPath);
                return 0; // Continue iteration
            };

            // Initialize the index for iteration
            ulong idx = 0;

            // Iterate over all groups in the root of the file
            H5L.iterate(fileId, H5.index_t.NAME, H5.iter_order_t.NATIVE, ref idx, callback, IntPtr.Zero);

            return trackPaths;
        }

        private ITransformer CreateAveragedModel(float[] averagedPredictions, DataViewSchema schema)
        {
            // Create a list of Prediction objects from the averaged predictions
            var predictionList = averagedPredictions.Select(score => new Prediction { Score = score }).ToList();

            var data = _mlContext.Data.LoadFromEnumerable(predictionList);

            var pipeline = _mlContext.Transforms.CopyColumns("PredictedLabel", "Score");
            var model = pipeline.Fit(data);

            return model;
        }

        public class Prediction
        {
            public float Score { get; set; }
        }
       

    

        public List<AudioData> LoadFromHDF5(string fileName)
        {
            var audioDataList = new List<AudioData>();

            // Open the HDF5 file
            long fileId = H5F.open(fileName, H5F.ACC_RDONLY);
            if (fileId < 0)
            {
                throw new Exception("Failed to open HDF5 file.");
            }

            // Iterate over each group (track) in the file
            foreach (var trackPath in GetTrackPaths(fileId))
            {
                // Open the group
                long groupId = H5G.open(fileId, trackPath);
                if (groupId < 0)
                {
                    throw new Exception($"Failed to open group for track {trackPath}.");
                }

                // Read the key attribute
                long keyAttrId = H5A.open(groupId, "Key");
                long keyAttrType = H5A.get_type(keyAttrId);
                long keyAttrSpace = H5A.get_space(keyAttrId);
                int keyLength = (int)H5T.get_size(keyAttrType);
                byte[] keyBytes = new byte[keyLength];
                GCHandle keyHandle = GCHandle.Alloc(keyBytes, GCHandleType.Pinned);
                H5A.read(keyAttrId, keyAttrType, keyHandle.AddrOfPinnedObject());
                keyHandle.Free();
                string key = System.Text.Encoding.ASCII.GetString(keyBytes);
                H5A.close(keyAttrId);
                H5S.close(keyAttrSpace);
                H5T.close(keyAttrType);

                // Read the features dataset
                long datasetId = H5D.open(groupId, "Features");
                long spaceId = H5D.get_space(datasetId);
                ulong[] dims = new ulong[1];
                ulong[] maxdims = null; // You can set this to null if you don't need the maximum dimensions
                H5S.get_simple_extent_dims(spaceId, dims, maxdims);
                float[] features = new float[dims[0]];
                GCHandle handle = GCHandle.Alloc(features, GCHandleType.Pinned);
                H5D.read(datasetId, H5T.NATIVE_FLOAT, H5S.ALL, H5S.ALL, H5P.DEFAULT, handle.AddrOfPinnedObject());
                handle.Free();
                H5D.close(datasetId);
                H5S.close(spaceId);

                // Ensure the feature length is correct
                if (features.Length != 44) // Replace 44 with the expected feature length
                {
                    throw new Exception($"Unexpected feature length: {features.Length}. Expected: 44.");
                }

                // Close the group
                H5G.close(groupId);

                // Create an AudioData object and add it to the list
                audioDataList.Add(new AudioData
                {
                    Path = trackPath,
                    Label = key, // Use the key as the label
                    Features = features
                });
            }

            H5F.close(fileId);

            return audioDataList;
        }

        private void CloseViewWithOk(object sender, EventArgs e)
        {
            ((ModelTrainerView)this.view).DialogResult = DialogResult.OK;
            ((ModelTrainerView)this.view).Close();
        }

        bool enableExtractChromaFeaturesLog = false;
        bool enableExtractMFCCsLog = false;
        bool enableExtractSpectralContrastLog = false;
        bool enableExtractHPCPLog = false;
        bool enableExtractSpectralCentroidLog = false;
        bool enableExtractFeaturesLog = true;

        /* private float[] ExtractFeatures(string filePath)
         {
             using (var reader = new AudioFileReader(filePath))
             {
                 var sampleProvider = reader.ToSampleProvider();
                 float[] buffer = new float[reader.WaveFormat.SampleRate * 2];
                 int samplesRead;
                 List<float> features = new List<float>();
                 int totalSamples = (int)Math.Ceiling((double)reader.Length / (buffer.Length * 2 * 2)); // Adjust for bytes per sample and channels
                 int currentSample = 0;

                 if (enableExtractFeaturesLog)
                     Console.WriteLine($"Total samples: {totalSamples}");

                 while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                 {
                     // Perform FFT
                     Complex[] fftBuffer = new Complex[buffer.Length];
                     for (int i = 0; i < buffer.Length; i++)
                     {
                         fftBuffer[i] = new Complex { X = buffer[i], Y = 0 };
                     }

                     FastFourierTransform.FFT(true, (int)Math.Log(buffer.Length, 2.0), fftBuffer);

                     if (this.IsChromaFeaturesEnabled)
                     {
                         // Extract chroma features from FFT result
                         float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                         features.AddRange(chromaFeatures);

                         if (this.IsTonnetzFeaturesEnabled)
                         {
                             // Extract Tonnetz features
                             float[] tonnetzFeatures = ExtractTonnetzFeatures(chromaFeatures);
                             features.AddRange(tonnetzFeatures);
                         }
                     }

                     if(this.IsMFCCsEnabled)
                     {
                         // Extract MFCCs
                         float[] mfccFeatures = ExtractMFCCs(buffer, reader.WaveFormat.SampleRate);
                         features.AddRange(mfccFeatures);
                     }

                     if (this.IsSpectralContrastEnabled)
                     {
                         // Extract spectral contrast
                         float[] spectralContrastFeatures = ExtractSpectralContrast(fftBuffer, reader.WaveFormat.SampleRate);
                         features.AddRange(spectralContrastFeatures);
                     }

                     if(this.IsHPCPEnabled)
                     {
                         // Extract HPCP
                         float[] hpcpFeatures = ExtractHPCP(fftBuffer, reader.WaveFormat.SampleRate);
                         features.AddRange(hpcpFeatures);
                     }

                     if (this.IsHPSEnabled)
                     {
                         // Harmonic/Percussive Source Separation
                         var (harmonic, percussive) = HarmonicPercussiveSeparation(buffer, reader.WaveFormat.SampleRate);
                         features.AddRange(harmonic);
                         features.AddRange(percussive);
                     }

                     if(this.IsSpectralCentroidEnabled)
                     {
                         // Extract spectral centroid
                         float spectralCentroid = ExtractSpectralCentroid(fftBuffer, reader.WaveFormat.SampleRate);
                         features.Add(spectralCentroid);
                     }

                     currentSample++;
                     int progressValue = currentSample * 100 / totalSamples;

                     if (enableExtractFeaturesLog)
                         Console.WriteLine($"Processing {filePath}: {progressValue}% complete (Sample {currentSample} of {totalSamples})");

                 }

                 return features.ToArray();
             }
         }*/


        
      

        

        public enum LogState
        {
            Extraction,
            ParallelProcess,
            FileReading,
            Finish,
            Canceled
        }
        //TODO
       /* private void AddTrainingDataEvent(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "HDF5 files (*.hdf5)|*.hdf5";
            // ofd.FilterIndex = this.settingDao.GetIntegerSetting(Settings.LastOpenTrainingDataFilterIndex.ToString());
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                /* TrainingData trainingData = new TrainingData()
                 {
                     Id = this.trackDao.GetNextId(TableName.TrainingData.ToString()),
                     FilePath = ofd.FileName,
                     TagId = this.CurrentTag.Id,
                     Name = this.CurrentPlaylist?.Name,
                     CreateDate = DateTime.Now,
                     SampleCount = this.trackList.Count,
                     Balance = this.balance
                 };

                //this.trackDao.CreateTrainingData(trainingData);
            }
             this.settingDao.SetIntegerSetting(Settings.LastOpenTrainingDataFilterIndex.ToString(), ofd.FilterIndex);

            this.InitializeTrainingDataListContent();
        }*/

        private string TransformKeycodeToKey(string keycode)
        {
            var keyMap = new Dictionary<string, string>
            {
            { "01A", "A Minor" }, { "02A", "E Minor" }, { "03A", "B Minor" }, { "04A", "F# Minor" },
            { "05A", "C# Minor" }, { "06A", "G# Minor" }, { "07A", "D# Minor" }, { "08A", "A# Minor" },
            { "09A", "F Minor" }, { "10A", "C Minor" }, { "11A", "G Minor" }, { "12A", "D Minor" },
            { "01B", "C Major" }, { "02B", "G Major" }, { "03B", "D Major" }, { "04B", "A Major" },
            { "05B", "E Major" }, { "06B", "B Major" }, { "07B", "F# Major" }, { "08B", "C# Major" },
            { "09B", "G# Major" }, { "10B", "D# Major" }, { "11B", "A# Major" }, { "12B", "F Major" }
            };
            return keyMap.ContainsKey(keycode) ? keyMap[keycode] : "Unknown";
        }

        /*
        private List<float[]> ExtractFeaturesAtIntervals(string filePath, int intervalSeconds, CancellationToken cancellationToken)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                int sampleRate = reader.WaveFormat.SampleRate;
                int bufferSize = sampleRate * intervalSeconds; // Buffer size for the interval
                float[] buffer = new float[bufferSize];
                int samplesRead;
                List<float[]> featuresList = new List<float[]>();

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    // Perform FFT and extract features
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i] = new Complex(buffer[i], 0);
                    }
                    Fourier.Forward(fftBuffer, FourierOptions.Matlab);

                    // Extract chroma features
                    float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                    float pitch = ExtractPitch(buffer, sampleRate);

                    // Aggregate features
                    float meanChroma = chromaFeatures.Average();
                    float varianceChroma = chromaFeatures.Select(f => (f - meanChroma) * (f - meanChroma)).Average();

                    // Combine features
                    List<float> aggregatedFeatures = new List<float> { meanChroma, varianceChroma, pitch };
                    featuresList.Add(aggregatedFeatures.ToArray());
                }

                return featuresList;
            }
        }


        private List<float[]> ExtractFeaturesAtIntervals(string filePath, int intervalSeconds, CancellationToken cancellationToken)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                int sampleRate = reader.WaveFormat.SampleRate;
                int bufferSize = sampleRate * intervalSeconds; // Buffer size for the interval
                float[] buffer = new float[bufferSize];
                int samplesRead;
                List<float[]> featuresList = new List<float[]>();

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    // Perform FFT and extract features
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i] = new Complex(buffer[i], 0);
                    }
                    Fourier.Forward(fftBuffer, FourierOptions.Matlab);

                    // Extract chroma features
                    float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                    float pitch = ExtractPitch(buffer, sampleRate);

                    // Combine features
                    List<float> combinedFeatures = new List<float>(chromaFeatures) { pitch };
                    featuresList.Add(combinedFeatures.ToArray());
                }

                // Convert featuresList to a 2D array for PCA
                double[,] featureArray = new double[featuresList.Count, featuresList[0].Length];
                for (int i = 0; i < featuresList.Count; i++)
                {
                    for (int j = 0; j < featuresList[i].Length; j++)
                    {
                        featureArray[i, j] = featuresList[i][j];
                    }
                }

                // Apply PCA to reduce dimensionality
                double[,] reducedFeatures = ApplyPCA(featureArray, numComponents: 50); // Set desired number of components

                // Convert reduced features back to List<float[]>
                List<float[]> reducedFeaturesList = new List<float[]>();
                for (int i = 0; i < reducedFeatures.GetLength(0); i++)
                {
                    float[] row = new float[reducedFeatures.GetLength(1)];
                    for (int j = 0; j < reducedFeatures.GetLength(1); j++)
                    {
                        row[j] = (float)reducedFeatures[i, j];
                    }
                    reducedFeaturesList.Add(row);
                }

                return reducedFeaturesList;
            }
        }
        */
        /*
        private double[,] ApplyPCA(double[,] data, int numComponents)
        {
            var matrix = DenseMatrix.OfArray(data);
            var pca = new PrincipalComponentAnalysis(matrix, true);
            pca.Compute();

            var components = pca.Transform(matrix, numComponents);
            return components.ToArray();
        }

        private List<float[]> ExtractAndAggregateFeatures(string filePath, int intervalSeconds, CancellationToken cancellationToken)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                int sampleRate = reader.WaveFormat.SampleRate;
                int bufferSize = sampleRate * intervalSeconds; // Buffer size for the interval
                float[] buffer = new float[bufferSize];
                int samplesRead;
                List<float[]> featuresList = new List<float[]>();

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    // Perform FFT and extract features
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i] = new Complex(buffer[i], 0);
                    }
                    Fourier.Forward(fftBuffer, FourierOptions.Matlab);

                    // Extract chroma features
                    float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                    float pitch = ExtractPitch(buffer, sampleRate);

                    // Aggregate features
                    float meanChroma = chromaFeatures.Average();
                    float varianceChroma = chromaFeatures.Select(f => (f - meanChroma) * (f - meanChroma)).Average();

                    // Combine features
                    List<float> aggregatedFeatures = new List<float> { meanChroma, varianceChroma, pitch };
                    featuresList.Add(aggregatedFeatures.ToArray());
                }

                return featuresList;
            }
        }
        */
        private void TrainModel(List<float[]> featuresList, List<string> labels)
        {
            var mlContext = new MLContext();
            var data = featuresList.Select((features, index) => new AudioData
            {
                Features = features,
                Label = labels[index]
            });

            var dataView = mlContext.Data.LoadFromEnumerable(data);

            var pipeline = mlContext.Transforms.Conversion.MapValueToKey(nameof(AudioData.Label))
            .Append(mlContext.Transforms.Concatenate("Features", nameof(AudioData.Features)))
            .Append(mlContext.Transforms.NormalizeMinMax("Features"))
            .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(new SdcaMaximumEntropyMulticlassTrainer.Options
            {
                LabelColumnName = nameof(AudioData.Label),
                FeatureColumnName = "Features"
            }))
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var model = pipeline.Fit(dataView);
            // Save or use the model as needed
        }

        /*
        private List<float[]> ExtractAndAggregateFeatures(string filePath, int intervalSeconds, CancellationToken cancellationToken)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                int sampleRate = reader.WaveFormat.SampleRate;
                int bufferSize = sampleRate * intervalSeconds; // Buffer size for the interval
                float[] buffer = new float[bufferSize];
                int samplesRead;
                List<float[]> featuresList = new List<float[]>();

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    // Perform FFT and extract features
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i] = new Complex(buffer[i], 0);
                    }
                    Fourier.Forward(fftBuffer, FourierOptions.Matlab);

                    // Extract chroma features
                    float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                    float pitch = ExtractPitch(buffer, sampleRate);

                    // Aggregate features
                    float meanChroma = chromaFeatures.Average();
                    float varianceChroma = chromaFeatures.Select(f => (f - meanChroma) * (f - meanChroma)).Average();

                    // Combine features
                    List<float> aggregatedFeatures = new List<float> { meanChroma, varianceChroma, pitch };
                    featuresList.Add(aggregatedFeatures.ToArray());
                }

                // Convert featuresList to a 2D array for PCA
                double[,] featureArray = new double[featuresList.Count, featuresList[0].Length];
                for (int i = 0; i < featuresList.Count; i++)
                {
                    for (int j = 0; j < featuresList[i].Length; j++)
                    {
                        featureArray[i, j] = featuresList[i][j];
                    }
                }

                // Apply PCA to reduce dimensionality
                double[,] reducedFeatures = ApplyPCA(featureArray, numComponents: 20); // Set desired number of components

                // Convert reduced features back to List<float[]>
                List<float[]> reducedFeaturesList = new List<float[]>();
                for (int i = 0; i < reducedFeatures.GetLength(0); i++)
                {
                    float[] row = new float[reducedFeatures.GetLength(1)];
                    for (int j = 0; j < reducedFeatures.GetLength(1); j++)
                    {
                        row[j] = (float)reducedFeatures[i, j];
                    }
                    reducedFeaturesList.Add(row);
                }

                return reducedFeaturesList;
            }
        }

        private double[,] ApplyPCA(double[,] data, int numComponents)
        {
            var matrix = DenseMatrix.OfArray(data);
            var pca = new PrincipalComponentAnalysis(matrix, true);
            pca.Compute();

            var components = pca.Transform(matrix, numComponents);
            return components.ToArray();
        }


        private float[] ExtractChromaFeatures(Complex[] fftBuffer)
        {
            // Implement chroma feature extraction logic here
            // This is a placeholder example
            return new float[12]; // Example: 12 chroma bins
        }

        private float ExtractPitch(float[] buffer, int sampleRate)
        {
            // Implement pitch extraction logic here
            // This is a placeholder example
            return 0.0f; // Example: pitch value
        }
        */

    }
}
