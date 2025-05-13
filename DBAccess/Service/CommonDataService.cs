using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DBAccess.BSAccessores;
using DBAccess.DBAccessores;
using DBAccess.Context;
using DBAccess.Logics;
using DBAccess.Model;
using DBAccess.Resource;
using DBAccess.Attributes;
using DBAccess.Common;
using Model.AllCommon;

namespace DBAccess.Service
{
    /// <summary>
    /// 汎用データアクセスクラスを定義します。
    /// </summary>
    public class CommonDataService
    {

        /// <summary>DBContext</summary>
        public ApplicationDbContext _Context { get; set; }

        /// <summary>暗号化用KEY</summary>
        public string ivKey;

        private const int SaltByteSize = 24;
        private const int HashByteSize = 24;
        private const int HasingIterationsCount = 10101;

        /// <summary>CommonLogic</summary>
        public CommonLogic _CommonLogic { get; set; }

        /// <summary>
        /// セッションID取得
        /// </summary>
        /// <returns></returns>
        public long GetSessionId()
        {
            return _CommonLogic.GetSeq("t_session_seq");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">コンテキスト</param>
        public CommonDataService(ApplicationDbContext context)
        {
            _Context = context;
            _CommonLogic = new CommonLogic(_Context);
        }

        /// <summary>
        /// ハッシュパスワード作成
        /// </summary>
        /// <param name="password">パスワード</param>
        /// <returns>作成したハッシュパスワード</returns>
        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, SaltByteSize, HasingIterationsCount))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(HashByteSize);
            }
            byte[] dst = new byte[(SaltByteSize + HashByteSize) + 1];
            Buffer.BlockCopy(salt, 0, dst, 1, SaltByteSize);
            Buffer.BlockCopy(buffer2, 0, dst, SaltByteSize + 1, HashByteSize);
            return Convert.ToBase64String(dst);
        }

        /// <summary>
        /// パスワード検証
        /// </summary>
        /// <param name="hashedPassword">ハッシュパスワード</param>
        /// <param name="password">パスワード</param>
        /// <returns>検証結果(ERROR:false)</returns>
        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] _passwordHashBytes;
            int _arrayLen = (SaltByteSize + HashByteSize) + 1;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != _arrayLen) || (src[0] != 0))
            {
                return false;
            }
            byte[] _currentSaltBytes = new byte[SaltByteSize];
            Buffer.BlockCopy(src, 1, _currentSaltBytes, 0, SaltByteSize);
            byte[] _currentHashBytes = new byte[HashByteSize];
            Buffer.BlockCopy(src, SaltByteSize + 1, _currentHashBytes, 0, HashByteSize);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, _currentSaltBytes, HasingIterationsCount))
            {
                _passwordHashBytes = bytes.GetBytes(SaltByteSize);
            }
            return AreHashesEqual(_currentHashBytes, _passwordHashBytes);
        }

        /// <summary>
        /// ハッシュパスワード比較
        /// </summary>
        /// <param name="firstHash">比較元</param>
        /// <param name="secondHash">比較先</param>
        /// <returns>比較結果(不一致:false)</returns>
        private bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
        {
            int _minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (int i = 0; i < _minHashLength; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }

        /// <summary>
        /// ユーザーID、プロセス名をメモリーに退避
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <param name="process">プロセス名</param>
        public void SetUserIdProcess(string userId, string process)
        {
            _Context.UserId = userId;
            _Context.Process = process;
        }


        /// <summary>
        /// トランザクション開始
        /// </summary>
        public void StartTransaction()
        {
            _CommonLogic.Execute("START TRANSACTION");
        }

        /// <summary>
        /// トランザクション・コミット
        /// </summary>
        public void CommitTransaction()
        {
            _CommonLogic.Execute("COMMIT WORK");
        }

        /// <summary>
        /// トランザクション・ロールバック
        /// </summary>
        public void RollbackTransaction()
        {
            _CommonLogic.Execute("ROLLBACK WORK");
        }

        /// <summary>
        /// 区分マスタ　読込
        /// </summary>
        /// <returns></returns>
        public IList<MKubun> GetMKubunList()
        {
            var acc = new BSMKubunAccessor(_Context);
            return acc.Get();
        }

        /// <summary>
        /// 入出金表スタ　読込
        /// </summary>
        /// <returns></returns>
        public IList<MDepositTable> GetMDepositTableList()
        {
            var acc = new BSMDepositTableAccessor(_Context);
            return acc.Get();
        }

        /// <summary>
        /// 入出金表スタ　更新
        /// </summary>
        /// <returns></returns>
        public Response UpdateMDepositTable(MDepositTable entity)
        {
            var acc = new BSMDepositTableAccessor(_Context);
            var ret = acc.MDepositTableCUD(entity);
            return ret;
        }

        /// <summary>
        /// 預かり文書　読込
        /// </summary>
        /// <returns></returns>
        public IList<MDocument> GetMDocumentList()
        {
            var acc = new BSMDocumentAccessor(_Context);
            return acc.Get();
        }

        /// <summary>
        /// 債権マスタ　読込
        /// </summary>
        /// <returns></returns>
        public IList<MAssets> GetMAssetsList()
        {
            var acc = new BSMAssetsAccessor(_Context);
            return acc.Get();
        }

        /// <summary>
        /// 入出金表スタ　更新
        /// </summary>
        /// <returns></returns>
        public Response UpdateMAssets(MAssets entity)
        {
            var acc = new BSMAssetsAccessor(_Context);
            var ret = acc.MAssetsCUD(entity);
            return ret;
        }
        /// <summary>
        /// OCR　読込
        /// </summary>
        /// <returns></returns>
        public IList<TOcr> GetTOcrList()
        {
            var acc = new BSTOcrAccessor(_Context);
            return acc.Get();
        }


        /// <summary>
        /// OCR　読込
        /// </summary>
        /// <returns></returns>
        public IList<TOcrMoto> GetTOcrMotoList()
        {
            var acc = new BSTOcrMotoAccessor(_Context);
            return acc.Get();
        }

        /// <summary>
        /// 顧客マスタ　読込
        /// </summary>
        /// <returns></returns>
        public IList<MCustomer> GetMCustomerList()
        {
            var acc = new BSMCustomerAccessor(_Context);
            return acc.Get();
        }

        /// <summary>
        /// 顧客マスタの更新
        /// </summary>
        /// <param name="newData">鍵・更新データ</param>
        [Transaction]
        public Response UpdateMCustomer(MCustomer entity)
        {
            var acc = new MCustomerAccessor(_Context);
            var ret = acc.MCustomerCUD(entity);
            return ret;
        }

        /// <summary>
        /// スタッフマスタ　読込
        /// </summary>
        /// <returns></returns>
        public IList<MStaff> GetMStaffList()
        {
            var acc = new BSMStaffAccessor(_Context);
            return acc.Get();
        }

        /// <summary>
        /// 銀行マスタ　読込
        /// </summary>
        /// <returns></returns>
        public IList<MBank> GetMBankList()
        {
            var acc = new BSMBankAccessor(_Context);
            return acc.Get();
        }

        /// <summary>
        /// 案件マスタ　読込
        /// </summary>
        /// <returns></returns>
        public IList<MSuit> GetMSuitList()
        {
            var acc = new BSMSuitAccessor(_Context);
            return acc.Get();
        }

        /// <summary>
        /// 案件マスタの更新
        /// </summary>
        /// <param name="newData">鍵・更新データ</param>
        [Transaction]
        public Response UpdateMSuit(MSuit entity)
        {
            var accMCustomer = new MCustomerAccessor(_Context);
            var ret = accMCustomer.MCustomerCUD(entity._MCustomer);

            if(ret.Status)
            {
                var accMSuit = new MSuitAccessor(_Context);
                entity.CustomerCd = entity._MCustomer.CustomerCd;
                ret = accMSuit.MSuitCUD(entity);
            }
            return ret;
        }

        /// <summary>
        /// 文書マスタの更新
        /// </summary>
        /// <param name="newData">鍵・更新データ</param>
        [Transaction]
        public Response UpdateMDocument(MDocument entity)
        {
            var accMDocument = new BSMDocumentAccessor(_Context);
            var ret = accMDocument.MDocumentCUD(entity);

            return ret;
        }

        /// <summary>
        /// OCRトランザクションの登録
        /// </summary>
        /// <param name="newData">鍵・更新データ</param>
        [Transaction]
        public Response InsertOcr(IList<TOcr> ocrList)
        {
            var accTOcr = new BSTOcrAccessor(_Context);
            var ret = accTOcr.TOcrCUD(ocrList);
            return ret;
        }

        /// <summary>
        /// OCRトランザクションの登録
        /// </summary>
        /// <param name="newData">鍵・更新データ</param>
        [Transaction]
        public Response InsertOcrMoto(IList<TOcrMoto> ocrList)
        {
            var accTOcrMoto = new BSTOcrMotoAccessor(_Context);
            var ret = accTOcrMoto.TOcrMotoCUD(ocrList);
            return ret;
        }

        /// <summary>
        /// 入出金表の登録
        /// </summary>
        /// <param name="newData">鍵・更新データ</param>
        [Transaction]
        public Response InsertMDepositTable(IList<MDepositTable> mDepositTableList)
        {
            var acc = new BSMDepositTableAccessor(_Context);
            var ret = acc.MDepositTableCUD(mDepositTableList);
            return ret;
        }

        /// <summary>
        /// 区分マスタの更新
        /// </summary>
        /// <param name="newData">区分・更新データ</param>
        [Transaction]
        public Response UpdateMKubun(MKubun entity)
        {
            var acc = new BSMKubunAccessor(_Context);
            var ret = acc.MKubunCUD(entity);
            return ret;
        }

        /// <summary>
        /// スタッフマスタの更新
        /// </summary>
        /// <param name="newData">鍵・更新データ</param>
        [Transaction]
        public Response UpdateMStaff(MStaff entity)
        {
            var acc = new BSMStaffAccessor(_Context);
            var ret = acc.MStaffCUD(entity);
            return ret;
        }

    }
}