using MitoPlayer_2024.Helpers.ErrorHandling;
using System;
using System.Collections.Generic;

namespace MitoPlayer_2024.Models
{
    public interface ITagDao
    {

        #region TAG
        void SetProfileId(int profileId);
        ResultOrError CreateTag(Tag tag);
        ResultOrError<Tag> GetTag(int id);
        ResultOrError<Tag> GetTagByName(String name);
        ResultOrError<List<Tag>> GetAllTag();
        ResultOrError UpdateTag(Tag tag);
        ResultOrError DeleteTag(int id);
        ResultOrError DeleteAllTag();
        ResultOrError ClearTagTable();
        #endregion

        #region TAGVALUE
        ResultOrError CreateTagValue(TagValue tagValue);
        ResultOrError<List<TagValue>> GetTagValuesByTagId(int id);
        ResultOrError<TagValue> GetTagValue(int id);
        ResultOrError<TagValue> GetTagValueByTagId(int id, int tagId);
        ResultOrError<TagValue> GetTagValueByName(int tagId, String name);
        ResultOrError UpdateTagValue(TagValue tag);
        ResultOrError DeleteTagValue(int id);
        ResultOrError DeleteTagValuesByTagId(int tagId);
        ResultOrError DeleteAllTagValue();
        ResultOrError ClearTagValueTable();
        #endregion
    }
}
