using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MitoPlayer_2024.Presenters.ModelTrainerPresenter;

namespace MitoPlayer_2024.Views
{
    public interface IModelTrainerView
    {
        event EventHandler<ListEventArgs> SelectTag;
        event EventHandler<ListEventArgs> SelectPlaylist;
        event EventHandler<ListEventArgs> SelectTemplate;
        event EventHandler CloseViewWithOk;
        event EventHandler<ListEventArgs> IsChromaFeaturesEnabled;
        event EventHandler<ListEventArgs> IsMFCCsEnabled;
        event EventHandler<ListEventArgs> IsSpectralContrastEnabled;
        event EventHandler<ListEventArgs> IsHPCPEnabled;
        event EventHandler<ListEventArgs> IsHPSEnabled;
        event EventHandler<ListEventArgs> IsSpectralCentroidEnabled;
        event EventHandler<ListEventArgs> IsTonnetzFeaturesEnabled;
         event EventHandler<ListEventArgs> IsSpectralBandwidthEnabled;
         event EventHandler<ListEventArgs> IsZCREnabled;
         event EventHandler<ListEventArgs> IsRMSEnabled;
         event EventHandler<ListEventArgs> IsPitchEnabled;
         event EventHandler<ListEventArgs> BatchProcessChanged;
         event EventHandler<ListEventArgs> LoadTrainingDataEvent;
         event EventHandler<ListEventArgs> DeleteTrainingDataEvent;
         event EventHandler<ListEventArgs> SetIsTracklistDetailsDisplayedEvent;
        event EventHandler GenerateTrainingData;
        event EventHandler CancelGenerationEvent;
        event EventHandler TrainModelEvent;
        event EventHandler AddTrainingDataEvent;
        event EventHandler CalculateDataSetQualityEvent;

        void InitializeTagsAndTagValues(List<Tag> tagList, Dictionary<String, Dictionary<String, Color>> tagValueDictionary);
        void InitializeFeatureSettings(bool extractChromaFeatures, 
            bool harmonicPercussiveSeparation, 
            bool extractMFCCs, 
            bool extractHPCP, 
            bool extractSpectralContrast, 
            bool extractSpectralCentroid, 
            bool extractSpectralBandwidth, 
            bool extractTonnetzFeatures,
            bool extractZeroCrossingRate,
            bool extractPitch, 
            bool extractRmsEnergy);
        void InitializeView(List<Models.Tag> tagList, List<Models.Playlist> playistList, List<Models.TrainingData> templateList, int batchSize, bool isTracklistDetailsDisplayed);
        void SetInputTrackListBindingSource(BindingSource trackList);
        void SetTraningDataListBindingSource(BindingSource trainingDataList);
        void UpdateProgressOnView(MessageTest message);
        void ChangeGeneratingStatus(bool isGenerating);
        void ChangeTrainingStatus(bool isTraining);


    }
}
