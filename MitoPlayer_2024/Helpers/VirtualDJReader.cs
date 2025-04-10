using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mysqlx.Notice.Warning.Types;
using System.Xml.Linq;
using MitoPlayer_2024.Model;
using System.Xml;
using System.Runtime.CompilerServices;
using MitoPlayer_2024.Dao;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using MitoPlayer_2024.Views;
using System.Management.Instrumentation;
using static Mysqlx.Expect.Open.Types.Condition.Types;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading;
using Accord.Math.Distances;

namespace MitoPlayer_2024.Helpers
{
    public class VDJTrack
    {
        public String Path { get; set; }
        public String KeyCode { get; set; }
        public Decimal Bpm { get; set; }
    }

    public sealed class VirtualDJReader
    {
        private static VirtualDJReader instance = null;
        private VirtualDJReader()
        {
            //this.InitializeAvailableDatabaseFiles();
        }
        public static VirtualDJReader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VirtualDJReader();
                }
                return instance;
            }
        }

        public List<String> VdjDatabasePathList { get; set; }



        
        public String[] KeyCodesArray { get; set; }
        public String[] KeysArray { get; set; }
        public String[] KeysAlterArray { get; set; }
        public List<VDJTrack> VDJTracklist { get; set; }

        public ResultOrError ReadKeysAndBpmsFromVirtualDJDatabase(ref List<Model.Track> trackList, ITrackDao trackDao, List<TagValue> keyTagValueList, List<TagValue> bpmTagValueList)
        {
            ResultOrError result = new ResultOrError();

            String currentDrive = String.Empty;
            List<String> driveList = new List<String>();
            List<String> validDriveList = new List<String>();
            List<Model.Track> filteredTrackList = trackList.FindAll(x => x.IsNew);

            try
            {
                for (int i = 0; i < filteredTrackList.Count; i++)
                {
                    filteredTrackList[i].IsNew = false;
                    if (String.IsNullOrEmpty(currentDrive))
                    {
                        currentDrive = filteredTrackList[i].Path.Substring(0, 1);
                        driveList.Add(currentDrive);
                    }
                    else
                    {
                        if (currentDrive != filteredTrackList[i].Path.Substring(0, 1))
                        {
                            currentDrive = filteredTrackList[i].Path.Substring(0, 1);
                            driveList.Add(currentDrive);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            List<List<String>> filePathsPerDrive = new List<List<String>>();

            if (result.Success)
            {
                try
                {
                    foreach (String drive in driveList)
                    {
                        bool driveIsValid = true;
                        List<String> filePaths = new List<String>();
                        String vdjDatabaseFilePath = drive + ":\\VirtualDJ\\database.xml";
                        if (File.Exists(vdjDatabaseFilePath))
                        {
                            validDriveList.Add(drive);
                        }
                        else
                        {
                            vdjDatabaseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VirtualDJ\\database.xml";
                            if (File.Exists(vdjDatabaseFilePath))
                            {
                                validDriveList.Add(drive);
                            }
                            else
                            {
                                driveIsValid = false;
                            }
                        }

                        if (driveIsValid)
                        {
                            using (StreamReader sr = new StreamReader(vdjDatabaseFilePath))
                            {
                                String content = sr.ReadToEnd();

                                for (int i = 0; i < filteredTrackList.Count; i++)
                                {
                                    if (content.Contains(filteredTrackList[i].Path
                                        .Replace("&", "&amp;")
                                        .Replace("'", "&apos;")))
                                    {
                                        filePaths.Add(filteredTrackList[i].Path);
                                    }
                                }
                                sr.Close();
                            }

                            filePathsPerDrive.Add(filePaths);
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }

            }

            ConcurrentQueue<String> filePathList = new ConcurrentQueue<String>();
            ConcurrentQueue<String> keyList = new ConcurrentQueue<String>();
            ConcurrentQueue<String> bpmList = new ConcurrentQueue<String>();

            if (result.Success)
            {
                try
                {
                    String keyCodes = System.Configuration.ConfigurationManager.AppSettings[Settings.KeyCodes.ToString()];
                    String keys = System.Configuration.ConfigurationManager.AppSettings[Settings.Keys.ToString()];
                    String keysAlter = System.Configuration.ConfigurationManager.AppSettings[Settings.KeysAlter.ToString()];
                    this.KeyCodesArray = Array.ConvertAll(keyCodes.Split(','), s => s);
                    this.KeysArray = Array.ConvertAll(keys.Split(','), s => s);
                    this.KeysAlterArray = Array.ConvertAll(keysAlter.Split(','), s => s);

                    String vdjDatabaseFilePath = String.Empty;

                    Parallel.ForEach(validDriveList, (drive, state, index) =>
                    {

                        if (drive == "C")
                        {
                            vdjDatabaseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VirtualDJ\\database.xml";
                        }
                        else
                        {
                            vdjDatabaseFilePath = drive + ":\\VirtualDJ\\database.xml";
                        }


                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(vdjDatabaseFilePath);
                        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("Song");

                        TrackTagValue ttv = new TrackTagValue();

                        bool isEnabledKeyAndBpmReading = false;

                        foreach (XmlNode node in nodeList)
                        {
                            if (node.Attributes["FilePath"].Value != null)
                            {
                                foreach (String filePath in filePathsPerDrive[(int)index])
                                {
                                    if (node.Attributes["FilePath"].Value.Contains(filePath))
                                    {
                                        filePathList.Enqueue(filePath);
                                        isEnabledKeyAndBpmReading = true;
                                    }
                                }
                            }
                            if (isEnabledKeyAndBpmReading)
                            {
                                XmlNodeList list = node.SelectNodes("Scan");
                                if (list != null && list.Count > 0)
                                {
                                    String key = String.Empty;
                                    String bpm = String.Empty;

                                    if (list[0].Attributes["Key"] != null)
                                    {
                                        key = list[0].Attributes["Key"].Value;
                                        key = this.KeyToKeyCode(key);
                                    }

                                    if (list[0].Attributes["Bpm"] != null)
                                    {
                                        bpm = list[0].Attributes["Bpm"].Value;
                                        Decimal bpmConverted = Convert.ToDecimal(bpm.Replace(".", ","));
                                        if (bpmConverted > 0)
                                        {
                                            bpm = (60 / bpmConverted).ToString("N1");
                                        }
                                    }

                                    keyList.Enqueue(key);
                                    bpmList.Enqueue(bpm);

                                    isEnabledKeyAndBpmReading = false;
                                }
                            }

                        }


                    });
                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }
            }

            if (result.Success)
            {
                try
                {
                    List<string> finalFilePathList = filePathList.ToList();
                    List<string> finalKeyList = keyList.ToList();
                    List<string> finalBpmList = bpmList.ToList();

                    for (int i = 0; i < filePathList.Count; i++)
                    {
                        Model.Track currentTrack = trackList.Find(x => x.Path == finalFilePathList[i]);
                        int index = trackList.IndexOf(currentTrack);
                        if (currentTrack != null)
                        {
                            foreach (TrackTagValue ttv in currentTrack.TrackTagValues)
                            {
                                if (ttv.TagName == "Key" && keyTagValueList != null && keyTagValueList.Count > 0)
                                {
                                    TagValue keyTagValue = keyTagValueList.Find(x => x.Name == finalKeyList[i]);
                                    if (keyTagValue != null)
                                    {
                                        ttv.TagValueId = keyTagValue.Id;
                                        ttv.TagValueName = keyTagValue.Name;
                                        ttv.HasMultipleValues = false;
                                    }
                                    trackDao.UpdateTrackTagValue(ttv);
                                }
                                if (ttv.TagName == "Bpm" && bpmTagValueList != null && bpmTagValueList.Count > 0)
                                {
                                    TagValue bpmTagValue = bpmTagValueList[0];
                                    if (bpmTagValue != null)
                                    {
                                        ttv.TagValueId = bpmTagValue.Id;
                                        ttv.TagValueName = bpmTagValue.Name;
                                        ttv.Value = finalBpmList[i];
                                        ttv.HasMultipleValues = true;
                                    }
                                    trackDao.UpdateTrackTagValue(ttv);
                                }
                            }
                        }
                        trackList[index] = currentTrack;
                    }

                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }

            }

            return result;

        }
        public ResultOrError ReadKeysFromVirtualDJDatabase(ref List<Model.Track> trackList, ITrackDao trackDao, List<TagValue> keyTagValueList)
        {
            ResultOrError result = new ResultOrError();

            String currentDrive = String.Empty;
            List<String> driveList = new List<String>();
            List<String> validDriveList = new List<String>();
            List<Model.Track> filteredTrackList = trackList.FindAll(x => x.IsNew);

            try
            {
                for (int i = 0; i < filteredTrackList.Count; i++)
                {
                    filteredTrackList[i].IsNew = false;
                    if (String.IsNullOrEmpty(currentDrive))
                    {
                        currentDrive = filteredTrackList[i].Path.Substring(0, 1);
                        driveList.Add(currentDrive);
                    }
                    else
                    {
                        if (currentDrive != filteredTrackList[i].Path.Substring(0, 1))
                        {
                            currentDrive = filteredTrackList[i].Path.Substring(0, 1);
                            driveList.Add(currentDrive);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            List<List<String>> filePathsPerDrive = new List<List<String>>();

            if (result.Success)
            {
                try
                {
                    foreach (String drive in driveList)
                    {
                        bool driveIsValid = true;
                        List<String> filePaths = new List<String>();
                        String vdjDatabaseFilePath = drive + ":\\VirtualDJ\\database.xml";
                        if (File.Exists(vdjDatabaseFilePath))
                        {
                            validDriveList.Add(drive);
                        }
                        else
                        {
                            vdjDatabaseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VirtualDJ\\database.xml";
                            if (File.Exists(vdjDatabaseFilePath))
                            {
                                validDriveList.Add(drive);
                            }
                            else
                            {
                                driveIsValid = false;
                            }
                        }

                        if (driveIsValid)
                        {
                            using (StreamReader sr = new StreamReader(vdjDatabaseFilePath))
                            {
                                String content = sr.ReadToEnd();

                                for (int i = 0; i < filteredTrackList.Count; i++)
                                {
                                    if (content.Contains(filteredTrackList[i].Path
                                        .Replace("&", "&amp;")
                                        .Replace("'", "&apos;")))
                                    {
                                        filePaths.Add(filteredTrackList[i].Path);
                                    }
                                }
                                sr.Close();
                            }

                            filePathsPerDrive.Add(filePaths);
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }

            }

            ConcurrentQueue<String> filePathList = new ConcurrentQueue<String>();
            ConcurrentQueue<String> keyList = new ConcurrentQueue<String>();

            if (result.Success)
            {
                try
                {
                    String keyCodes = System.Configuration.ConfigurationManager.AppSettings[Settings.KeyCodes.ToString()];
                    String keys = System.Configuration.ConfigurationManager.AppSettings[Settings.Keys.ToString()];
                    String keysAlter = System.Configuration.ConfigurationManager.AppSettings[Settings.KeysAlter.ToString()];
                    this.KeyCodesArray = Array.ConvertAll(keyCodes.Split(','), s => s);
                    this.KeysArray = Array.ConvertAll(keys.Split(','), s => s);
                    this.KeysAlterArray = Array.ConvertAll(keysAlter.Split(','), s => s);

                    String vdjDatabaseFilePath = String.Empty;

                    Parallel.ForEach(validDriveList, (drive, state, index) =>
                    {

                        if (drive == "C")
                        {
                            vdjDatabaseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VirtualDJ\\database.xml";
                        }
                        else
                        {
                            vdjDatabaseFilePath = drive + ":\\VirtualDJ\\database.xml";
                        }


                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(vdjDatabaseFilePath);
                        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("Song");

                        TrackTagValue ttv = new TrackTagValue();

                        bool isEnabledKeyAndBpmReading = false;

                        foreach (XmlNode node in nodeList)
                        {
                            if (node.Attributes["FilePath"].Value != null)
                            {
                                foreach (String filePath in filePathsPerDrive[(int)index])
                                {
                                    if (node.Attributes["FilePath"].Value.Contains(filePath))
                                    {
                                        filePathList.Enqueue(filePath);
                                        isEnabledKeyAndBpmReading = true;
                                    }
                                }
                            }
                            if (isEnabledKeyAndBpmReading)
                            {
                                XmlNodeList list = node.SelectNodes("Scan");
                                if (list != null && list.Count > 0)
                                {
                                    String key = String.Empty;

                                    if (list[0].Attributes["Key"] != null)
                                    {
                                        key = list[0].Attributes["Key"].Value;
                                        key = this.KeyToKeyCode(key);
                                    }

                                    keyList.Enqueue(key);

                                    isEnabledKeyAndBpmReading = false;
                                }
                            }

                        }


                    });
                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }
            }

            if (result.Success)
            {
                try
                {
                    List<string> finalFilePathList = filePathList.ToList();
                    List<string> finalKeyList = keyList.ToList();

                    for (int i = 0; i < filePathList.Count; i++)
                    {
                        Model.Track currentTrack = trackList.Find(x => x.Path == finalFilePathList[i]);
                        int index = trackList.IndexOf(currentTrack);
                        if (currentTrack != null)
                        {
                            foreach (TrackTagValue ttv in currentTrack.TrackTagValues)
                            {
                                if (ttv.TagName == "Key" && keyTagValueList != null && keyTagValueList.Count > 0)
                                {
                                    TagValue keyTagValue = keyTagValueList.Find(x => x.Name == finalKeyList[i]);
                                    if (keyTagValue != null)
                                    {
                                        ttv.TagValueId = keyTagValue.Id;
                                        ttv.TagValueName = keyTagValue.Name;
                                        ttv.HasMultipleValues = false;
                                    }
                                    trackDao.UpdateTrackTagValue(ttv);
                                }
                            }
                        }
                        trackList[index] = currentTrack;
                    }

                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }

            }

            return result;

        }
        public ResultOrError ReadBpmsFromVirtualDJDatabase(ref List<Model.Track> trackList, ITrackDao trackDao, List<TagValue> bpmTagValueList)
        {
            ResultOrError result = new ResultOrError();

            String currentDrive = String.Empty;
            List<String> driveList = new List<String>();
            List<String> validDriveList = new List<String>();
            List<Model.Track> filteredTrackList = trackList.FindAll(x => x.IsNew);

            try
            {
                for (int i = 0; i < filteredTrackList.Count; i++)
                {
                    filteredTrackList[i].IsNew = false;
                    if (String.IsNullOrEmpty(currentDrive))
                    {
                        currentDrive = filteredTrackList[i].Path.Substring(0, 1);
                        driveList.Add(currentDrive);
                    }
                    else
                    {
                        if (currentDrive != filteredTrackList[i].Path.Substring(0, 1))
                        {
                            currentDrive = filteredTrackList[i].Path.Substring(0, 1);
                            driveList.Add(currentDrive);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            List<List<String>> filePathsPerDrive = new List<List<String>>();

            if (result.Success)
            {
                try
                {
                    foreach (String drive in driveList)
                    {
                        bool driveIsValid = true;
                        List<String> filePaths = new List<String>();
                        String vdjDatabaseFilePath = drive + ":\\VirtualDJ\\database.xml";
                        if (File.Exists(vdjDatabaseFilePath))
                        {
                            validDriveList.Add(drive);
                        }
                        else
                        {
                            vdjDatabaseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VirtualDJ\\database.xml";
                            if (File.Exists(vdjDatabaseFilePath))
                            {
                                validDriveList.Add(drive);
                            }
                            else
                            {
                                driveIsValid = false;
                            }
                        }

                        if (driveIsValid)
                        {
                            using (StreamReader sr = new StreamReader(vdjDatabaseFilePath))
                            {
                                String content = sr.ReadToEnd();

                                for (int i = 0; i < filteredTrackList.Count; i++)
                                {
                                    if (content.Contains(filteredTrackList[i].Path
                                        .Replace("&", "&amp;")
                                        .Replace("'", "&apos;")))
                                    {
                                        filePaths.Add(filteredTrackList[i].Path);
                                    }
                                }
                                sr.Close();
                            }

                            filePathsPerDrive.Add(filePaths);
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }

            }

            ConcurrentQueue<String> filePathList = new ConcurrentQueue<String>();
            ConcurrentQueue<String> bpmList = new ConcurrentQueue<String>();

            if (result.Success)
            {
                try
                {
                    String vdjDatabaseFilePath = String.Empty;
                    String drive = String.Empty;
                    int index = 0;
                    // Parallel.ForEach(validDriveList, (drive, state, index) =>
                    for (int i = 0;i < validDriveList.Count; i++)
                    {
                        drive = validDriveList[i];
                        index = i;

                        if (drive == "C")
                        {
                            vdjDatabaseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VirtualDJ\\database.xml";
                        }
                        else
                        {
                            vdjDatabaseFilePath = drive + ":\\VirtualDJ\\database.xml";
                        }


                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(vdjDatabaseFilePath);
                        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("Song");

                        TrackTagValue ttv = new TrackTagValue();

                        bool isEnabledKeyAndBpmReading = false;

                        foreach (XmlNode node in nodeList)
                        {
                            if (node.Attributes["FilePath"].Value != null)
                            {
                                foreach (String filePath in filePathsPerDrive[(int)index])
                                {
                                    if (node.Attributes["FilePath"].Value.Contains(filePath))
                                    {
                                        filePathList.Enqueue(filePath);
                                        isEnabledKeyAndBpmReading = true;
                                    }
                                }
                            }
                            if (isEnabledKeyAndBpmReading)
                            {
                                XmlNodeList list = node.SelectNodes("Scan");
                                if (list != null && list.Count > 0)
                                {
                                    String bpm = String.Empty;

                                    if (list[0].Attributes["Bpm"] != null)
                                    {
                                        bpm = list[0].Attributes["Bpm"].Value;
                                        Decimal bpmConverted = Convert.ToDecimal(bpm.Replace(".", ","));
                                        if (bpmConverted > 0)
                                        {
                                            bpm = (60 / bpmConverted).ToString("N1");
                                        }
                                    }

                                    bpmList.Enqueue(bpm);

                                    isEnabledKeyAndBpmReading = false;
                                }
                            }

                        }

                    }
                    //});
                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }
            }

            if (result.Success)
            {
                try
                {
                    List<string> finalFilePathList = filePathList.ToList();
                    List<string> finalBpmList = bpmList.ToList();

                    for (int i = 0; i < filePathList.Count; i++)
                    {
                        Model.Track currentTrack = trackList.Find(x => x.Path == finalFilePathList[i]);
                        int index = trackList.IndexOf(currentTrack);
                        if (currentTrack != null)
                        {
                            foreach (TrackTagValue ttv in currentTrack.TrackTagValues)
                            {
                                if (ttv.TagName == "Bpm" && bpmTagValueList != null && bpmTagValueList.Count > 0)
                                {
                                    TagValue bpmTagValue = bpmTagValueList[0];
                                    if (bpmTagValue != null)
                                    {
                                        ttv.TagValueId = bpmTagValue.Id;
                                        ttv.TagValueName = bpmTagValue.Name;
                                        ttv.Value = finalBpmList[i];
                                        ttv.HasMultipleValues = true;
                                    }
                                    trackDao.UpdateTrackTagValue(ttv);
                                }
                            }
                        }
                        trackList[index] = currentTrack;
                    }

                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }

            }

            return result;

        }



        public List<TagValue> KeyList { get; set; }



        private ISettingDao settingDao { get; set; }

        public VirtualDJReader(ITrackDao trackDao, ISettingDao settingDao, String[] filePaths)
        {
         
            /*
            this.VDJTracklist = new List<VDJTrack>();
            this.VirtualDjDatabasePathList = new List<String>();

            String drive = String.Empty;
            String virtualDjDatabasePath = String.Empty;

            foreach (String filePath in filePaths)
            {
                drive = filePath.Substring(0, 1);
                virtualDjDatabasePath = drive + ":\\VirtuaDJ\\database.xml";

                if (File.Exists(virtualDjDatabasePath) && !this.VirtualDjDatabasePathList.Contains(virtualDjDatabasePath))
                {
                    this.VirtualDjDatabasePathList.Add(virtualDjDatabasePath);
                }
                else
                {
                    virtualDjDatabasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ":\\VirtuaDJ\\database.xml";
                    if (File.Exists(virtualDjDatabasePath) && !this.VirtualDjDatabasePathList.Contains(virtualDjDatabasePath))
                    {
                        this.VirtualDjDatabasePathList.Add(virtualDjDatabasePath);
                    }
                }
            }

            if (this.VirtualDjDatabasePathList == null || this.VirtualDjDatabasePathList.Count == 0)
            {
                MessageBox.Show("The program can't find any VirtualDj database file on the currently connected hardrives and pendrives!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                String keyCodes = System.Configuration.ConfigurationManager.AppSettings[Settings.KeyCodes.ToString()];
                String keys = System.Configuration.ConfigurationManager.AppSettings[Settings.Keys.ToString()];
                String keysAlter = System.Configuration.ConfigurationManager.AppSettings[Settings.KeysAlter.ToString()];
                this.KeyCodesArray = Array.ConvertAll(keyCodes.Split(','), s => s);
                this.KeysArray = Array.ConvertAll(keys.Split(','), s => s);
                this.KeysAlterArray = Array.ConvertAll(keysAlter.Split(','), s => s);

                foreach(String path in this.VirtualDjDatabasePathList)
                {
                    this.GetAttributeListFromVirtualDjDatabase(path);
                }
            }*/
        }

        public void GetAttributeListFromVirtualDjDatabase(String databasePath)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(databasePath);

                String bpm = String.Empty;
                String key = String.Empty;
                String path = String.Empty;

                foreach (XmlNode mainNodes in xDoc.DocumentElement.ChildNodes)
                {
                    if (mainNodes.Name == "Song" && mainNodes.Attributes != null && mainNodes.Attributes["FilePath"] != null)
                    {
                        VDJTrack vdjTrack = new VDJTrack();
                        vdjTrack.Path = mainNodes.Attributes["FilePath"].Value;

                        foreach (XmlNode child in mainNodes)
                        {
                            if (child.Name == "Scan" && child.Attributes != null)
                            {
                                if (child.Attributes["Bpm"] != null)
                                {
                                    bpm = child.Attributes["Bpm"].Value;

                                    Decimal bpmConverted = Convert.ToDecimal(bpm.Replace(".", ","));
                                    if (bpmConverted > 0)
                                    {
                                        vdjTrack.Bpm = 60 / bpmConverted;
                                    }
                                }
                                if (child.Attributes["Key"] != null)
                                {
                                    key = child.Attributes["Key"].Value;
                                    vdjTrack.KeyCode = this.KeyToKeyCode(key);
                                }
                                break;
                            }
                        }
                        this.VDJTracklist.Add(vdjTrack);
                    }
                }

                if(this.VDJTracklist != null && this.VDJTracklist.Count > 0)
                {
                    for(int i = this.VDJTracklist.Count -1; i >=0; i--)
                    {
                        for (int j = this.VDJTracklist.Count - 1; j >= 0; j--)
                        {
                            if (this.VDJTracklist[i].Path == this.VDJTracklist[j].Path)
                            {
                                this.VDJTracklist.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
            catch { }
        }
        public String KeyToKeyCode(String key)
        {
            String result = "";

            for (int i = 0; i <= this.KeysArray.Count() - 1; i++)
            {
                if (this.KeysArray[i] == key)
                {
                    return this.KeyCodesArray[i];
                }
                else if (this.KeysAlterArray[i] == key)
                {
                    return this.KeyCodesArray[i];
                }
            }
            return result;
        }

        private Thread loadingThread;
        private LoadingDialog loadingDialog;
        private void ShowLoadingDialog()
        {
            loadingDialog = new LoadingDialog();
            loadingDialog.ShowDialog();
        }

        private void CloseLoadingDialog()
        {
            if (loadingDialog != null && loadingDialog.InvokeRequired)
            {
                loadingDialog.Invoke(new Action(() => loadingDialog.Close()));
            }
            else
            {
                loadingDialog?.Close();
            }

            // Ensure the thread is properly terminated
            if (loadingThread != null && loadingThread.IsAlive)
            {
                loadingThread.Join();
                loadingThread = null;
            }
        }
        

        //ez egy költséges xml olvasás
        //az útvonal alapján azonosítjuk, hogy a vitualdj adatbázisa hol lehet
        public void ReadKeyAndBpmFromVirtualDJDatabase(String path, ref String key, ref String bpm)
        {
            String vdjDatabaseFilePath = this.DetectVdjDatabaseFile(path);

            String keyCodes = System.Configuration.ConfigurationManager.AppSettings[Settings.KeyCodes.ToString()];
            String keys = System.Configuration.ConfigurationManager.AppSettings[Settings.Keys.ToString()];
            String keysAlter = System.Configuration.ConfigurationManager.AppSettings[Settings.KeysAlter.ToString()];
            this.KeyCodesArray = Array.ConvertAll(keyCodes.Split(','), s => s);
            this.KeysArray = Array.ConvertAll(keys.Split(','), s => s);
            this.KeysAlterArray = Array.ConvertAll(keysAlter.Split(','), s => s);

            if (!String.IsNullOrEmpty(vdjDatabaseFilePath))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(vdjDatabaseFilePath);
                    XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("Song");

                    int startIndex = path.LastIndexOf('\\') + 1;
                    int endIndex = path.Count();
                    String fileName = path.Substring(startIndex, endIndex - startIndex);

                    foreach (XmlNode node in nodeList)
                    {
                        if (node.Attributes["FilePath"].Value.Contains(fileName))
                        {
                            XmlNodeList list = node.SelectNodes("Scan");
                            if (list != null && list.Count > 0)
                            {
                                key = list[0].Attributes["Key"].Value;
                                key = this.KeyToKeyCode(key);

                                bpm = list[0].Attributes["Bpm"].Value;
                                Decimal bpmConverted = Convert.ToDecimal(bpm.Replace(".", ","));
                                if (bpmConverted > 0)
                                {
                                    bpm = (60 / bpmConverted).ToString("N1");
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        //hol lehet a virtualdj adatbázisa
        //path alapján megnézzük, hogy mi a meghajtó, majd azon a meghajtón megnézzük a sima virtualdj könyvtár alatt az adatbázist
        //a path-ból kiszedjük a fájlnevet
        private String DetectVdjDatabaseFile(String path)
        {
            String drive = path.Substring(0, 1);
            String vdjDatabaseFilePath = drive + ":\\VirtualDJ\\database.xml";

            int startIndex = path.LastIndexOf('\\') + 1;
            int endIndex = path.Count();
            String fileName = path.Substring(startIndex, endIndex - startIndex);

            bool isExists = false;

            //ha megvan a virtualdj adatbázis, megnézzük, hogy a fájlnév látezik-e benne
            //REFACTOR: összeszedni a meghajtókat, és meghajtóként azonosítani az adatbázis és megnézni, hogy létezik-e benne a fájl és be is olvasni az adatait

            if (File.Exists(vdjDatabaseFilePath))
            {
                using (StreamReader sr = new StreamReader(vdjDatabaseFilePath))
                {
                    while (!sr.EndOfStream)
                    {
                        if (sr.ReadLine().Contains(fileName))
                        {
                            isExists = true;
                            break;
                        }
                    }
                }
            }

            if (!isExists)
            {
                vdjDatabaseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VirtualDJ\\database.xml";
                if (File.Exists(vdjDatabaseFilePath))
                {
                    using (StreamReader sr = new StreamReader(vdjDatabaseFilePath))
                    {
                        while (!sr.EndOfStream)
                        {
                            if (sr.ReadLine().Contains(fileName))
                            {
                                isExists = true;
                                break;
                            }
                        }
                    }
                }
            }

            return vdjDatabaseFilePath;
        } 

        public void ReadVirtualDJDatabase(ref List<Model.Track> trackList, List<TagValue> keyList)
        {
            if(trackList != null && trackList.Count > 0)
            {
                trackList = trackList.OrderBy(x => x.Path).ToList();

                foreach (Model.Track track in trackList)
                {
                    VDJTrack vdjTrack = this.VDJTracklist.Find(x => x.Path == track.Path);
                    if (vdjTrack != null)
                    {
                        TrackTagValue ttvBpm = track.TrackTagValues.Find(x => x.TagName == "Bpm");
                        if (ttvBpm != null)
                        {
                            ttvBpm.Value = vdjTrack.Bpm.ToString("N1");
                            ttvBpm.HasMultipleValues = true;
                            //trackDao.UpdateTrackTagValue(ttvBpm);
                        }

                        TrackTagValue ttvKey = track.TrackTagValues.Find(x => x.TagName == "Key");
                        TagValue keyTagValue = keyList.Find(x => x.Name == vdjTrack.KeyCode);
                        if (ttvKey != null && keyTagValue != null)
                        {
                            ttvKey.TagValueId = keyTagValue.Id;
                            ttvKey.TagValueName = keyTagValue.Name;
                            ttvKey.HasMultipleValues = false;
                            //trackDao.UpdateTrackTagValue(ttvKey);
                        }
                    }
                }
            }
        }
    }
}
