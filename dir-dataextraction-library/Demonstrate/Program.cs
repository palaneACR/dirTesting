// See https://aka.ms/new-console-template for more information
using Contracts;
using Dicom;
using Dir.DataExtraction.Localizer;
using Dir.DataExtraction.RDSR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

//var path = @"\\acr.org\Shares\MIS\DIR\Dicom files\RDSR-Localizer\CT_RDSR_Test_WithLoc\412034443-RDSR-7798baf7-e.1183275518724ace9f16454da1f90282.f25ae759ab.dcm";
//var path = "C:\\Users\\aspavankumar\\Downloads\\Localizer\\Localizer\\loc_45446732_test.txt";
var path = "C:\\Users\\aspavankumar\\Downloads\\CT_Localizer\\CT_Localizer\\test.dcm";
    //var serviceProvider = new ServiceCollection()
    //.AddSingleton<IRDSRProcessor>(new RDSRProcessor(path))
    //.BuildServiceProvider();
    var serviceProvider1 = new ServiceCollection()
        .AddSingleton<ILocalizerProcessor>(new LocalizerProcessor())
        .BuildServiceProvider();

    //var rdsrProcessor = servicePro0vider.GetService<IRDSRProcessor>();

    var rdsrProcessor1 = serviceProvider1.GetService<ILocalizerProcessor>();

    try
    {
        //var res = await rdsrProcessor.GetRdsrObjectAsync();
        var res1 = await rdsrProcessor1.GetCalculatedResultAsync(path);
        var json = JsonConvert.SerializeObject(res1);

       // Console.WriteLine(json);
    }
    catch (System.Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

