using System.Threading.Tasks;

public interface IShare {
    Task Show(string title, string message, string filePath);
}

