using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public interface ITagValueDao
    {
        void SetProfileId(int profileId);

        List<Tag> GetAllTag();
        Tag GetTag(int id);
        List<TagValue> GetTagValuesByTagId(int id);
        TagValue GetTagValueByTagIdAndTagValueId(int tagId, int id);

        void CreateTag(Tag tag);
        void UpdateTag(Tag tag);
        Tag GetTagByName(String name);
        void DeleteTag(int id);
        void CreateTagValue(int tagId, TagValue tagValue);
        TagValue GetTagValueByNameAndTagId(int tagId, string stringField1);
        void UpdateTagValue(TagValue tag);
        void DeleteTagValue(int id);
        int GetLastObjectId(String tableName);
    }
}
