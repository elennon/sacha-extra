
using System.IO;
using System.Threading.Tasks;

namespace Extras.Models
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
