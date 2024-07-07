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

namespace MitoPlayer_2024.Helpers
{
    public class VDJTrack
    {
        public String Path { get; set; }
        public String KeyCode { get; set; }
        public Decimal Bpm { get; set; }
    }
    public class VirtualDJReader
    {

        public String VirtualDjDatabasePath { get; set; }
        public String[] KeyCodesArray { get; set; }
        public String[] KeysArray { get; set; }
        public String[] KeysAlterArray { get; set; }
        public List<VDJTrack> VDJTracklist { get; set; }
        public List<TagValue> KeyList { get; set; }


        private ITrackDao trackDao { get; set; }

        public VirtualDJReader(ITrackDao trackDao)
        {
            this.trackDao = trackDao;


            this.VirtualDjDatabasePath = "C:/Users/Szalas_Portable/AppData/Local/VirtualDJ/database.xml";

            String keyCodes = System.Configuration.ConfigurationManager.AppSettings[Settings.KeyCodes.ToString()];
            String keys = System.Configuration.ConfigurationManager.AppSettings[Settings.Keys.ToString()];
            String keysAlter = System.Configuration.ConfigurationManager.AppSettings[Settings.KeysAlter.ToString()];
            this.KeyCodesArray = Array.ConvertAll(keyCodes.Split(','), s => s);
            this.KeysArray = Array.ConvertAll(keys.Split(','), s => s);
            this.KeysAlterArray = Array.ConvertAll(keysAlter.Split(','), s => s);

            this.GetAttributeListFromVirtualDjDatabase();
        }

        public void GetAttributeListFromVirtualDjDatabase()
        {
            this.VDJTracklist = new List<VDJTrack>();

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(this.VirtualDjDatabasePath);

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

        public void ReadKeyFromVirtualDJDatabase(String path, ref TrackTagValue ttv, List<TagValue> keyList)
        {
            if(ttv != null)
            {
                VDJTrack vdjTrack = this.VDJTracklist.Find(x => x.Path == path);
                if (vdjTrack != null)
                {
                    if (ttv.TagName == "Key")
                    {
                        TagValue keyTagValue = keyList.Find(x => x.Name == vdjTrack.KeyCode);
                        if (keyTagValue != null)
                        {
                            ttv.TagValueId = keyTagValue.Id;
                            ttv.TagValueName = keyTagValue.Name;
                        }
                    }
                }
            }
        }
        public void ReadBpmFromVirtualDJDatabase(String path, ref TrackTagValue ttv, List<TagValue> keyList)
        {
            if (ttv != null)
            {
                VDJTrack vdjTrack = this.VDJTracklist.Find(x => x.Path == path);
                if (vdjTrack != null)
                {
                    if (ttv.TagName == "Bpm")
                    {
                        ttv.Value = vdjTrack.Bpm.ToString("N1");
                        ttv.HasValue = true;

                        TagValue keyTagValue = keyList.Find(x => x.Name =="Bpm");
                        if (keyTagValue != null)
                        {
                            ttv.TagValueId = keyTagValue.Id;
                            ttv.TagValueName = "";
                        }
                    }
                }
            }
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
