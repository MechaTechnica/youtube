namespace PythonHospitalDemo.Services
{
    public interface IFileService
    {
        string FilePrompt(string filter);

        string ReadFile(string fileName);
    }
}
