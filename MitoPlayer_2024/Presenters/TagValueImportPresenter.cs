using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class TagValueImportPresenter
    {
        private ITagValueImportView view;
        public Playlist newPlaylist;
        public ITagDao tagDao;

        public List<String> tagNames = new List<String>();
        public List<List<String>> tagValueNames = new List<List<String>>();
        public List<List<String>> colorCodes = new List<List<String>>();

        public TagValueImportPresenter(ITagValueImportView view, ITagDao tagDao)
        {
            this.view = view;
            this.view.CloseView += TagValueImportView_CloseView;
            this.tagDao = tagDao;

            ((TagValueImportView)this.view).DialogResult = DialogResult.Cancel;
        }

        private void TagValueImportView_CloseView(object sender, Messenger e)
        {
            ResultOrError result = new ResultOrError();
            String script = e.StringField1;
            tagNames = new List<String>();
            tagValueNames = new List<List<String>>();
            colorCodes = new List<List<String>>();

            if (String.IsNullOrEmpty(script))
            {
                MessageBox.Show("No script to process.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ((TagValueImportView)this.view).Close();
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

                    for (int i = 0;i <= lines.Count() - 1; i++)
                    {
                        String[] parts = null;
                        String[] tagValues = null;
                        String[] colorArray = null;

                        if (lines[i].Count(f => f == ':') > 1)
                        {
                            result.AddError("Syntax error! (more than one colon)" + " Line: " + (i+1).ToString());
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
                                    for(int j = 0; j < tagValues.Count(); j++)
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
                            if (tagNames.Contains(parts[0]))
                            {
                                result.AddError("Syntax error! (tag name already exists in the script)" + " Line: " + (i + 1).ToString());
                                break;
                            }
                            else
                            {
                                List<Tag> tagList = this.tagDao.GetAllTag();
                                if (tagList != null && tagList.Count > 0)
                                {
                                    if (tagList.Exists(x => x.Name.Equals(parts[0])))
                                    {
                                        result.AddError("Syntax error! (tag name already exists in the database)" + " Line: " + (i + 1).ToString());
                                        break;
                                    }
                                    else
                                    {
                                        tagNames.Add(parts[0]);
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
                            tagValueNames.Add(tmp);
                            colorCodes.Add(tmp2);
                        }

                    }
                }

                if (result.Success)
                {
                    ((TagValueImportView)this.view).DialogResult = DialogResult.OK;
                    ((TagValueImportView)this.view).Close();
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        public bool IsHexCodeValid(string hexCode)
        {
            return Regex.Match(hexCode, "^#[0-9a-fA-F]{6}$").Success;
        }

    }
}
