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

// �Í����L�[�P
const string key_text = "OakPotalWeb2023061011314";

// �Í����L�[�Q
// �������x�N�g���𐶐����邽�߂̕����� (128bit = 16����)
const string iv_text = "OakPot1234567890";

// �Í����L�[�Ə������x�N�g�������ꂼ��o�C�g�^�z��ɕϊ�
byte[] key = Encoding.UTF8.GetBytes(key_text);
byte[] iv = Encoding.UTF8.GetBytes(iv_text);

// �Í���������������
//const string text = "";
//var test = MyEncryptClass.MyEncrypt(text, key, iv);

// �@���C�Z���Xkey�@����
var Licensing = builder.Configuration.GetValue<string>("Api:Licensing");
var Licens = MyEncryptClass.MyDecrypt(Licensing, key, iv);

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Licens);

CommonContext.TemplateFolder = builder.Configuration.GetValue<string>("Api:TemplateFolder");
CommonContext.DownLoadFolder = builder.Configuration.GetValue<string>("Api:DownLoadFolder");
CommonContext.UploadFolder = builder.Configuration.GetValue<string>("Api:UploadFolder");


// ���O�o�͐ݒ�
builder.Logging.AddLog4Net();


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDevExpressBlazor(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
    options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
});

// Syncfusion �T�[�r�X�ݒ�
builder.Services.AddSyncfusionBlazor();

// ���ݒ�
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

try
{
    // SqlServer Db�ڑ��ݒ� �v�[�����O���[�h�ڑ�
    builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(config.GetConnectionString("SQLServerConnection"));
    });

    // PostgreSQL Db�ڑ��ݒ� �v�[�����O���[�h�ڑ�
    //builder.Services.AddDbContextPool<ApplicationDbContext>(option =>
    //{
    //    option.EnableSensitiveDataLogging();
    //    option.UseNpgsql(config.GetConnectionString("PostgreSQLConnection"));
    //});

    // �Z�b�V�����p
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromSeconds(600000);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.AddDevExpressServerSideBlazorPdfViewer();

    // �ėp�f�[�^�A�N�Z�X�T�[�r�X�ǉ�
    builder.Services.AddScoped<CommonDataService>();

    // API�A�N�Z�X�T�[�r�X�ǉ�
    builder.Services.AddScoped<MyApiService>();

    builder.WebHost.UseWebRoot("wwwroot");
    builder.WebHost.UseStaticWebAssets();

    var app = builder.Build();

    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    // ���[�J���C�Y�@�ݒ�
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

    // �Z�b�V�����@�\�ǉ�
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