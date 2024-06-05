using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public interface ITagDao
    {
        int GetNextId(String tableName);

        #region TAG
        void SetProfileId(int profileId);
        void CreateTag(Tag tag);
        Tag GetTag(int id);
        Tag GetTagByName(String name);
        List<Tag> GetAllTag();
        void UpdateTag(Tag tag);
        void DeleteTag(int id);
        void DeleteAllTag();
        void ClearTagTable();
        #endregion

        #region TAGVALUE
        void CreateTagValue(TagValue tagValue);
        List<TagValue> GetTagValuesByTagId(int id);
        TagValue GetTagValue(int id);
        TagValue GetTagValueByTagId(int id, int tagId);
        TagValue GetTagValueByName(int tagId, String name);
        void UpdateTagValue(TagValue tag);
        void DeleteTagValue(int id);
        void DeleteTagValuesByTagId(int tagId);
        void DeleteAllTagValue();
        void ClearTagValueTable();
        #endregion
    }
}
