using System;
using System.IO;
using System.Net.Http.Headers;
using Demo.Model;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;

namespace Demo.Pages
{
    /// <summary>
    /// OCR 取込(バッチ)
    /// </summary>
    public partial class OcrImport : Base
    {
        [Inject]
        private NavigationManager _NavigationManager { get; set; }

        private IGrid Grid { get; set; } = new DxGrid();
        private string FolderPath { get; set; } = @"C:\OBS\git\Deposits-Withdrawals\Demo\Demo\wwwroot\Upload";
        private bool PopupVisible { get; set; } = false;
        private bool UploadVisible { get; set; } = false;

        private List<FilePathModel> FilePathModelList = new List<FilePathModel>();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new List<FilePathModel>();

        protected override async Task Init()
        {
            LogWriters.LogWriter.Info("MenteMCustomer 画面");
            LoadFiles();
            await Task.Delay(2);
        }

        private void LoadFiles()
        {
            var folderPath = Path.Combine(FolderPath, SessionInfo._MSuit.SuitNo);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Console.WriteLine("フォルダを作成しました: " + folderPath);
            }

            FilePathModelList = Directory.GetFiles(folderPath)
                .Select(file => new FilePathModel { FileName = Path.GetFileName(file) })
                .ToList();

            SelectedDataItems = FilePathModelList;
        }

        protected async Task SelectedFilesChanged(IEnumerable<UploadFileInfo> files)
        {
            UploadVisible = files.Any();
            await InvokeAsync(StateHasChanged);
        }

        protected string GetUploadUrl(string url) => _NavigationManager.ToAbsoluteUri(url).AbsoluteUri;

        void OnFileUploadStart(FileUploadStartEventArgs e)
        {
            e.RequestData.Add("SuitNo", SessionInfo._MSuit.SuitNo);
        }

        private void OnButtonClose()
        {
            LoadFiles();
            PopupVisible = false;
        }

        private async Task OnButtonTorikomi()
        {
            PopupVisible = true;
            await Task.Delay(10);
        }

        private async Task OnButtonBatch()
        {
        }

        private async Task OnButtonDelete()
        {
            var folderPath = Path.Combine(FolderPath, SessionInfo._MSuit.SuitNo);

            foreach (FilePathModel data in SelectedDataItems)
            {
                var filePath = Path.Combine(folderPath, data.FileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine("ファイルを削除しました。");
                }
                else
                {
                    Console.WriteLine("指定されたファイルが存在しません。");
                }
            }

            LoadFiles();
            SelectedDataItems = new List<FilePathModel>();
            await Task.Delay(10);
        }
    }
}