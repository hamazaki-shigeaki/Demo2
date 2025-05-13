using System.Text;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
using DBAccess.Context;
using DBAccess.Service;
using Demo.Common;
using Demo;
using LogWriters;

var builder = WebApplication.CreateBuilder(args);

// 暗号化キー１
const string key_text = "OakPotalWeb2023061011314";

// 暗号化キー２
// 初期化ベクトルを生成するための文字列 (128bit = 16文字)
const string iv_text = "OakPot1234567890";

// 暗号化キーと初期化ベクトルをそれぞれバイト型配列に変換
byte[] key = Encoding.UTF8.GetBytes(key_text);
byte[] iv = Encoding.UTF8.GetBytes(iv_text);

// 暗号化したい文字列
//const string text = "";
//var test = MyEncryptClass.MyEncrypt(text, key, iv);

// 　ライセンスkey　複合
var Licensing = builder.Configuration.GetValue<string>("Api:Licensing");
var Licens = MyEncryptClass.MyDecrypt(Licensing, key, iv);

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Licens);

CommonContext.TemplateFolder = builder.Configuration.GetValue<string>("Api:TemplateFolder");
CommonContext.DownLoadFolder = builder.Configuration.GetValue<string>("Api:DownLoadFolder");
CommonContext.UploadFolder = builder.Configuration.GetValue<string>("Api:UploadFolder");


// ログ出力設定
builder.Logging.AddLog4Net();


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDevExpressBlazor(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
    options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
});

// Syncfusion サービス設定
builder.Services.AddSyncfusionBlazor();

// 環境設定
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

try
{
    // SqlServer Db接続設定 プーリングモード接続
    builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(config.GetConnectionString("SQLServerConnection"));
    });

    // PostgreSQL Db接続設定 プーリングモード接続
    //builder.Services.AddDbContextPool<ApplicationDbContext>(option =>
    //{
    //    option.EnableSensitiveDataLogging();
    //    option.UseNpgsql(config.GetConnectionString("PostgreSQLConnection"));
    //});

    // セッション用
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromSeconds(600000);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.AddDevExpressServerSideBlazorPdfViewer();

    // 汎用データアクセスサービス追加
    builder.Services.AddScoped<CommonDataService>();

    // APIアクセスサービス追加
    builder.Services.AddScoped<MyApiService>();

    builder.WebHost.UseWebRoot("wwwroot");
    builder.WebHost.UseStaticWebAssets();

    var app = builder.Build();

    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    // ローカライズ　設定
    var supportedCultures = new[] { new CultureInfo("ja-JP") };
    var localizationOptions = new RequestLocalizationOptions
    {
        DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ja-JP"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    };
    app.UseRequestLocalization(localizationOptions);

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    app.UseHttpsRedirection();

    app.UseRouting();

    app.MapHub<BlazorChatSampleHub>(BlazorChatSampleHub.HubUrl);

    app.UseStaticFiles();

    // セッション機能追加
    app.UseSession();

    app.UseRouting();

    app.MapControllers();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();
}
catch (Exception ex)
{
    LogWriter.Error(ex);
}