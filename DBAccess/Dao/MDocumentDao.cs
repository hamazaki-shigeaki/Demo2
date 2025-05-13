using System;
using System.Text;
using Dapper;
using DBAccess.Model;
using DBAccess.Context;
using DBAccess.Logics;
using Model.CBean;

namespace DBAccess.Dao
{
    /// <summary>
    /// MDocumentのDaoクラスを定義します。
    /// </summary>
    public partial class MDocumentDao : CommonLogic
    {
        /// <summary>DBコンテキスト</summary>
        private ApplicationDbContext con = null;

        #region "コンストラクタ"
        public MDocumentDao(ApplicationDbContext context) : base(context)
        {
            con = context;
        }
        #endregion

        #region "Insert"
        /// <summary>
        /// Insert(MDocument)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Insert(MDocument entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("INSERT INTO M_DOCUMENT (");
            sql.Append("        ID");
            sql.Append("        ,DOCUMENT_NO");
            sql.Append("        ,SUIT_NO");
            sql.Append("        ,DOC_CLASSIFICATION");
            sql.Append("        ,DOC_NAME");
            sql.Append("        ,CONTENTS");
            sql.Append("        ,FOLDER_NAME");
            sql.Append("        ,FILE_NAME");
            sql.Append("        ,REGISTER_USER");
            sql.Append("        ,REGISTER_DATETIME");
            sql.Append("        ,REGISTER_PROCESS");
            sql.Append("        ,UPDATE_USER");
            sql.Append("        ,UPDATE_DATETIME");
            sql.Append("        ,UPDATE_PROCESS");
            sql.Append(") VALUES (");
            sql.Append("        @Id");
            sql.Append("        ,@DocumentNo");
            sql.Append("        ,@SuitNo");
            sql.Append("        ,@DocClassification");
            sql.Append("        ,@DocName");
            sql.Append("        ,@Contents");
            sql.Append("        ,@FolderName");
            sql.Append("        ,@FileName");
            sql.Append("        ,@RegisterUser");
            sql.Append("        ,@RegisterDatetime");
            sql.Append("        ,@RegisterProcess");
            sql.Append("        ,@UpdateUser");
            sql.Append("        ,@UpdateDatetime");
            sql.Append("        ,@UpdateProcess");
            sql.Append(");");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("DocumentNo", entity.DocumentNo);
            param.Add("SuitNo", entity.SuitNo);
            param.Add("DocClassification", entity.DocClassification);
            param.Add("DocName", entity.DocName);
            param.Add("Contents", entity.Contents);
            param.Add("FolderName", entity.FolderName);
            param.Add("FileName", entity.FileName);
            param.Add("RegisterUser", con.UserId.ToString());
            param.Add("RegisterDatetime", now);
            param.Add("RegisterProcess", con.Process);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", now);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Update"
        /// <summary>
        /// Update(MDocument)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Update(MDocument entity)
        {
            var sql = new StringBuilder(); 
            sql.Append($"UPDATE M_DOCUMENT");
            sql.Append($"   SET DOCUMENT_NO = @DocumentNo");
            sql.Append($",       SUIT_NO = @SuitNo");
            sql.Append($",       DOC_CLASSIFICATION = @DocClassification");
            sql.Append($",       DOC_NAME = @DocName");
            sql.Append($",       CONTENTS = @Contents");
            sql.Append($",       FOLDER_NAME = @FolderName");
            sql.Append($",       FILE_NAME = @FileName");
            sql.Append($",       UPDATE_USER = @UpdateUser");
            sql.Append($",       UPDATE_DATETIME = SYSDATETIME() ");
            sql.Append($",       UPDATE_PROCESS = @UpdateProcess");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("DocumentNo", entity.DocumentNo);
            param.Add("SuitNo", entity.SuitNo);
            param.Add("DocClassification", entity.DocClassification);
            param.Add("DocName", entity.DocName);
            param.Add("Contents", entity.Contents);
            param.Add("FolderName", entity.FolderName);
            param.Add("FileName", entity.FileName);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", entity.UpdateDatetime);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Delete"
        /// <summary>
        /// Delete(MDocument)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Delete(MDocument entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("DELETE FROM M_DOCUMENT");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("UpdateDatetime", entity.UpdateDatetime);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Find"
        /// <summary>
        /// Find
        /// </summary>
        /// <param name="id">ID</param>
        public MDocument Find(long? id)
        {
            var cb = new MDocumentCB(); 
            cb.Query().SetId_Equal(id); 
            return ExecuteEntity<MDocument>(cb); 
        }
        #endregion

        #region "Exist"
        /// <summary>
        /// Exist
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>取得結果(存在:=true,存在しない:=false)</returns>
        public bool Exist(long? id)
        {
            if(Find(id) == null) 
            {
               return false; 
            }
            return true;
        }
        #endregion
    }
}
