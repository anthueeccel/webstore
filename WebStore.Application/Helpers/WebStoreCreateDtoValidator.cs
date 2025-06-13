using WebStore.Application.Dtos.WebStore;

namespace WebStore.Application.Helpers
{
    public class WebStoreCreateDtoValidator
    {
        public static (bool,string) Validate(WebStoreCreateDto webStoreCreateDto)
        {
            if (webStoreCreateDto == null)
            {
                return (false, "Web store data is null.");
            }
            if (string.IsNullOrWhiteSpace(webStoreCreateDto.Name))
            {
                return (false, "Please provide a Web store Name.");
            }
            if (string.IsNullOrWhiteSpace(webStoreCreateDto.Description))
            {
                return (false, "Please provide a Web store Description.");
            }
            if (string.IsNullOrWhiteSpace(webStoreCreateDto.ContactEmail) || !webStoreCreateDto.ContactEmail.Contains('@'))
            {
                return (false, "Please provide a Web store Contact Email.");
            }
            return (true, "WebStoreCreateDto required data is OK.");
        }
    }
}
