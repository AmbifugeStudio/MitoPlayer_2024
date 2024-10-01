using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public TagValueImportPresenter(ITagValueImportView view, ITagDao tagDao)
        {
            this.view = view;
            this.view.CloseView += TagValueImportView_CloseView;
            this.tagDao = tagDao;
        }

        private void TagValueImportView_CloseView(object sender, ListEventArgs e)
        {
            ResultOrError result = new ResultOrError();
            String script = e.StringField1;
            tagNames = new List<String>();
            tagValueNames = new List<List<String>>();

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

                    for (int i = 0;i<= lines.Count()-1;i++)
                    {
                        String[] parts = null;
                        String[] tagValues = null;

                        if (lines[i].Count(f => f == ':') > 1)
                        {
                            result.AddError("Syntax error! (more than one colon)" + " Line: " + i);
                            break;
                        }

                        if (result.Success)
                        {
                            if (lines[i].Count(f => f == ':') == 0)
                            {
                                result.AddError("Syntax error! (missing colon)" + " Line: " + i);
                                break;
                            }
                        }

                        if (result.Success)
                        {
                            if (lines[i].Count(f => f == ',') == 0)
                            {
                                result.AddError("Syntax error! (missing comma)" + " Line: " + i);
                                break;
                            }
                        }

                        if (result.Success)
                        {
                            parts = lines[i].Split(':');
                            if (parts[0].Contains(','))
                            {
                                result.AddError("Syntax error! (tag name must not contains comma)" + " Line: " + i);
                                break;
                            }

                            if (result.Success)
                            {
                                if (String.IsNullOrEmpty(parts[0]))
                                {
                                    result.AddError("Syntax error! (tag name missing)" + " Line: " + i);
                                    break;
                                }
                            }

                            if (result.Success)
                            {
                                tagValues = parts[1].Split(',');
                                if(tagValues.Count() == 1)
                                {
                                    result.AddError("Syntax error! (there must be at least two tag values)" + " Line: " + i);
                                    break;
                                }
                            }
                        }

                        if (result.Success)
                        {
                            if (tagNames.Contains(parts[0]))
                            {
                                result.AddError("Syntax error! (tag name already exists in the script)" + " Line: " + i);
                                break;
                            }
                            else
                            {
                                List<Tag> tagList = this.tagDao.GetAllTag();
                                if (tagList != null && tagList.Count > 0)
                                {
                                    if (tagList.Exists(x => x.Name.Equals(parts[0])))
                                    {
                                        result.AddError("Syntax error! (tag name already exists in the database)" + " Line: " + i);
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
                                    result.AddError("Syntax error! (tag value name already exists for this tag)" + " Line: " + i);
                                    break;
                                }
                                else
                                {
                                    tmp.Add(tagValue);
                                }   
                            }
                            
                        }

                        if (result.Success)
                        {
                            tagValueNames.Add(tmp);
                        }

                    }
                }

                if (result.Success)
                {
                    ((TagValueImportView)this.view).Close();
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }
    }
}
