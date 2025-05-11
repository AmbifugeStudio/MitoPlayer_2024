using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Properties;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MitoPlayer_2024.Presenters
{
    public class TagValueImportPresenter
    {
        private ITagValueImportView _view;
        private ITagDao _tagDao;
        private String _script;
        private String _tutorialText;

        public List<String> TagNames { get; set; }
        public List<List<String>> TagValueNames { get; set; }
        public List<List<String>> ColorCodes { get; set; }

        public TagValueImportPresenter(ITagValueImportView view, ITagDao tagDao)
        {
            _view = view;
            _tagDao = tagDao;
            _tutorialText = this.LoadTutorial();
            _script = this.LoadScript();

            _view.CloseViewEvent += TagValueImportView_CloseView;
            _view.GenerateScriptEvent += GenerateScriptEvent;
            _view.SaveScriptEvent += SaveScriptEvent;

            TagNames = new List<String>();
            TagValueNames = new List<List<String>>();
            ColorCodes = new List<List<String>>();

            ((TagValueImportView)_view).InitilizeScript(this._script);
            ((TagValueImportView)_view).InitilizeTutorial(this._tutorialText);
            ((TagValueImportView)_view).DialogResult = DialogResult.Cancel;
        }

        private String LoadTutorial()
        {
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string resourcesPath = Path.Combine(projectDirectory, "Resources", "TagValueScriptTutorial.txt");
            return File.ReadAllText(resourcesPath);
        }
        private String LoadScript()
        {
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string resourcesPath = Path.Combine(projectDirectory, "Resources", "TagValueScript.txt");
            return File.ReadAllText(resourcesPath);
        }
        private void GenerateScriptEvent(object sender, EventArgs e)
        {
            String script = String.Empty;
            List<Tag> tags = new List<Tag>();
            List<TagValue> tagValues = new List<TagValue>();
            String comma = ",";

            tags = _tagDao.GetAllTag().Value;

            if(tags != null && tags.Count > 0)
            {
                foreach (Tag tag in tags)
                {
                    script += tag.Name;
                    if (tag.TextColoring)
                    {
                        script += "(Text";
                    }
                    else
                    { 
                        script += "(Field";
                    }
                    if (tag.HasMultipleValues)
                    {
                        script += ",HasMultipleValues):";
                    }
                    else
                    {
                        script += ",NoMultipleValues):";
                    }

                    tagValues = _tagDao.GetTagValuesByTagId(tag.Id).Value;

                    if (tagValues != null && tagValues.Count > 0)
                    {
                        foreach (TagValue tagValue in tagValues)
                        {
                            if (script[script.Length-1] != ':')
                            {
                                script += comma;
                            }
                            script += tagValue.Name + "(" + ColorToHex(tagValue.Color) + ")";
                        }
                        script += "\n";
                    }
                }
            }

            _script = script;

            ((TagValueImportView)_view).InitilizeScript(_script);
        }
        private void SaveScriptEvent(object sender, Messenger e)
        {
            List<String> errors = ValidateScript(e.StringField1);
            if (errors != null && errors.Count > 0)
            {
                string errorMessage = string.Join("\n", errors);
                MessageBox.Show(errorMessage, "Script Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _script = e.StringField1;
                SaveScript(_script);
            }
        }

        private static readonly Regex LinePattern = new Regex(
            @"^[A-Za-z0-9/_]+\(Text|Field,(NoMultipleValues|HasMultipleValues)\):((?:[A-Za-z0-9/_ .]+\((?:#[0-9A-Fa-f]{6}|[A-Za-z]+)\),?)+)$",
            RegexOptions.Compiled);

        public List<string> ValidateScript(string script)
        {
            var tags = new HashSet<string>();
            var errors = new List<string>();

            if (String.IsNullOrEmpty(script))
            {
                errors.Add("No script to process.");
            }
            else {
                string[] lines = script.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (!ValidateLine(line, tags, i + 1, errors))
                    {
                        // Hiba esetén a ValidateLine hozzáadja a hibát az errors listához
                    }
                }
            }

            return errors;
        }
        private bool ValidateLine(string line, HashSet<string> tags, int lineNumber, List<string> errors)
        {
            Match match = LinePattern.Match(line);
            if (!match.Success)
            {
                errors.Add($"Hiba a {lineNumber}. sorban: A sor formátuma nem megfelelő.");
                return false;
            }

            string tagName = match.Groups[0].Value.Split('(')[0];
            if (!tags.Add(tagName))
            {
                errors.Add($"Hiba a {lineNumber}. sorban: A '{tagName}' tag már szerepel.");
                return false;
            }

            string tagValuesPart = match.Groups[2].Value;
            var tagValues = new HashSet<string>();
            string[] tagValuesArray = tagValuesPart.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string tagValue in tagValuesArray)
            {
                if (!ValidateTagValue(tagValue, tagValues, lineNumber, errors))
                {
                    return false;
                }
            }

            return true;
        }
        private bool ValidateTagValue(string tagValue, HashSet<string> tagValues, int lineNumber, List<string> errors)
        {
            if (!Regex.IsMatch(tagValue, @"^[A-Za-z0-9/_ .]+\(#(?:[0-9A-Fa-f]{6})\)$"))
            {
                errors.Add($"Hiba a {lineNumber}. sorban: A '{tagValue}' tag value formátuma nem megfelelő. A tag value nem tartalmazhat vesszőt vagy zárójelet, és a színkódnak hexadecimálisnak kell lennie vagy egy színnévnek.");
                return false;
            }

            string tagValueName = tagValue.Split('(')[0];
            if (!tagValues.Add(tagValueName))
            {
                errors.Add($"Hiba a {lineNumber}. sorban: A '{tagValueName}' tag value már szerepel ebben a tag-ben.");
                return false;
            }

            return true;
        }
        private void SaveScript(string script)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TagValueScript.txt");
            File.WriteAllText(filePath, script);

            string savedScript = File.ReadAllText(filePath);
            if (script.Equals(savedScript))
            {
                MessageBox.Show($"Script saved successfuly!", "Siker", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }
        public string ColorToHex(Color color)
        {
            return ColorTranslator.ToHtml(color);
        }

        private void TagValueImportView_CloseView(object sender, Messenger e)
        {
            List<String> errors = ValidateScript(e.StringField1);
            if (errors != null && errors.Count > 0)
            {
                string errorMessage = string.Join("\n", errors);
                MessageBox.Show(errorMessage, "Script Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            String script = e.StringField1;
            TagNames = new List<String>();
            TagValueNames = new List<List<String>>();
            ColorCodes = new List<List<String>>();

            String[] lines = script.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                String[] parts = lines[i].Split(':');
                String[] tagValues = parts[1].Split(',');
                String[] colorArray = new String[tagValues.Length];

                for (int j = 0; j < tagValues.Length; j++)
                {
                    String tagValue = tagValues[j];
                    String hexaCode = String.Empty;

                    int brIdx = tagValues[j].LastIndexOf('(');
                    if (brIdx >= 0)
                    {
                        hexaCode = tagValue.Substring(brIdx);
                        hexaCode = hexaCode.Remove(0, 1);
                        hexaCode = hexaCode.Remove(hexaCode.Length - 1, 1);

                        tagValues[j] = tagValue.Substring(0, brIdx);
                        colorArray[j] = hexaCode;
                    }
                    else
                    {
                        colorArray[j] = "";
                    }
                }

                TagNames.Add(parts[0]);
                TagValueNames.Add(new List<String>(tagValues));
                ColorCodes.Add(new List<String>(colorArray));
            }

            ((TagValueImportView)this._view).DialogResult = DialogResult.OK;
            ((TagValueImportView)this._view).Close();
            /*
            if (String.IsNullOrEmpty(script))
            {
                MessageBox.Show("No script to process.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ((TagValueImportView)this._view).Close();
            }
            else
            {
                String[] lines = null;

                if (result.Success)
                {
                    if (script.Length > 255)
                    {
                        result.AddError("The script must be shorter than 255 characters!");
                    }
                }

                if (result.Success)
                {
                    lines = script.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                    List<String> tvList = new List<String>();
                    List<String> colorList = new List<String>();

                    for (int i = 0; i <= lines.Count() - 1; i++)
                    {
                        String[] parts = null;
                        String[] tagValues = null;
                        String[] colorArray = null;

                        if (lines[i].Count(f => f == ':') > 1)
                        {
                            result.AddError("Syntax error! (more than one colon)" + " Line: " + (i + 1).ToString());
                            break;
                        }

                        if (result.Success)
                        {
                            if (lines[i].Count(f => f == ':') == 0)
                            {
                                result.AddError("Syntax error! (missing colon)" + " Line: " + (i + 1).ToString());
                                break;
                            }
                        }

                        if (result.Success)
                        {
                            parts = lines[i].Split(':');
                            if (parts[0].Contains(','))
                            {
                                result.AddError("Syntax error! (tag name must not contains comma)" + " Line: " + (i + 1).ToString());
                                break;
                            }

                            if (result.Success)
                            {
                                if (String.IsNullOrEmpty(parts[0]))
                                {
                                    result.AddError("Syntax error! (tag name missing)" + " Line: " + (i + 1).ToString());
                                    break;
                                }
                            }

                            if (result.Success)
                            {
                                if (String.IsNullOrEmpty(parts[1]))
                                {
                                    result.AddError("Syntax error! (tag value name missing)" + " Line: " + (i + 1).ToString());
                                    break;
                                }
                            }

                            if (result.Success)
                            {
                                tagValues = parts[1].Split(',');
                                colorArray = new String[tagValues.Length];

                                foreach (String s in tagValues)
                                {
                                    if ((s.Contains("(") && !s.Contains(")")) || (!s.Contains("(") && s.Contains(")")))
                                    {
                                        result.AddError("Syntax error! (missing bracket in color definition)" + " Line: " + (i + 1).ToString());
                                        break;
                                    }
                                }

                                if (result.Success)
                                {
                                    for (int j = 0; j < tagValues.Count(); j++)
                                    {
                                        if (tagValues[j].Contains("(") && tagValues[j].Contains(")"))
                                        {
                                            String tagValue = tagValues[j];
                                            String hexaCode = String.Empty;

                                            int brIdx = tagValues[j].LastIndexOf("(");
                                            if (brIdx >= 0)
                                            {
                                                hexaCode = tagValue.Substring(brIdx);
                                                hexaCode = hexaCode.Remove(0, 1);
                                                hexaCode = hexaCode.Remove(hexaCode.Length - 1, 1);

                                                tagValues[j] = tagValue.Substring(0, brIdx);

                                                if (this.IsHexCodeValid(hexaCode))
                                                {
                                                    colorArray[j] = hexaCode;
                                                }
                                                else
                                                {
                                                    result.AddError("Syntax error! (invalid hexadecimal color definition)" + " Line: " + (i + 1).ToString());
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            colorArray[j] = "";
                                        }
                                    }
                                }
                            }
                        }

                        if (result.Success)
                        {
                            if (TagNames.Contains(parts[0]))
                            {
                                result.AddError("Syntax error! (tag name already exists in the script)" + " Line: " + (i + 1).ToString());
                                break;
                            }
                            else
                            {
                                List<Tag> tagList = this._tagDao.GetAllTag();
                                if (tagList != null && tagList.Count > 0)
                                {
                                    if (tagList.Exists(x => x.Name.Equals(parts[0])))
                                    {
                                        result.AddError("Syntax error! (tag name already exists in the database)" + " Line: " + (i + 1).ToString());
                                        break;
                                    }
                                    else
                                    {
                                        TagNames.Add(parts[0]);
                                    }
                                }
                            }
                        }

                        List<String> tmp = new List<String>();
                        if (result.Success)
                        {
                            foreach (String tagValue in tagValues)
                            {
                                if (tmp.Contains(tagValue))
                                {
                                    result.AddError("Syntax error! (tag value name already exists for this tag)" + " Line: " + (i + 1).ToString());
                                    break;
                                }
                                else
                                {
                                    tmp.Add(tagValue);
                                }
                            }

                        }
                        List<String> tmp2 = new List<String>();
                        if (result.Success)
                        {
                            foreach (String colorCode in colorArray)
                            {
                                tmp2.Add(colorCode);
                            }

                        }

                        if (result.Success)
                        {
                            TagValueNames.Add(tmp);
                            ColorCodes.Add(tmp2);
                        }

                    }
                }

                if (result.Success)
                {
                    ((TagValueImportView)this._view).DialogResult = DialogResult.OK;
                    ((TagValueImportView)this._view).Close();
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }*/

        }
        public bool IsHexCodeValid(string hexCode)
        {
            return Regex.Match(hexCode, "^#[0-9a-fA-F]{6}$").Success;
        }

    }
}
