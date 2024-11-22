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
        event EventHandler<Messenger> SelectTag;
        event EventHandler<Messenger> SelectPlaylist;
        event EventHandler<Messenger> SelectTemplate;
        event EventHandler CloseViewWithOk;
        event EventHandler<Messenger> IsChromaFeaturesEnabled;
        event EventHandler<Messenger> IsMFCCsEnabled;
        event EventHandler<Messenger> IsSpectralContrastEnabled;
        event EventHandler<Messenger> IsHPCPEnabled;
        event EventHandler<Messenger> IsHPSEnabled;
        event EventHandler<Messenger> IsSpectralCentroidEnabled;
        event EventHandler<Messenger> IsTonnetzFeaturesEnabled;
         event EventHandler<Messenger> IsSpectralBandwidthEnabled;
         event EventHandler<Messenger> IsZCREnabled;
         event EventHandler<Messenger> IsRMSEnabled;
         event EventHandler<Messenger> IsPitchEnabled;
         event EventHandler<Messenger> BatchProcessChanged;
         event EventHandler<Messenger> LoadTrainingDataEvent;
         event EventHandler<Messenger> DeleteTrainingDataEvent;
         event EventHandler<Messenger> SetIsTracklistDetailsDisplayedEvent;

         event EventHandler<Messenger> AnalyseTrackEvent;
         event EventHandler<Messenger> SetCurrentFeatureTypeEvent;

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
