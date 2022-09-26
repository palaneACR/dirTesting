using Contracts;
using Dicom;
using Entities.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.DataTransferObject.Localizer;
using System.Diagnostics;

namespace Dir.DataExtraction.Localizer;
public class LocalizerProcessor : ILocalizerProcessor
{
    private readonly ILogger<LocalizerProcessor> _logger;

    //public LocalizerProcessor(ILogger<LocalizerProcessor> logger)
    //{
    //    _logger = logger;
    //}

    public async Task<LocalizerDTO> GetCalculatedResultAsync(string filePath)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = new LocalizerDTO();
        try
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            try
            {
                processStartInfo.Arguments = await GetArguments(filePath);
                processStartInfo.WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Engine");
                processStartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine("Engine", "LocalizerEngine.exe"));
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
            }catch(Exception ex)
            {
                throw new LocalizerEngineAcessException(1002);
            }

            using var process = System.Diagnostics.Process.Start(processStartInfo);
            await process.WaitForExitAsync();
            var standardOutput = await process.StandardOutput.ReadToEndAsync();
            var standardError = await process.StandardError.ReadToEndAsync();
            var exitCode = process.ExitCode;
            if (exitCode != 0)
            {
                result.StdOrErrOut = $"DIR: ExitCode= {exitCode} {(string.IsNullOrEmpty(standardError) ? string.Empty : standardError)}";
            }

            var lines = standardOutput.Split('\n');
            if (lines is not null && lines.Length <= 2)
            {
                var topLineValue = lines[0].Split(' ');
                if (topLineValue is not null && topLineValue.Length == 2)
                {
                    result.Thickness = Convert.ToDecimal(topLineValue[1].Trim());
                    result.Orientation = topLineValue[0].Trim();
                    result.Unit = "mm";
                    result.StdOrErrOut = standardOutput;
                }
            }
            else
            {
                result.StdOrErrOut = standardOutput;
            }
        }
        catch (System.Exception ex)
        {
            result.StdOrErrOut = $"DIR: Error occured in durin calculating !\n {ex.Message} \n {ex.StackTrace}";
        }
        stopwatch.Stop();
        return result;
    }

    private async Task<string> GetArguments(string path)
    {
        DicomDataset dataset = new DicomDataset();
        try
        {
            var file = await DicomFile.OpenAsync(path);
            dataset = file.Dataset;
        }
        catch(Exception Ex)
        {
            throw new NotDicomFileException(Ex.Message);
        }       
        var pixelSpacing = dataset.GetString(DicomTag.PixelSpacing);
        var imageOrientationPatient = dataset.GetString(DicomTag.ImageOrientationPatient);
        try
        {
            if (!string.IsNullOrEmpty(pixelSpacing) && !string.IsNullOrEmpty(imageOrientationPatient))
            {
                var pxSpacingValues = pixelSpacing.Split('\\');
                var imageOrientationValues = imageOrientationPatient.Split('\\');

                if ((pxSpacingValues != null && pxSpacingValues.Length >= 2) && (imageOrientationValues != null && imageOrientationValues.Length >= 6))
                {
                    return $" {path} {pxSpacingValues[0]} {pixelSpacing[1]} {imageOrientationValues[0]} {imageOrientationValues[5]} ";
                }
            }
            else
            {
                return $" {path}";
            }
        }
        catch (Exception ex)
        {
            throw new LocalizerFileArgumentsExtractionException(1001);
        }
        return string.Empty;
    }
}
