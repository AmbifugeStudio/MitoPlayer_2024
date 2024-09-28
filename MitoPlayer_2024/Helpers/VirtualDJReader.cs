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

        private void InitializeAvailableDatabaseFiles()
        {
            String letters = "ABCDEFGHIJKLMNOPQRSTIJKLMNOPQRSTUVWXYZ";
            String vdjDatabaseFilePath = String.Empty;

            foreach (char drive in letters)
            {
                vdjDatabaseFilePath = drive + ":\\VirtuaDJ\\database.xml";
                if (File.Exists(vdjDatabaseFilePath) && !this.VdjDatabasePathList.Contains(vdjDatabaseFilePath))
                {
                    this.VdjDatabasePathList.Add(vdjDatabaseFilePath);
                }
            }
            vdjDatabaseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + ":\\VirtuaDJ\\database.xml";
            if (File.Exists(vdjDatabaseFilePath) && !this.VdjDatabasePathList.Contains(vdjDatabaseFilePath))
            {
                this.VdjDatabasePathList.Add(vdjDatabaseFilePath);
            }

            
        }


        
        public String[] KeyCodesArray { get; set; }
        public String[] KeysArray { get; set; }
        public String[] KeysAlterArray { get; set; }
        public List<VDJTrack> VDJTracklist { get; set; }
        public List<TagValue> KeyList { get; set; }

        private ITrackDao trackDao { get; set; }
        private ISettingDao settingDao { get; set; }

        public VirtualDJReader(ITrackDao trackDao, ISettingDao settingDao, String[] filePaths)
        {
          /*  this.trackDao = trackDao;
            this.settingDao = settingDao;

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
        private String DetectVdjDatabaseFile(String path)
        {
            String drive = path.Substring(0, 1);
            String vdjDatabaseFilePath = drive + ":\\VirtualDJ\\database.xml";

            int startIndex = path.LastIndexOf('\\') + 1;
            int endIndex = path.Count();
            String fileName = path.Substring(startIndex, endIndex - startIndex);

            bool isExists = false;

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

        public void ReadVirtualDJDatabase(ref List<Track> trackList, List<TagValue> keyList)
        {
            if(trackList != null && trackList.Count > 0)
            {
                trackList = trackList.OrderBy(x => x.Path).ToList();

                foreach (Track track in trackList)
                {
                    VDJTrack vdjTrack = this.VDJTracklist.Find(x => x.Path == track.Path);
                    if (vdjTrack!= null)
                    {
                        TrackTagValue ttvBpm = track.TrackTagValues.Find(x => x.TagName == "Bpm");
                        if (ttvBpm != null)
                        {
                            ttvBpm.Value = vdjTrack.Bpm.ToString("N1");
                            trackDao.UpdateTrackTagValue(ttvBpm);
                        }

                        TrackTagValue ttvKey = track.TrackTagValues.Find(x => x.TagName == "Key");
                        TagValue keyTagValue = keyList.Find(x => x.Name == vdjTrack.KeyCode);
                        if (ttvKey != null && keyTagValue != null)
                        {
                            ttvKey.TagValueId = keyTagValue.Id;
                            ttvKey.TagValueName = keyTagValue.Name;
                            ttvKey.HasValue = true;
                            trackDao.UpdateTrackTagValue(ttvKey);
                        }
                    }
                }
            }
        }
    }
}
